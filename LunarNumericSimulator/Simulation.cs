using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using LunarNumericSimulator.Modules;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator {
    public class Simulation {

        Dictionary<string, float> defaultConfig = new Dictionary<string, float>()
            {
                {"number_of_Humans", 4},
                {"starting_H2O", 1000},
                {"starting_Food", 200},
                {"atmospheric_CO2_start", 0.004F},
                {"atmospheric_O_start", 0.216F},
                {"atmospheric_N_start", 0.78F},
                {"starting_Pressure", 101.4F},
                {"starting_Temperature", 25}
            };
        
        private UInt64 clock = 0;
        public CH4_ResourceManager CH4ResourceManager;
        public CO2_ResourceManager CO2ResourceManager;
        public Food_ResourceManager FoodResourceManager;
        public H_ResourceManager HResourceManager;
        public H2O_ResourceManager H2OResourceManager;
        public N_ResourceManager NResourceManager;
        public O_ResourceManager OResourceManager;
        public Pressure_ResourceManager PressureResourceManager;
        private List<Module> loadedModules;
        private Dictionary<string,Type> moduleCatalogue;
        
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
            float startO, startH2O, startH, startN, startEnthalpy, startCO2, startFood, startPressure;
            try
            {
                defaultConfig.TryGetValue("atmospheric_O_start", out startO);
                defaultConfig.TryGetValue("starting_H2O", out startH2O);
                defaultConfig.TryGetValue("starting_H", out startH);
                defaultConfig.TryGetValue("starting_Enthalpy", out startEnthalpy);
                defaultConfig.TryGetValue("atmospheric_CO2_start", out startCO2);
                defaultConfig.TryGetValue("starting_Food", out startFood);
                defaultConfig.TryGetValue("atmospheric_N_start", out startN);
                defaultConfig.TryGetValue("starting_Pressure", out startPressure);
            }
            catch (Exception e)
            {
                throw e;
            }


            CH4ResourceManager = new CH4_ResourceManager(startO);
            CO2ResourceManager = new CO2_ResourceManager(startCO2);
            FoodResourceManager = new Food_ResourceManager(startFood);
            HResourceManager = new H_ResourceManager(startH);
            H2OResourceManager = new H2O_ResourceManager(startH2O);
            NResourceManager = new N_ResourceManager(startH2O);
            OResourceManager = new O_ResourceManager(startO);
            PressureResourceManager = new Pressure_ResourceManager(startPressure);
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
            // TODO: Stop people from adding modules after step function is run. Force all mdules to be loadd before step
            clock++;
            float volume = 0;
            foreach(Module m in loadedModules){
                volume += m.getModuleVolume();
                m.tick(clock);
            }
            // Calculate pressure
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