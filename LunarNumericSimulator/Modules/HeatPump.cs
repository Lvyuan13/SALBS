using LunarNumericSimulator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Modules
{
    class HeatPump : Module
    {
        PIDController pid = new PIDController(0.5, 0.4, 1);
        bool passed = false;
        public HeatPump(Simulation sim, int id) : base(sim, id)
        {
        }

        public override string moduleName {
            get { return "HeatPump"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Heat Pump"; }
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>()
            {
                Resources.Heat,
                Resources.ElecticalEnergy
            };
        }

        protected override void update(ulong clock)
        {
            var result = pid.update(getAirTemperature() - 25, 1);

            if (result < 0)
                return;
            consumeResource(Resources.Heat, result);
            consumePower(result / 0.15); // Assume the heat pump is 15% efficient
        }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }
    }
}
