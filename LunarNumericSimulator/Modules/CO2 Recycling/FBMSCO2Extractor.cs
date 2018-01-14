using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules
{
    class FMSCO2Extractor : Module
    {
        protected PIDController pid;

        [NumericConfigurationParameter("P Gain", "2", "double", false)]
        public double PGain { get; set; }
        [NumericConfigurationParameter("I Gain", "0.01", "double", false)]
        public double IGain { get; set; }
        [NumericConfigurationParameter("D Gain", "0.0", "double", false)]
        public double DGain { get; set; }
        [NumericConfigurationParameter("Nominal CO2 Volume %", "0.005", "double", false)]
        public double DesiredCO2Level { get; set; }
        // Functions
        public FMSCO2Extractor(Simulation sim, int moduleid) : base(sim, moduleid)
        {



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
            get { return "FBMSCO2Extractor"; }
        }

        public override string moduleFriendlyName
        {
            get { return "4 bed Molecular Sieve CO2 Extractor"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> { "ExtractedCO2" };
        }

        protected override void update(UInt64 clock)
        {
            // update the PID
            double CO2Level = getAtmosphericMolarFraction(Resources.CO2);

            var result = pid.update(CO2Level - DesiredCO2Level, 1); // check for acceptable CO2 level

            if (result < 0)
            {
                pid.removeWindup();
                return;
            }

            // extractor figures obtained from:
            // "Spaceflight life supprt and biospherics" by Peter Eckart
            // design is to support 3 people per day, this is approximately to remove 3kg per day
            //double designPower = 0.535;  // [kW]
            //double efficiency = 0.66;  
            //double inflowCO2 = 3;   // [kg](CO2) per day

            // only removes CO2
            // At a mass flow rate of 3 [kg/day] or 0.0000347222 [kg / s] using 0.535kw of power
            // 0.535 / 0.0000347222 = 15408.01 kj/kg
            double flowRateInPerSecond = result;      // [kg]
            double extractorPower = 15408.01 * Math.Abs(result);

            // Consume and produce all resources
            consumeResource(Resources.CO2, result);
            consumePower(extractorPower);
            getTank("ExtractedCO2").addResource(result);
        }




    }
}
