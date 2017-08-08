using System;
using System.Collections;
using System.Collections.Generic;

namespace LunarNumericSimulator {
    public abstract class Module {
        // A link to the simulation environment, should not be directly accessed outside of this class (even by subclasses)
        private Simulation Environment;

        // An ID that is allocated to the module when the user loads it into the workspace
        protected int ModuleID;

        // Keeps track of resources used by the module since last update
        private float[] resourceReceipts;
        public Module(Simulation sim, int id){
            Environment = sim;
            ModuleID = id;

            int resourceCount = Enum.GetNames(typeof(Resources)).Length;
            resourceReceipts = new float[resourceCount];
        }

        // Returns the runtime ID of the module
        public int getID(){
            return ModuleID;
        }

        // Sets the module load. Anywhere from 0%-100%, how hard is the module working?
        abstract public void setLoad(float load);

        // Called every tick, describes the behaviour of the module.
        abstract protected void update(UInt64 clock);

        // Resources used by the module must be provided by the function. Hardcoding a list is advised. Prevents bug from silly cross-resource mistakes
        abstract public List<Resources> getRegisteredResources();

        // A hardcoded name for the module. How will we recognise a Hab module from a science module?
        abstract public string getModuleName();

        // A hardcoded friendly name for the module. This is for user presentation only
        abstract public string getModuleFriendlyName();

        // Return the volume of the module in m3
        abstract public float getModuleVolume();

        // Internal function which is called by the simulator, this function will trigger an update
        public void tick(UInt64 clock){
            for (int i = 0; i < resourceReceipts.Length; i++){
                resourceReceipts[i] = 0;
            }
            update(clock);
        }

        // Callable by the simulator to determine the resources used by the module in the last update
        public float[] getResourceConsumption(){
            return resourceReceipts;
        }

        // Not abstract, as the subclasses should access resources through this function
        protected void consumeResource(Resources res, float quantity){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            switch (res){
                case Resources.CO2:
                    Environment.CO2ResourceManager.consumeResource(quantity);
                    break;
                case Resources.CH4:
                    Environment.CH4ResourceManager.consumeResource(quantity);
                    break;
                case Resources.Enthalpy:
                    Environment.ThermalResourceManager.consumeResource(quantity);
                    break;
                case Resources.Food:
                    Environment.FoodResourceManager.consumeResource(quantity);
                    break;
                case Resources.H:
                    Environment.HResourceManager.consumeResource(quantity);
                    break;
                case Resources.H2O:
                    Environment.H2OResourceManager.consumeResource(quantity);
                    break;
                case Resources.O:
                    Environment.OResourceManager.consumeResource(quantity);
                    break;
                case Resources.N:
                    Environment.NResourceManager.consumeResource(quantity);
                    break;
            }
            resourceReceipts[(int) res] -= quantity;
        }

        // Not abstract, as the subclasses should access resources through this function
        protected void produceResource(Resources res, float quantity){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            switch (res){
                case Resources.CO2:
                    Environment.CO2ResourceManager.addResource(quantity);
                    break;
                case Resources.CH4:
                    Environment.CH4ResourceManager.addResource(quantity);
                    break;
                case Resources.Enthalpy:
                    Environment.ThermalResourceManager.addResource(quantity);
                    break;
                case Resources.Food:
                    Environment.FoodResourceManager.addResource(quantity);
                    break;
                case Resources.H:
                    Environment.HResourceManager.addResource(quantity);
                    break;
                case Resources.H2O:
                    Environment.H2OResourceManager.addResource(quantity);
                    break;
                case Resources.O:
                    Environment.OResourceManager.addResource(quantity);
                    break;
                case Resources.N:
                    Environment.NResourceManager.addResource(quantity);
                    break;
            }
            resourceReceipts[(int) res] += quantity;
        }

        protected float getResourceLevel(Resources res){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            switch (res){
                case Resources.CO2:
                    return Environment.CO2ResourceManager.getLevel();
                case Resources.CH4:
                    return Environment.CH4ResourceManager.getLevel();
                case Resources.Enthalpy:
                    return Environment.ThermalResourceManager.getLevel();
                case Resources.Food:
                    return Environment.FoodResourceManager.getLevel();
                case Resources.H:
                    return Environment.HResourceManager.getLevel();
                case Resources.H2O:
                    return Environment.H2OResourceManager.getLevel();
                case Resources.O:
                    return Environment.OResourceManager.getLevel();
                case Resources.N:
                    return Environment.NResourceManager.getLevel();
            }
            return 0F;
        }
    }
}