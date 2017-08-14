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
                Resources.O,
                Resources.CH4
            };
        }

        public override double getModuleVolume()
        {
            return 400;
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
            if (clock > 500)
            {
                produceResource(Resources.CH4, 0.2);
            }
        }


    }
}