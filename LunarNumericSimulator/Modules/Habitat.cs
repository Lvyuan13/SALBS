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

            };
        }

        public override float getModuleVolume()
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

        }


    }
}