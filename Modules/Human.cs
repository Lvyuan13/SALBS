using System.Collections;
using System;
using System.Collections.Generic;
namespace LunarParametricNumeric.Modules
{
    public class Human: Module
    {

        Random rand = new Random();

        public Human(Simulation sim, int moduleid) : base(sim,moduleid){
            
        }
        
        public override List<Resources> getResources(){
            return new List<Resources>() { 
                Resources.CO2,
                Resources.CH4,
                Resources.H2O,
                Resources.Enthalpy,
                Resources.O,
                Resources.Food
            };
        } 

        public override string getModuleName(){
            return "Human";
        }

        public override string getModuleFriendlyName(){
            return "Human";
        }

        public override void setLoad(float value){

        }

        protected override void update(UInt64 clock){
            double airIntakeL = 0.25*(2*Math.PI/5)*Math.Cos((2*Math.PI/5)*clock); // A sine function which estimates human breathing patterns
            // TODO: Factor in varying breathing rates for intensities
            double airIntakeKG = 0.001225*airIntakeL; // There's 0.001225kg/L of air

            if (airIntakeKG < 0){
                double nitrogenIntake = 0.78*airIntakeKG;
                double oxygenIntake = 0.21*airIntakeKG;
                double co2Intake = 0.0004*airIntakeKG;
                consumeResource(Resources.N, Convert.ToSingle(nitrogenIntake));
                consumeResource(Resources.O, Convert.ToSingle(oxygenIntake));
                consumeResource(Resources.CO2, Convert.ToSingle(co2Intake));
            } else if (airIntakeKG > 0){
                double nitrogenProduced = 0.78*airIntakeKG;
                double oxygenProduced = 0.17*airIntakeKG;
                double co2Produced = 0.05*airIntakeKG;
                produceResource(Resources.N, Convert.ToSingle(nitrogenProduced));
                produceResource(Resources.O, Convert.ToSingle(oxygenProduced));
                produceResource(Resources.CO2, Convert.ToSingle(co2Produced));
            }

            double heatRelease = 118 * 10^-3; // Humans release heat at 118W, converting to kJ
            produceResource(Resources.Enthalpy, Convert.ToSingle(heatRelease)); 
            
            if (rand.Next(10800) == 1) // There is an 8 in 86400 chance that flatulence will occur, since average person has flatulence 8 times per day
                flatulence();

            if (rand.Next(21600) == 1) // There is an 4 in 86400 chance that eating will occur, since a person has 4 meals in a day
                eat();

        }

        protected void flatulence(){
            produceResource(Resources.N, N_ResourceManager.LitresToKG(0.0531F)); // See Harry's notebook for details of these numbers
            produceResource(Resources.H, H_ResourceManager.LitresToKG(0.0189F));
            produceResource(Resources.CO2, CO2_ResourceManager.LitresToKG(0.0081F));
            produceResource(Resources.CH4, CH4_ResourceManager.LitresToKG(0.0063F));
            produceResource(Resources.O, O_ResourceManager.LitresToKG(0.0036F));
        }

        protected void eat(){
            consumeResource(Resources.Food, 0.5F);
        }
        
    }
}