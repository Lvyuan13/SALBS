using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using LunarParametricNumeric.Modules;

namespace LunarParametricNumeric {
    public class Simulation {

        Dictionary<string, int> defaultConfig = new Dictionary<string, int>()
            {
                {"number_of_Humans", 4},
                {"starting_O", 1000},
                {"starting_H2O", 1000},
                {"starting_H", 1000},
                {"starting_Enthalpy", 1000},
                {"starting_Food", 200},
                {"starting_CO2", 1000},
                {"starting_N", 1000}
            };
        
        private UInt64 clock = 0;
        public CH4_ResourceManager CH4ResourceManager;
        public CO2_ResourceManager CO2ResourceManager;
        public Food_ResourceManager FoodResourceManager;
        public H_ResourceManager HResourceManager;
        public H2O_ResourceManager H2OResourceManager;
        public N_ResourceManager NResourceManager;
        public O_ResourceManager OResourceManager;
        public Thermal_ResourceManager ThermalResourceManager;
        private List<Module> loadedModules;
        private Dictionary<string,Type> moduleCatalogue;
        
        public void initiate(){
            int startO,startH2O, startH, startN, startEnthalpy, startCO2, startFood;
            try {
                defaultConfig.TryGetValue("starting_O", out startO);
                defaultConfig.TryGetValue("starting_H2O", out startH2O);
                defaultConfig.TryGetValue("starting_H", out startH);
                defaultConfig.TryGetValue("starting_Enthalpy", out startEnthalpy);
                defaultConfig.TryGetValue("starting_CO2", out startCO2);
                defaultConfig.TryGetValue("starting_Food", out startFood);
                defaultConfig.TryGetValue("starting_N", out startN);
            } catch(Exception e){
                throw e;
            }


            CH4ResourceManager = new CH4_ResourceManager(startO);
            CO2ResourceManager = new CO2_ResourceManager(startCO2);
            FoodResourceManager = new Food_ResourceManager(startFood);
            HResourceManager = new H_ResourceManager(startH);
            H2OResourceManager = new H2O_ResourceManager(startH2O);
            NResourceManager = new N_ResourceManager(startH2O);
            OResourceManager = new O_ResourceManager(startO);
            ThermalResourceManager = new Thermal_ResourceManager(startEnthalpy);

            loadedModules = new List<Module>();
            loadModuleCatalogue();
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

        public void initiate(Dictionary<string, int> customConfig){
            customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
            initiate();
        }

        public void step(){
            clock++;
            foreach(Module m in loadedModules){
                m.tick(clock);
            }
        }

        private void registerModule(string moduleName){
            Type moduleType = null;

            try {
                moduleCatalogue.TryGetValue(moduleName, out moduleType);
            } catch (Exception e){
                throw e;
            }

            Module newModule = (Module)Activator.CreateInstance(moduleType, this, loadedModules.Count+1);
            loadedModules.Add(newModule);
        }

        private void deregisterModule(int moduleid){
            foreach(Module m in loadedModules)
                if (m.getID() == moduleid)
                    loadedModules.Remove(m);
        }

        public List<Module> getModules(){
            return loadedModules;
        }

    }
}