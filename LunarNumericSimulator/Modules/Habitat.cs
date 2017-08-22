using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Habitat : Module
    {
        Random rand = new Random();
        int setpoint = 0;
        public Habitat(Simulation sim, int moduleid) : base(sim, moduleid)
        {
            setpoint = rand.Next(20);
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
            int time = Convert.ToInt32(clock) - 100;
            var value = rand.Next(10);
            produceResource(Resources.Heat, (value / (1+Math.Exp(-time))));
        }


    }
}