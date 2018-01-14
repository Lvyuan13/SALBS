using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules
{
    class BoschCO2Recycler : Module
    {
        protected PIDController pid;

        [NumericConfigurationParameter("P Gain", "2", "double", false)]
        public double PGain { get; set; }
        [NumericConfigurationParameter("I Gain", "0.1", "double", false)]
        public double IGain { get; set; }
        [NumericConfigurationParameter("D Gain", "0.01", "double", false)]
        public double DGain { get; set; }
        [NumericConfigurationParameter("Nomial CO2 Volume %", "0.005", "double", false)]
        public double DesiredCO2Level { get; set; }
        // Functions
        public BoschCO2Recycler(Simulation sim, int moduleid) : base(sim, moduleid)
        {


        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.CO2,
                Resources.H,
                Resources.H2O,
                Resources.CH4,
                Resources.N2,
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
            get { return "BoschCO2Recycler"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Bosch CO2 Recycler"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> { "BoschCarbon", "ActiveThermalLoop", "ExtractedCO2" };
        }

        protected override void update(UInt64 clock)
        {
            // update the PID
            double CO2TankLevel = getTank("ExtractedCO2").getLevel();
            var result = pid.update(CO2TankLevel - DesiredCO2Level, 1); // check for acceptable CO2 level

            if (result < 0)
            {
                pid.removeWindup();
                return;
            }


            // Bosch reactor figures obtained from:
            // "Spaceflight life supprt and biospherics" by Peter Eckart

            // Inflow gas composed of:
            // 54.9% Hydrogen
            // 45.4% CO2
            // 0.1% Nitrogen
            // At a mass flow rate of 0.145 [kg/day] = 0.00000167824 [kg/s]
            double flowRateInPerSecond = result;

            double boschCO2Consumed = flowRateInPerSecond * 0.454;           // [kg] / s
            double boschH2Consumed = flowRateInPerSecond * 0.545;          // [kg] / s
            double boschN2Consumed = flowRateInPerSecond * 0.001;           // [kg] / s

            // outflow consists of:
            // 0.009 kg/day of Water
            // 0.034 kg/day of solid carbon
            // 0.102 kg/day of liquid hydrogen
            double boschN2Produced = 0.0621 * result;                                 // [kg] / s
            // TODO the bosch recycler produces solid carbon for which we don't have a resource manager for
            // maybe we need a way to add new random resources?
            double boschCProduced = 0.234 * result;                                 // [kg] / s
            double boschH2OProduced = 0.703 * result;                              // [kg] / s



            double boschPower = result * (0.239 / 0.00000167824);                                   // [kW]
            double boschHeatGeneration = result * (0.313 / 0.00000167824);                          // [kW]

            // Consume and produce all resources
            getTank("ExtractedCO2").consumeResource(boschCO2Consumed);
            consumeResource(Resources.H, boschH2Consumed);
            consumeResource(Resources.N2, boschN2Consumed);
            consumePower(boschPower); // convert kW to Joules in 1 second

            produceResource(Resources.H2O, boschH2OProduced);
            produceResource(Resources.N2, boschN2Produced);
            produceResource(Resources.Heat, boschHeatGeneration * 0.05);
            getTank("ActiveThermalLoop").addResource(boschHeatGeneration * 0.95);
            getTank("BoschCarbon").addResource(boschCProduced);
            
        }

    }
}
