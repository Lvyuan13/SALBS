using System.Collections;
using System;
using System.Collections.Generic;
namespace LunarParametricNumeric.Modules
{
	public class Transport: Module
	{
		public override List<Resources> getRegisteredResources(){
            return new List<Resources>() { 
                Resources.ElecticalEnergy;
            };
        } 

        public override double getModuleVolume(){
            return 10.0;
        }

        public override string moduleName {
            get { return "TransportModule"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Transport Module"; }
        }

        protected override void update(UInt64 clock) {
        	/*	If the time during the simulation is between
        		Midnight and 6AM then charge the rover. If
        		we assume a 120 Ah battery and a charge current
        		of 20 A then it takes six hours to charge the
        		battery fully. The voltage of charging is the 
        		basewide voltage, assumed here to be 36V. */
        	if ((double)clock/86400 > 0.0 && (double)clock/86400 < 21600) {
        		float voltage = 36; //[V]
        		float current = 20; //[A]

        		float energyConsumed = voltage * current * 1/1000; //[kJ]
        		consumeResource(Resources.ElecticalEnergy, energyConsumed);
        	}

        }
	}
}