using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using LunarNumericSimulator.Modules;
using LunarNumericSimulator.ResourceManagers;
using LunarNumericSimulator.Reporting;

namespace LunarNumericSimulator {
    public class Simulation {

        Dictionary<string, float> defaultConfig = new Dictionary<string, float>()
            {
                {"starting_H2O", 1000},
                {"starting_Food", 200},
                {"starting_H", 1000 },
                {"atmospheric_CO2_start", 0.004F},
                {"atmospheric_O_start", 0.216F},
                {"atmospheric_N_start", 0.78F},
                {"atmospheric_CH4_start", 0F},
                {"starting_Pressure", 101.4F},
                {"starting_Temp", 25}
            };
        
        private UInt64 clock = 0;
        protected CH4_ResourceManager CH4ResourceManager;
        protected CO2_ResourceManager CO2ResourceManager;
        protected Food_ResourceManager FoodResourceManager;
        protected H_ResourceManager HResourceManager;
        protected H2O_ResourceManager H2OResourceManager;
        protected N_ResourceManager NResourceManager;
        protected O_ResourceManager OResourceManager;
        protected ElectricalEnergy_ResourceManager EEResourceManager;
        public ThermodynamicEngine ThermoEngine;
        protected List<Module> loadedModules;
        protected Dictionary<string,Type> moduleCatalogue;
        
        public void initiate(){

            setupResourceManagers();

            loadedModules = new List<Module>();
            loadModuleCatalogue();
        }

        public void initiate(Dictionary<string, float> customConfig)
        {
            customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
            initiate();
        }

        protected void setupResourceManagers()
        {
            float startH2O, startH,startFood;
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


            CH4ResourceManager = new CH4_ResourceManager();
            CO2ResourceManager = new CO2_ResourceManager();
            FoodResourceManager = new Food_ResourceManager(startFood);
            HResourceManager = new H_ResourceManager(startH);
            H2OResourceManager = new H2O_ResourceManager(startH2O);
            NResourceManager = new N_ResourceManager();
            OResourceManager = new O_ResourceManager();
            EEResourceManager = new ElectricalEnergy_ResourceManager(0);
        }

        protected void assignThermoControllerToResourceManagers()
        {
            CH4ResourceManager.setThermoManager(ref ThermoEngine);
            CO2ResourceManager.setThermoManager(ref ThermoEngine);
            NResourceManager.setThermoManager(ref ThermoEngine);
            OResourceManager.setThermoManager(ref ThermoEngine);
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
                ThermoEngine = new ThermodynamicEngine(ref CH4ResourceManager, ref CO2ResourceManager, ref OResourceManager, ref NResourceManager, defaultConfig, getSystemVolume());
                assignThermoControllerToResourceManagers();
            }
            clock++;
            foreach (Module m in loadedModules)
            {
                m.tick(clock);
            }
        }

        public ResourceManager<float>[] getAllResourceManagers()
        {
            return new ResourceManager<float>[]
            {
                CH4ResourceManager,
                CO2ResourceManager,
                FoodResourceManager,
                HResourceManager,
                H2OResourceManager,
                NResourceManager,
                OResourceManager,
                ThermoEngine
            };
        }

        public EnvironmentState getResourceStatus()
        {
            var state = new EnvironmentState();
            state.Atmospheric = new EnvironmentState.Atmosphere();
            state.Atmospheric.TotalPressure = ThermoEngine.getSystemPressure();
            state.Atmospheric.Temperature = ThermoEngine.getSystemTemperature();
            state.Atmospheric.TotalMass = ThermoEngine.getSystemMass();
            state.Atmospheric.TotalEnthalpy = ThermoEngine.getSystemEnthalpy();

            var atmos_rms = from element in getAllResourceManagers()
                            where element.GetType() == typeof(AtmosphericResourceManager)
                            select element;
            foreach (var rm in atmos_rms)
                state.Atmospheric.Add(new EnvironmentState.Gas((AtmosphericResourceManager)rm));

            state.Stored = new EnvironmentState.Storage();
            var stored_rms = from element in getAllResourceManagers()
                            where (element.GetType() != typeof(AtmosphericResourceManager) && element.GetType() != typeof(ThermodynamicEngine))
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
            return new ModuleResourceLevels(module.getResourceConsumption());
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

        protected float getSystemVolume()
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