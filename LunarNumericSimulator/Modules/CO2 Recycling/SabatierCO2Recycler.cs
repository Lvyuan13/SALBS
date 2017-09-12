using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules
{
    class SabatierCO2Recycler: Module
    {
        protected PIDController pid;

        [NumericConfigurationParameter("P Gain", "2", "double", false)]
        public double PGain { private get; set; }
        [NumericConfigurationParameter("I Gain", "0.1", "double", false)]
        public double IGain { private get; set; }
        [NumericConfigurationParameter("D Gain", "0.01", "double", false)]
        public double DGain { private get; set; }
        [NumericConfigurationParameter("Nominal CO2 Volume %", "0.005", "double", false)]
        public double DesiredCO2Level { private get; set; }
        // Functions
        public SabatierCO2Recycler(Simulation sim, int moduleid) : base(sim, moduleid) {


  
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.CO2,
                Resources.H,
                Resources.H2O,
                Resources.CH4,
                Resources.ElecticalEnergy,
                Resources.Heat
            };
        }

        public override void ModuleReady()
        {
            pid = new PIDController(PGain, IGain, DGain, 50);
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override string moduleName
        {
            get { return "SabatierCO2Recycler"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Sabatier CO2 Recycler"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> { "SabatierMethane", "ActiveThermalLoop" };
        }

        protected override void update(UInt64 clock)
        {
            // update the PID
            double CO2Level = getAtmosphericMolarFraction(Resources.CO2);
            double CO2mass = getResourceLevel(Resources.CO2);

            var result = pid.update(CO2Level - DesiredCO2Level, 1); // check for acceptable CO2 level

            if (result < 0)
            {
                pid.removeWindup();
                return;
            }
                


            // Bosch reactor figures obtained from:
            // "Spaceflight life supprt and biospherics" by Peter Eckart

            // Inflow gas composed of:
            // 84.6% Hydrogen
            // 15.4% CO2
            // At a mass flow rate of 0.196 [kg/day] or 0.00000226851 [kg / s]
            double flowRateInPerSecond = result;      // [kg]

            double sabatierCO2Consumed = flowRateInPerSecond * 0.154;      // [kg] 
            double sabatierH2Consumed = flowRateInPerSecond * 0.846;      // [kg] 

            // Outflow consists of 
            // 0.136 kg/day of Water
            // 0.06 kg/day of methane
            double sabatierH2OProduced = 0.69* result;     // [kg] 
            double sabatierCH4Produced = 0.31 * result;      // [kg] 


            // TODO pass the power and heat generation information to the resource managers
            // The power required for a Sabatier reactor is:

            //TODO explain the numbers below: 118139 (kj/kg) = 0.268 / 0.00000226851
            // 22041 (kj/kg) = 0.05 / 0.00000226851
            double sabatierPower = 22041 * Math.Abs(result);                               // [kJ]
            double sabatierHeatGeneration = 118061 * Math.Abs(result);                     // [kJ]


            // Consume and produce all resources
            consumeResource(Resources.CO2, sabatierCO2Consumed);
            consumeResource(Resources.H, sabatierH2Consumed);
            consumePower(sabatierPower);

            produceResource(Resources.H2O, sabatierH2OProduced);
            produceResource(Resources.Heat, sabatierHeatGeneration*0.01);
            getTank("ActiveThermalLoop").addResource(sabatierHeatGeneration * 0.99);
            getTank("SabatierMethane").addResource(sabatierCH4Produced);
        }




    }
}
