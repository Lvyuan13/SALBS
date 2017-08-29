using LunarNumericSimulator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarNumericSimulator.Modules
{
    class VentilationSystem : Module
    {

        public VentilationSystem(Simulation sim, int id) : base(sim, id)
        {
        }

        public override string moduleName
        {
            get { return "VentilationSystem"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Ventilation System"; }
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>()
            {
                Resources.ElecticalEnergy,
                Resources.Heat
            };
        }

        protected override void update(ulong clock)
        {
            double energyUse = Math.Pow(300, -3);
            double efficiency = 0.9;
            produceResource(Resources.Heat, energyUse * (1-efficiency));
            consumeResource(Resources.ElecticalEnergy, energyUse * efficiency); // TODO: get real numbers, right now we assume it uses 300W
        }
    }
}
