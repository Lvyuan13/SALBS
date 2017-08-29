using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Habitat : Module
    {
        public Habitat(Simulation sim, int moduleid) : base(sim, moduleid)
        {
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.Heat
            };
        }

        public override double getModuleVolume()
        {
            return 70;
        }

        public override string moduleName
        {
            get { return "Habitat"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Habitat"; }
        }

        protected override void update(UInt64 clock)
        {
            // TODO: Must model the heat transfer behaviour of the spacecraft hull in here
        }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }
    }
}