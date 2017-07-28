using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

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
                {"starting_CO2", 1000}
            };
        
        
        public CH4_ResourceManager CH4ResourceManager;
        public CO2_ResourceManager CO2ResourceManager;
        public Food_ResourceManager FoodResourceManager;
        public H_ResourceManager HResourceManager;
        public H2O_ResourceManager H2OResourceManager;
        public O_ResourceManager OResourceManager;
        public Thermal_ResourceManager ThermalResourceManager;
        private List<Module> loadedModules;
        private Dictionary<string,Type> moduleCatalogue;
        
        public void initiate(){
            int startO,startH2O, startH, startEnthalpy, startCO2, startFood;
            try {
                defaultConfig.TryGetValue("starting_O", out startO);
                defaultConfig.TryGetValue("starting_H2O", out startH2O);
                defaultConfig.TryGetValue("starting_H", out startH);
                defaultConfig.TryGetValue("starting_Enthalpy", out startEnthalpy);
                defaultConfig.TryGetValue("starting_CO2", out startCO2);
                defaultConfig.TryGetValue("starting_Food", out startFood);
            } catch(Exception e){
                throw e;
            }


            CH4ResourceManager = new CH4_ResourceManager(startO);
            CO2ResourceManager = new CO2_ResourceManager(startCO2);
            FoodResourceManager = new Food_ResourceManager(startFood);
            HResourceManager = new H_ResourceManager(startH);
            H2OResourceManager = new H2O_ResourceManager(startH2O);
            OResourceManager = new O_ResourceManager(startO);
            ThermalResourceManager = new Thermal_ResourceManager(startEnthalpy);

            loadedModules = new List<Module>();
            loadModuleCatalogue();
        }

        private void loadModuleCatalogue(){
            moduleCatalogue = new Dictionary<string, Type>();
            IEnumerable<Module> exporters = typeof(Module).GetTypeInfo().Assembly.DefinedTypes
                .Where(t => t.IsSubclassOf(typeof(Module)) && !t.IsAbstract)
                .Select(t => (Module)Activator.CreateInstance(t.GetType()));
            foreach(Module m in exporters){
                moduleCatalogue.Add(m.getModuleName(), m.GetType());
            }
        }

        public void initiate(Dictionary<string, int> customConfig){
            customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
            initiate();
        }

        private void registerModule(string moduleName){
            Type moduleType;
            moduleCatalogue.TryGetValue(moduleName, out moduleType);
            Module newModule = (Module)Activator.CreateInstance(moduleType);
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