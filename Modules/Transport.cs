using System.Collections;
using System;
using System.Collections.Generic;
namespace LunarParametricNumeric.Modules
{
	public class Transport: Module
	{
		/*	The LRV is just being monitered as a battery that
			will be recharged whenever it is back at the base.
		 */
		private float batterySize; //[Ah]
		private float batteryCharge; //[Ah]

		/*	Figure out a way to model when the rover is driving,
			possibly could be done using a probablity method, or
			by simpling seeing what time of day it is.
		 */
		private boolean isDriving;

		private boolean isCharged;

		/*	Need to change this value into something meaningful. It's
			meant to be the amount of Oxygen per second that a person
			would consume whilst driving the LRV. At the moment it's 
			just a dummy variable to allow the code to be written.
		 */
		private float humanOxygenIntake = 10.0f;

		/*	Need to also change this value to the amount of enthalpy
			produced by the rover and the humans when it is in 
			operation. Currently just a dummmy variable.
		 */
		private float enthalpyProduced = 10.0f;

		public Transport(Simulation sim, int moduleid) : base(sim,moduleid) {}

		public override List<Resources> getResources() {
			return new List<Resources>() {
				Resources.O,
				Resources.Enthalpy
			};
		}

		public override string getModuleName(){
            return "Transport";
        }

        public override string getModuleFriendlyName(){
            return "Transport";
        }

        public override void setLoad(float value) {}

        protected override void update(UInt64 clock) {
        	
        	/*	Need to insert code here to essentialy decide when
        		the rover is driving based off of the clock.
        	 */


        	if (isDriving) {
        		consumeResource(Resources.O, Convert.ToSingle(humanOxygenIntake));
        		produceResource(Resources.Enthalpy, Convert.ToSingle(enthalpyProduced));
        		
        		/*	Need to change this value. This is assuming that every
        			second the car is driving the charge in the battery drops
        			0.01 Ah. Don't even know if that makes sense.
        		 */
        		if (batteryCharge >= 0) {
        			batteryCharge -= 0.01;
        		} else {
        			isCharged = false;
        		}
        	}

        	else if(!isCharged) {
        		if (batteryCharge >= batterySize) {
        			isCharged = true;
        			batteryCharge = batterySize;
        		} else {
        			batteryCharge += 0.01;
        		}
        	}
        }

        // Set the size of the battery in ampere hours
		public void setBatterySize(float size) {
			batterySize = size;
			batteryCharge = batterySize;
			isCharged = true;
		}
	}
}