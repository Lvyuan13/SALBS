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

        PIDController pidAtmosphere;
        PIDController pidLoop;

        [NumericConfigurationParameter("Atmosphere P Gain", "1", "double", false)]
        public double pidAtmospherePGain { private get; set; }
        [NumericConfigurationParameter("Atmosphere I Gain", "0.4", "double", false)]
        public double pidAtmosphereIGain { private get; set; }
        [NumericConfigurationParameter("Atmosphere D Gain", "0", "double", false)]
        public double pidAtmosphereDGain { private get; set; }

        [NumericConfigurationParameter("Loop P Gain", "1", "double", false)]
        public double pidLoopPGain { private get; set; }
        [NumericConfigurationParameter("Loop I Gain", "0.5", "double", false)]
        public double pidLoopIGain { private get; set; }
        [NumericConfigurationParameter("Loop D Gain", "0", "double", false)]
        public double pidLoopDGain { private get; set; }

        [NumericConfigurationParameter("Coefficient of Performance", "7", "double", false)]
        public double coefficientOfPerformance { private get; set; }



        public HeatPump(Simulation sim, int id) : base(sim, id)
        {
        }

        public override void ModuleReady()
        {
            pidAtmosphere = new PIDController(pidAtmospherePGain, pidAtmosphereIGain, pidAtmosphereDGain, 1);
            pidLoop = new PIDController(pidLoopPGain, pidLoopIGain, pidLoopDGain, 1);
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

        public override List<string> requiresTanks()
        {
            return new List<string>()
            {
                "ActiveThermalLoop"
            };
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
            var result = pidAtmosphere.update(getAirState().Temperature - 25, 1);

            if (result > 0)
            {
                consumeResource(Resources.Heat, result);
                // TODO: Consume heat from atmospheric heat control - model convection
                consumePower(result / coefficientOfPerformance);
            }
  



            result = pidLoop.update(getTank("ActiveThermalLoop").getLevel(), 1);

            if (getTank("ActiveThermalLoop").getLevel() - result < 0)
            {
                result = getTank("ActiveThermalLoop").getLevel();
                pidLoop.removeWindup();
            }

            if (result > 0)
            {
                getTank("ActiveThermalLoop").consumeResource(result);
                consumePower(result / coefficientOfPerformance);
            }

            
        }
    }
}
