using System;
using System.Collections;
using System.Collections.Generic;

namespace LunarNumericSimulator {
    public abstract class Module {
        // A link to the simulation environment, should not be directly accessed outside of this class (even by subclasses)
        private Simulation Environment;

        // An ID that is allocated to the module when the user loads it into the workspace
        public int ModuleID
        {
            get;
            protected set;
        }

        // Keeps track of resources used by the module since last update
        private double[] resourceReceipts;
        public Module(Simulation sim, int id){
            Environment = sim;
            ModuleID = id;

            int resourceCount = Enum.GetNames(typeof(Resources)).Length;
            resourceReceipts = new double[resourceCount];
        }

        // Called every tick, describes the behaviour of the module.
        abstract protected void update(UInt64 clock);

        // Resources used by the module must be provided by the function. Hardcoding a list is advised. Prevents bug from silly cross-resource mistakes
        abstract public List<Resources> getRegisteredResources();

        // A hardcoded name for the module. How will we recognise a Hab module from a science module?
        abstract public string moduleName { get; }

        // A hardcoded friendly name for the module. This is for user presentation only
        abstract public string moduleFriendlyName { get; }

        // Return the volume of the module in m3
        abstract public double getModuleVolume();

        // Internal function which is called by the simulator, this function will trigger an update
        public void tick(UInt64 clock){
            for (int i = 0; i < resourceReceipts.Length; i++){
                resourceReceipts[i] = 0;
            }
            update(clock);
        }

        // Callable by the simulator to determine the resources used by the module in the last update
        public double[] getResourceConsumption(){
            return resourceReceipts;
        }

        // Not abstract, as the subclasses should access resources through this function
        protected void consumeResource(Resources res, double quantity){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == res)
                    rm.consumeResource(quantity);
            resourceReceipts[(int) res] -= quantity;
        }

        // Not abstract, as the subclasses should access resources through this function
        protected void produceResource(Resources res, double quantity){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == res)
                    rm.addResource(quantity);
            resourceReceipts[(int) res] += quantity;
        }

        protected double getResourceLevel(Resources res){
            if (!getRegisteredResources().Contains(res)){
                throw new Exception("The module " + ModuleID + " has not declared access to this resource! ");
            }
            foreach (ResourceManager<double> rm in Environment.getAllResourceManagers())
                if (rm.managedResource == res)
                    return rm.getLevel();
            return 0F;
        }
    }
}