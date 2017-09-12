using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator;
using LunarNumericSimulator.Utilities;

namespace LunarParametricNumeric.Modules
{
    public class Transport : Module
    {
        /*	The LRV is just being monitered as a battery that
			will be recharged whenever it is back at the base.
		 */
        protected PIDController pid;
        private double batterySize; //[Ah]
        private double batteryCharge; //[Ah]
        [NumericConfigurationParameter("P Gain", "2", "double", false)]
        public double PGain { private get; set; }
        [NumericConfigurationParameter("I Gain", "0.1", "double", false)]
        public double IGain { private get; set; }
        [NumericConfigurationParameter("D Gain", "0.01", "double", false)]
        public double DGain { private get; set; }
        /*	Figure out a way to model when the rover is driving,
			possibly could be done using a probablity method, or
			by simpling seeing what time of day it is.
		 */
        private bool isDriving;
        private bool isCharged;

        public override double getModuleVolume()
        {
            return 0;
        }

        public Transport(Simulation sim, int moduleid) : base(sim, moduleid)
        {
            isDriving = false;
            isCharged = true;
        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.Heat,
                Resources.ElecticalEnergy
            };
        }

        public override void ModuleReady()
        {
            pid = new PIDController(PGain, IGain, DGain, 50);
        }

        public override string moduleName
        {
            get { return "Transport"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Transport"; }
        }

        public void setLoad(double value) { }

        public override List<string> requiresTanks()
        {
            return new List<string>();
        }


        /*	Add changes to allow ConsumeResource.Energy()
         */
        protected override void update(UInt64 clock)
        {

            /*	Need to insert code here to essentialy decide when
        		the rover is driving based off of the clock.
        	 */


            if (isDriving)
            {

                /*	Need to change this value. This is assuming that every
        			second the car is driving the charge in the battery drops
        			0.01 Ah. Don't even know if that makes sense.
        		 */
                if (batteryCharge >= 0)
                {
                    batteryCharge -= 0.01;
                }
                else
                {
                    isCharged = false;
                }
            }

            else if (!isCharged)
            {
                if (batteryCharge >= batterySize)
                {
                    isCharged = true;
                    batteryCharge = batterySize;
                }
                else
                {
                    batteryCharge += 0.01;
                }
            }
        }

        // Set the size of the battery in ampere hours
        public void setBatterySize(double size)
        {
            batterySize = size;
            batteryCharge = batterySize;
            isCharged = true;
        }
    }
}