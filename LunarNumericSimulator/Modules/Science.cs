using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator;
using LunarNumericSimulator.Utilities;

namespace LunarParametricNumeric.Modules
{
    public class Science : Module
    {
        [NumericConfigurationParameter("Startup Time (hour < 24)", "0.5", "double", false)]
        public double StartupTime { get; set; }
        [NumericConfigurationParameter("Shutdown Time (hour <= 24)", "3", "double", false)]
        public double ShutdownTime { get; set; }

        [NumericConfigurationParameter("Power Consumption (kW)", "5", "double", false)]
        public double PowerConsumption { get; set; }
        [NumericConfigurationParameter("Thermal Efficiency", "0.78", "double", false)]
        public double ThermalEfficiency { get; set; }



        public Science(Simulation sim, int moduleid) : base(sim, moduleid)
        {
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.Heat,
                Resources.ElecticalEnergy
            };
        }

        public override void ModuleReady()
        {
        }

        /*  This value comes from "The Lunar Base Handbook"
            by Peter Eckhart in the section "Habitats,
            Laboratories, and Airlocks".
         */
        public override double getModuleVolume()
        {
            return 100.0;
        }

        public override string moduleName
        {
            get { return "ScienceModule"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Science Module"; }
        }

        public void setLoad(double value) { }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }


        protected override void update(UInt64 clock)
        {
            if (ThermalEfficiency > 1 || ThermalEfficiency < 0)
                throw new Exception("Thermal Efficiency cannot be greater than 1 or less than 0!");
            if (StartupTime < 0 || StartupTime >= 24)
                throw new Exception("Invalid startup time!");
            if (ShutdownTime <= 0 || ShutdownTime > 24)
                throw new Exception("Invalid shutdown time!");

            var startupTimeSeconds = StartupTime * 60 * 60;
            var shutdownTimeSeconds = ShutdownTime * 60 * 60;
            var result = (1 / (1 + Math.Exp(-0.05 * (clock - startupTimeSeconds)))) * (1 / (1 + Math.Exp(0.05 * (clock - shutdownTimeSeconds))));
            consumePower(PowerConsumption*result);
            produceResource(Resources.Heat, PowerConsumption * result * (1-ThermalEfficiency));

        }

    }
}