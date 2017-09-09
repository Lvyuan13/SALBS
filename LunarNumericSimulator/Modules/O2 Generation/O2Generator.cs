﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarNumericSimulator.Utilities;

namespace LunarNumericSimulator.Modules.O2_Generation
{
    class O2Generator : Module
    {
        protected PIDController pid;

        [NumericConfigurationParameter("P Gain", "double", false)]
        public double PGain { private get; set; }
        [NumericConfigurationParameter("I Gain", "double", false)]
        public double IGain { private get; set; }
        [NumericConfigurationParameter("D Gain", "double", false)]
        public double DGain { private get; set; }
        [NumericConfigurationParameter("Nominal O2 Volume %", "double", false)]
        public double DesiredO2Level { private get; set; }

        public O2Generator(Simulation sim, int moduleid) : base(sim,moduleid)
        {


        }

        public override List<Resources> getRegisteredResources()
        {
            return new List<Resources>() {
                Resources.H2O,
                Resources.ElecticalEnergy,
                Resources.H,
                Resources.O,
            };
        }

        public override void ModuleReady()
        {
            pid = new PIDController(PGain, IGain, DGain, 1);
        }

        public override double getModuleVolume()
        {
            return 0;
        }

        public override string moduleName
        {
            get { return "O2Generator"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Oxygen Generator"; }
        }

        public override List<string> requiresTanks()
        {
            return new List<string> {   };
        }


        protected override void update(UInt64 clock)
        {

            // update the PID
            double O2Level = getAtmosphericMolarFraction(Resources.O);
            double O2Mass = getResourceLevel(Resources.O);

            var result = pid.update(DesiredO2Level - O2Level, 1);

            if (result < 0)
            {
                pid.removeWindup();
                return;
            }
                

            // Example calculation
            // say we need Xkg of O2, we then need
            // X * ((2*18)/32) kg of H2O
            // takada et.al 2015 say that the ISS
            // can produce 9.2kg of O2 per day using 4kW of power 

            // design values summary:

            // to convert 9.2*((2*18)/32) = 10.35kg of H2O into 9.2kg of O2
            // the machine will run at 9.2kW
            // it will generate an unknown amount of heat.

            double massflowRate = 0.00011979167; // kg
            double powerPerMassFlowRate = 9.2; // kJ

            double powerRequired = powerPerMassFlowRate / massflowRate; // kJ / kg

            //double inflowH2O = 10.35; // kg/day = mass flow rate = 1 percentage by mass
            //double outflowO2 = 9.2; // kg/day => 0.8888 percentage by mass
            double inflowAdjusted = result;

            double O2Produced = 0.8889 * result;
            double H2Produced = 0.1111 * result;

            double powerUsed = powerRequired * result;


            Console.WriteLine("power used = " + powerUsed);
            consumeResource(Resources.H2O, inflowAdjusted);
            consumePower(powerUsed);
            produceResource(Resources.O, O2Produced);
            produceResource(Resources.H, H2Produced);
            // TODO find theoretical values for heat produced if any

        }


        // TODO the double floating point values are tricky to compare, this needs making more robust
        /*protected double updatePID()
        {
 // check for acceptable O2 level

            // set member variable as to whether the resources should be updated on this particular update call
            // true by default
            changeResources = false;

            if (result >= 0.000000000000 || O2Level < 0.2160000000000)
                changeResources = true;

            //if (O2Mass - result < 0)
            //{
            //    pid.removeWindup();
            //    changeResources = false;
            //}

            //Console.WriteLine("O2 level = " + O2Level + "   O2Mass = " + O2Mass + "   result = " + result + " changeResources = " + changeResources);

            // return the result of the pid controller
            return result;
        }*/
    }
}

