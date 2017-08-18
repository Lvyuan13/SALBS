using System.Collections;
using System;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator.Modules
{
    public class Human: Module
    {

        Random rand = new Random();
        public Human(Simulation sim, int moduleid) : base(sim,moduleid){
            
        }
        
        public override List<Resources> getRegisteredResources(){
            return new List<Resources>() { 
                Resources.CO2,
                Resources.CH4,
                Resources.H2O,
                Resources.Heat,
                Resources.O,
                Resources.Food,
                Resources.N
            };
        } 

        public override double getModuleVolume(){
            return 0;
        }

        public override string moduleName {
            get { return "Human"; }
        }

        public override string moduleFriendlyName
        {
            get { return "Human"; }
        }

        protected override void update(UInt64 clock){
            double airIntakeL = 0.25*(2*Math.PI/5)*Math.Cos((2*Math.PI/5)*clock); // A sine function which estimates human breathing patterns
            // TODO: Factor in varying breathing rates for intensities
            double airIntakeKG = Math.Abs(getAirDensity() * 0.001 * airIntakeL); // There's 0.001225kg/L of air

            if (airIntakeL < 0){
                double nitrogenIntake = 0.78*airIntakeKG;
                double oxygenIntake = 0.21*airIntakeKG;
                double co2Intake = 0.0004*airIntakeKG;
                consumeResource(Resources.N, nitrogenIntake);
                consumeResource(Resources.O, oxygenIntake);
                consumeResource(Resources.CO2, co2Intake);
            } else if (airIntakeL > 0){
                double nitrogenProduced = 0.78*airIntakeKG;
                double oxygenProduced = 0.15*airIntakeKG;
                double co2Produced = 0.07*airIntakeKG;
                produceResource(Resources.N, nitrogenProduced);
                produceResource(Resources.O, oxygenProduced);
                produceResource(Resources.CO2, co2Produced);
            }

            double heatRelease = 118 * Math.Pow(10,-3); // Humans release heat at 118W, converting to kJ
            produceResource(Resources.Heat, heatRelease); 
            
            if (rand.Next(10800) == 1) // There is an 8 in 86400 chance that flatulence will occur, since average person has flatulence 8 times per day
                flatulence();

            if (rand.Next(21600) == 1) // There is an 4 in 86400 chance that eating will occur, since a person has 4 meals in a day
                eat();

        }

        protected void flatulence(){
            produceResourceLitres(Resources.N, 0.0531F); // See Harry's notebook for details of these numbers
            produceResourceLitres(Resources.CO2, 0.0081F);
            produceResourceLitres(Resources.CH4, 0.0063F);
            produceResourceLitres(Resources.O, 0.0036F);
        }

        protected void eat(){
            consumeResource(Resources.Food, 2);
        }
        
    }
}