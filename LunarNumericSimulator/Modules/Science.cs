using System.Collections;
using System;
using System.Collections.Generic;
namespace LunarParametricNumeric.Modules
{
	public class Science: Module
	{
        private float energyUse, startTime, finishTime;

		public override List<Resources> getRegisteredResources(){
            return new List<Resources>() { 
                Resources.ElecticalEnergy;
            };
        } 

        // This is just a dummy variable.
        public override double getModuleVolume(){
            return 10.0;
        }

        public override string moduleName {
            get { return "ScienceModule"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Science Module"; }
        }

        protected override void update(UInt64 clock) {
            if (clock > startTime && clock < finishTime) {
                consumeResource(Resources.ElecticalEnergy, energyUse);
            }
        }

        /*  Allow the user to set the amount of energy
            useage required for the module as specified
            in the prelim report. This will be the energy
            used in kW and will then be converted
            to kJ to be consumed every tick.
         */
        public void setUseage(float energy, float start, float finish) {
            energyUse = energy;
            startTime = start;
            finishTime = finish;
        }
	}
}