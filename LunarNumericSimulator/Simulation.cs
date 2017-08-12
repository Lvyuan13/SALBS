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
                {"atmospheric_CO2_start", 0.004F},
                {"atmospheric_O_start", 0.216F},
                {"atmospheric_N_start", 0.78F},
                {"atmospheric_CH4_start", 0F},
                {"starting_Pressure", 101.4F},
                {"starting_Temperature", 25}
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
        protected ThermodynamicEngine ThermoEngine;
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

        private void loadModuleCatalogue(){
            moduleCatalogue = new Dictionary<string, Type>();
            IEnumerable<Module> exporters = typeof(Module).GetTypeInfo().Assembly.DefinedTypes
                .Where(t => t.IsSubclassOf(typeof(Module)) && !t.IsAbstract)
                .Select(t => (Module)Activator.CreateInstance(t.GetType(), this, 0));
            foreach(Module m in exporters){
                moduleCatalogue.Add(m.getModuleName(), m.GetType());
            }
        }

        public void step(){
            if (clock == 0)
            {
                ThermoEngine = new ThermodynamicEngine(CH4ResourceManager, CO2ResourceManager, OResourceManager, NResourceManager, defaultConfig, getSystemVolume());
            }
            clock++;
            foreach(Module m in loadedModules){
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
            state.Atmospheric.TotalPressure = ThermoEngine.getSystemPressure();
            state.Atmospheric.Temperature = ThermoEngine.getSystemTemperature();
            state.Atmospheric.TotalMass = ThermoEngine.getSystemMass();
            state.Atmospheric.TotalEnthalpy = ThermoEngine.getSystemEnthalpy();

            var atmos_rms = from element in getAllResourceManagers()
                            where element.GetType() == typeof(AtmosphericResourceManager)
                            select element;
            foreach (var rm in atmos_rms)
                state.Atmospheric.Add(new EnvironmentState.Gas((AtmosphericResourceManager)rm));

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
                         where element.getID() == moduleid
                         select element).FirstOrDefault();
            return new ModuleResourceLevels(module.getResourceConsumption());
        }

        protected float getSystemVolume()
        {
            return (from element in loadedModules
                    select element.getModuleVolume()).Sum();
        }

        private bool registerModule(string moduleName){
            if (clock > 0)
                throw new Exception("All Modules must be registered before the simulation starts!");
            Type moduleType = null;

            try {
                moduleCatalogue.TryGetValue(moduleName, out moduleType);
            } catch (Exception e){
                return false;
            }

            Module newModule = (Module)Activator.CreateInstance(moduleType, this, loadedModules.Count+1);
            loadedModules.Add(newModule);
            return true;
        }

        private bool deregisterModule(int moduleid){
            if (clock > 0)
                throw new Exception("Cannot deregister a Module during simulation!");
            foreach (Module m in loadedModules)
                if (m.getID() == moduleid){
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