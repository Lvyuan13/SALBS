using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules.H2O_Management
{
    abstract class UrineProcessor : Module
    {
        protected PIDController pid = new PIDController(0.001, 0.0005, 0.0002, 1);
        protected bool changeResources = true;

        protected double maximumUrineStorageTankLevel;
        protected double nominalUrineStorageTankLevel;

        protected bool allowFilling;
        protected double margin;

        public UrineProcessor(Simulation sim, int moduleid) : base(sim, moduleid)
        {
            // set maximum tank level for unrine
            maximumUrineStorageTankLevel = 50;
            // set nominal tank level for urine
            nominalUrineStorageTankLevel = 5;

            allowFilling = true;
            margin = 0.01;
        }

        public override void ModuleReady()
        {
        }

        public override List<string> requiresTanks()
        {
            return new List<string> {"UrineTreatmentTank"};
        }

        protected double updatePID()
        {
            double currentUrineLevel = getTank("UrineTreatmentTank").getLevel();

            // initialise assuming allowFilling is true
            var result = 0.0;
            // causes it to convege on nominal (nominal can't be 0 exactly because of devision)
            
            

            // allow filling if the level is below nominal
            if (currentUrineLevel <= nominalUrineStorageTankLevel)
                allowFilling = true;

            // if current urine level is approximately nominal (+- margin)
            if (currentUrineLevel <= nominalUrineStorageTankLevel + margin && currentUrineLevel >= nominalUrineStorageTankLevel - margin)
            {
                // allow it to fill the tank
                allowFilling = true;
            }
            // only if the tank goes above maximum level disable filling (enabling changing of resources)
            else if (currentUrineLevel >= maximumUrineStorageTankLevel)
            {
                allowFilling = false;
            }

            // only update the PID if the tank needs emptying
            if (!allowFilling)
            {
                result = pid.update((currentUrineLevel - nominalUrineStorageTankLevel) / nominalUrineStorageTankLevel, 1);
            }

            

            return result;

        }

    }
}
