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

        public static double percentageVolumeFlowRate = 0.05;
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

        public override void ModuleReady()
        {
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
            var airState = getAirState();
            double volumetricFlowRate = getSystemVolume() * percentageVolumeFlowRate; // We want air to flow at 5% the total volume of air per second
            double fanArea = 1 * 1; //Assume fan area of 1m2
            double airSpeed = volumetricFlowRate / fanArea;
            double mass = airState.Density * volumetricFlowRate;

            double efficiency = 0.7;
            double energyRequired = 0.5 * mass * airSpeed;
            double energyUsed = energyRequired / efficiency; // Assume 70% efficiency

            produceResource(Resources.Heat, energyUsed * (1-efficiency));
            consumePower(energyUsed * efficiency); 
        }
    }
}
