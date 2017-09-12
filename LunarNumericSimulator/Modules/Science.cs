using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator;
using LunarNumericSimulator.Utilities;

namespace LunarParametricNumeric.Modules
{
    public class Science : Module
    {
        protected PIDController pid;
        [NumericConfigurationParameter("P Gain", "2", "double", false)]
        public double PGain { private get; set; }
        [NumericConfigurationParameter("I Gain", "0.1", "double", false)]
        public double IGain { private get; set; }
        [NumericConfigurationParameter("D Gain", "0.01", "double", false)]
        public double DGain { private get; set; }



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
            pid = new PIDController(PGain, IGain, DGain, 50);
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
            /*	If the time during the simulation is between
            Midnight and 6AM then charge the rover. If
            we assume a 120 Ah battery and a charge current
            of 20 A then it takes six hours to charge the
            battery fully. The voltage of charging is the 
            basewide voltage, assumed here to be 36V. */
            if ((double)clock / 86400 > 0.0 && (double)clock / 86400 < 21600)
            {
                float voltage = 36; //[V]
                float current = 20; //[A]

                float energyConsumed = voltage * current * 1 / 1000; //[kJ]

                consumePower(energyConsumed);
            }

        }

    }
}