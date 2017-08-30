using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using LunarNumericSimulator.Modules;
using LunarNumericSimulator.ResourceManagers;
using LunarNumericSimulator.Reporting;
using System.Threading.Tasks;

namespace LunarNumericSimulator {
    public class Simulation {

        Dictionary<string, double> defaultConfig = new Dictionary<string, double>()
            {
                {"starting_H2O", 1000},
                {"starting_Food", 200},
                {"starting_H", 1000 },
                {"atmospheric_CO2_start", 0.003F},
                {"atmospheric_O_start", 0.216F},
                {"atmospheric_N_start", 0.78F},
                {"atmospheric_CH4_start", 0.001F},
                {"starting_Pressure", 101.4F},
                {"starting_Temp", 25}
            };
        
        private UInt64 clock = 0;
        List<AtmosphericResourceManager> atmosphericResourceManagers = new List<AtmosphericResourceManager>();
        List<StoredResourceManager> storedResourceManagers = new List<StoredResourceManager>();
        public ThermodynamicEngine ThermoEngine;
        protected List<Module> loadedModules;
        protected Dictionary<string,Type> moduleCatalogue;
        static bool locked = false;
        
        public void initiate(){

            setupResourceManagers();

            loadedModules = new List<Module>();
            loadModuleCatalogue();
        }

        public void initiate(Dictionary<string, double> customConfig)
        {
            customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
            initiate();
        }

        protected void setupResourceManagers()
        {
            double startH2O, startH,startFood;
            try
            {
                defaultConfig.TryGetValue("starting_H2O", out startH2O);
                defaultConfig.TryGetValue("starting_H", out startH);
                defaultConfig.TryGetValue("starting_Food", out startFood);
            }
            catch (Exception e)
            {
                throw e;
            }


            atmosphericResourceManagers.Add(new AtmosphericResourceManager(Resources.CH4, "Methane"));
            atmosphericResourceManagers.Add(new AtmosphericResourceManager(Resources.CO2, "CarbonDioxide"));
            atmosphericResourceManagers.Add(new AtmosphericResourceManager(Resources.N, "Nitrogen"));
            atmosphericResourceManagers.Add(new AtmosphericResourceManager(Resources.O, "Oxygen"));

            storedResourceManagers.Add(new StoredResourceManager(Resources.Food, startFood));
            storedResourceManagers.Add(new StoredResourceManager(Resources.H, startH));
            storedResourceManagers.Add(new StoredResourceManager(Resources.H2O, startH2O));
            storedResourceManagers.Add(new StoredResourceManager(Resources.ElecticalEnergy, 0));
        }

        protected void assignThermoControllerToResourceManagers()
        {
            foreach (var rm in atmosphericResourceManagers)
                rm.setThermoManager(ref ThermoEngine);
        }

        private void loadModuleCatalogue(){
            moduleCatalogue = new Dictionary<string, Type>();
            var exporters = (from element in Assembly.GetExecutingAssembly().GetTypes()
                                            where element.IsSubclassOf(typeof(Module)) && !element.IsAbstract
                                            select element).ToList();
            
            foreach(var t in exporters){
                var m = (Module)Activator.CreateInstance(t, this, 0);
                moduleCatalogue.Add(m.moduleName, t);
            }
        }

        public void step(){
            if (clock == 0)
            {
                var CH4 = getAtmosphericResourceManagerIndex(Resources.CH4);
                var CO2 = getAtmosphericResourceManagerIndex(Resources.CO2);
                var O = getAtmosphericResourceManagerIndex(Resources.O);
                var N = getAtmosphericResourceManagerIndex(Resources.N);
                ThermoEngine = new ThermodynamicEngine(ref CH4, ref CO2, ref O, ref N, defaultConfig, getSystemVolume());
                assignThermoControllerToResourceManagers();
            }
            clock++;
            ulong thing;
            if (clock == 86400)
                thing = clock;
            foreach (Module m in loadedModules)
            //Parallel.ForEach(loadedModules, (m) =>
            {
                m.tick(clock);
            }

        }

        protected AtmosphericResourceManager getAtmosphericResourceManagerIndex(Resources res)
        {
            for (int i = 0; i < atmosphericResourceManagers.Count; i++)
                if (atmosphericResourceManagers[i].managedResource == res)
                    return atmosphericResourceManagers[i];
            throw new Exception("Resource does not exist!");
        }

        public ResourceManager<double>[] getAllResourceManagers()
        {
            var resourceManagers = new List<ResourceManager<double>>();
            resourceManagers.AddRange(atmosphericResourceManagers);
            resourceManagers.AddRange(storedResourceManagers);
            resourceManagers.Add(ThermoEngine);
            return resourceManagers.ToArray();
        }

        public EnvironmentState getResourceStatus()
        {
            var state = new EnvironmentState();
            state.clock = clock;
            state.Atmospheric = new EnvironmentState.Atmosphere();
            state.Atmospheric.TotalPressure = ThermoEngine.getSystemPressure();
            state.Atmospheric.Temperature = ThermoEngine.getSystemTemperature();
            state.Atmospheric.TotalMass = ThermoEngine.getSystemMass();
            state.Atmospheric.TotalEnthalpy = ThermoEngine.getSystemEnthalpy();

            var atmos_rms = from element in getAllResourceManagers()
                            where element.managedResource.IsAtmospheric()
                            select element;
            foreach (var rm in atmos_rms)
                state.Atmospheric.Add(new EnvironmentState.Gas((AtmosphericResourceManager)rm));

            state.Stored = new EnvironmentState.Storage();
            var stored_rms = from element in getAllResourceManagers()
                            where (!element.managedResource.IsAtmospheric())
                            select element;
            foreach (var rm in stored_rms)
                state.Stored.Add(new EnvironmentState.StoredResource(rm));

            return state;
        }

        public ModuleResourceLevels getModuleResourceStatus(int moduleid)
        {
            var module = (from element in loadedModules
                         where element.ModuleID == moduleid
                         select element).FirstOrDefault();
            return new ModuleResourceLevels(module.ModuleID, module.moduleName, module.getResourceConsumption(), module.getRegisteredResources());
        }

        public SimulationProgressReport getSimulationState()
        {
            SimulationProgressReport report = new SimulationProgressReport();
            report.GlobalState = getResourceStatus();
            var modules = (from element in loadedModules
                            select getModuleResourceStatus(element.ModuleID)).ToList();
            report.ModuleStates = modules;
            return report;

        }

        protected double getSystemVolume()
        {
            return (from element in loadedModules
                    select element.getModuleVolume()).Sum();
        }

        public bool registerModule(string moduleName){
            if (clock > 0)
                throw new Exception("All Modules must be registered before the simulation starts!");
            Type moduleType = null;

            try {
                moduleCatalogue.TryGetValue(moduleName, out moduleType);
            } catch (Exception e){
                return false;
            }

            if (moduleType == null)
                return false;

            Module newModule = (Module)Activator.CreateInstance(moduleType, this, loadedModules.Count+1);
            loadedModules.Add(newModule);
            return true;
        }

        public bool deregisterModule(int moduleid){
            if (clock > 0)
                throw new Exception("Cannot deregister a Module during simulation!");
            foreach (Module m in loadedModules)
                if (m.ModuleID == moduleid){
                    loadedModules.Remove(m);
                    return true;
                }
            
            return false;
        }

        public List<Module> getModules(){
            return loadedModules;
        }

    }
}