using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules.H2O_Management
{
    abstract class WaterProcessor : Module
    {
        protected PIDController pid = new PIDController(0.01, 0.005, 0.02, 1);
        protected bool changeResources = true;

        protected double maximumWasteStorageTankLevel;
        protected double nominalWasteStorageTankLevel;

        protected bool allowFilling;
        protected double margin;

        public WaterProcessor(Simulation sim, int moduleid) : base(sim, moduleid)
        {
            // set maximum tank level for unrine
            maximumWasteStorageTankLevel = 200;
            // set nominal tank level for urine
            nominalWasteStorageTankLevel = 50;

            allowFilling = true;
            margin = 0.01;
        }

        public override void ModuleReady()
        {
        }

        public override List<string> requiresTanks()
        {
            return new List<string> { "WasteWaterStorage" };
        }

        protected double updatePID()
        {
            double currentWasteLevel = getTank("WasteWaterStorage").getLevel();

            // initialise assuming allowFilling is true
            var result = 0.0;
            // causes it to convege on nominal (nominal can't be 0 exactly because of devision)



            // allow filling if the level is below nominal
            if (currentWasteLevel <= nominalWasteStorageTankLevel)
                allowFilling = true;

            // if current waste level is approximately nominal (+- margin)
            if (currentWasteLevel <= nominalWasteStorageTankLevel + margin && currentWasteLevel >= nominalWasteStorageTankLevel - margin)
            {
                // allow it to fill the tank
                allowFilling = true;
            }
            // only if the tank goes above maximum level disable filling (enabling changing of resources)
            else if (currentWasteLevel >= maximumWasteStorageTankLevel)
            {
                allowFilling = false;
            }

            // only update the PID if the tank needs emptying
            if (!allowFilling)
            {
                result = pid.update((currentWasteLevel - nominalWasteStorageTankLevel) / nominalWasteStorageTankLevel, 1);
            }

            return result;

        }


    }

}
