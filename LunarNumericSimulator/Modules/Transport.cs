using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator;
using LunarNumericSimulator.Utilities;

namespace LunarParametricNumeric.Modules
{
    public class Transport : Module
    {
        /*	The transport is just being modelled as a battery that
			will be recharged whenever it is back at the base.
		 */
        
        [NumericConfigurationParameter("Rover Battery Voltage [V]", "36.0", "double", false)]
        public double batteryVoltage { get; set; }
        [NumericConfigurationParameter("Rover Batter AmpHours [Amp-hrs]", "120.0", "double", false)]
        public double batteryAmpHrs { get; set; }
        [NumericConfigurationParameter("Rover Charging Start Time [hr]", "02.0", "double", false)]
        public double StartupTime { get; set; }
        [NumericConfigurationParameter("Rover Charging End Time [hr]", "05.0", "double", false)]
        public double ShutdownTime { get; set; }


        public override double getModuleVolume()
        {
            return 0;
        }

        public Transport(Simulation sim, int moduleid) : base(sim, moduleid)
        {

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

        protected override void update(UInt64 clock)
        {
            /*	If the time during the simulation is between
            Midnight and 6AM then charge the rover. If
            we assume a 120 Ah battery and a charge current
            of 20 A then it takes six hours to charge the
            battery fully. The voltage of charging is the 
            basewide voltage, assumed here to be 36V. */

            int startSeconds = (int)StartupTime * 60 * 60;
            int endSeconds = (int)ShutdownTime * 60 * 60;

            double currentRequired = batteryAmpHrs / ( ShutdownTime - StartupTime);  // [A]

            if ((int)clock > startSeconds && (int)clock <= endSeconds)
            {
                double energyConsumed = batteryVoltage * currentRequired * 1 / 1000; //[kJ]

                var result = (1 / (1 + Math.Exp(-0.05 * ((int)clock - startSeconds)))) * (1 / (1 + Math.Exp(0.05 * ((int)clock - endSeconds))));
                consumePower(energyConsumed*result);
            }

        }

    }
}