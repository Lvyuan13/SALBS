using System.Collections;
using System.Collections.Generic;
namespace LunarParametricNumeric.Modules
{
    public class Human: Module
    {

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

        protected override void update(){

        }
        
    }
}