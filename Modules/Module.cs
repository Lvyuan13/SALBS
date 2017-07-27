using System;

namespace LunarParametricNumeric {
    public abstract class Module {
        
        protected Simulation Environment;
        public Module(ref Simulation sim){
            Environment = sim;
        }
        abstract public void setLoad(float load);
        abstract public void update();

    }
}