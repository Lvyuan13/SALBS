using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules.H2O_Management
{
    class VCDUrineProcessor : Module
    {

        protected PIDController pid = new PIDController(0, 0.0005, 0);

        // Power requirement figures obtained from:
        // "Spaceflight life supprt and biospherics" by Peter Eckart
        // power used is 0.115kW inflow of 32.65 [kg/day] = 0.00037789352 [kg/s]
        // heat is 0.115kW
        // TODO designed efficiency is 70%, is this factored in already or should the output be reduced by 30%?

        private double designPowerRequired;       // [kJ]
        private double designLiquidInFlow;    // [kg]
        private double designHeatProduced;      //kJ

        public VCDUrineProcessor(Simulation sim, int moduleid) : base(sim, moduleid)
        {

        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.ElecticalEnergy,
                Resources.Heat
            };
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override string moduleName
        {
            get { return "VCDUrineProcessor"; }
        }

        public override string moduleFriendlyName
        {
            get { return "VCD Urine Processor"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> {   "UrineTreatmentTank",
                                        "WasteWaterStorage"};
        }


        protected override void update(UInt64 clock)
        {

            double currentUrineLevel = getTank("UrineTreatmentTank").getLevel();

            var result = pid.update(currentUrineLevel, 1);

            if (currentUrineLevel - result < 0.0)
            {
                result = currentUrineLevel;
            }

            designLiquidInFlow = result;
            designPowerRequired = (.115 / 0.00037789352) * result;
            designHeatProduced = (.115 / 0.00037789352) * result;

            consumePower(designPowerRequired);
            getTank("UrineTreatmentTank").consumeResource(designLiquidInFlow);
            // assume all inflow is converted to outflow
            getTank("WasteWaterStorage").addResource(designLiquidInFlow);
            produceResource(Resources.Heat, designHeatProduced);
        }

    }
}

