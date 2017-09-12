using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules.H2O_Management
{
    class MFBWaterProcessor : Module
    {
        PIDController pid;
        [NumericConfigurationParameter("P Gain", "0", "double", false)]
        public double PGain { private get; set; }
        [NumericConfigurationParameter("I Gain", "0.0005", "double", false)]
        public double IGain { private get; set; }
        [NumericConfigurationParameter("D Gain", "0", "double", false)]
        public double DGain { private get; set; }
        // Figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart
        // power used is 0.00038kW for inflow of 32.65 [kg/day] = 0.00131944 [kg/s]
        // heat generated is 0.00038kW
        private double designPower;       // [kJ]
        private double designInflow;         // [kg]
        private double designHeatProduced;

        public MFBWaterProcessor(Simulation sim, int moduleid) : base(sim,moduleid)
        {


        }

        public override void ModuleReady()
        {
            pid = new PIDController(PGain, IGain, DGain, 1);
        }


        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.ElecticalEnergy,
                Resources.H2O,
                Resources.Heat
            };
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override string moduleName
        {
            get { return "MFBWaterProcessor"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Multi Filtration Bed Water Processor"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> {"WasteWaterStorage"};
        }


        protected override void update(UInt64 clock)
        {

            double currentWasteLevel = getTank("WasteWaterStorage").getLevel();
            var result = pid.update(currentWasteLevel, 1);

            if (currentWasteLevel -  result < 0.0)
            {
                result = currentWasteLevel;
            }

            designInflow = result;
            designPower = (0.00038 / 0.00131944) * result;
            designHeatProduced = (0.00038 / 0.00131944) * result;

            consumePower(designPower);
            getTank("WasteWaterStorage").consumeResource(designInflow);
            // (design inflow equals design outflow, assuming all water is converted into potable.)
            produceResource(Resources.H2O, designInflow);
            produceResource(Resources.Heat, designHeatProduced);
        }

    }
}

