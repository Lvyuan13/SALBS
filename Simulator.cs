using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LunarParametricNumeric {
    public class Simulation {

        Dictionary<string, int> defaultConfig = new Dictionary<string, int>()
            {
                {"number_of_Humans", 4},
                {"starting_O", 1000},
                {"starting_H2O", 1000},
                {"starting_H", 1000},
                {"starting_Thermal", 1000},
                {"starting_Food", 200},
                {"starting_CO2", 1000}
            };
        
        CH4_ResourceManager CH4ResourceManager;
        CO2_ResourceManager CO2ResourceManager;
        Food_ResourceManager FoodResourceManager;
        H_ResourceManager HResourceManager;
        H2O_ResourceManager H2OResourceManager;
        O_ResourceManager OResourceManager;
        Thermal_ResourceManager ThermalResourceManager;
        
        public void initiate(){
            int startO,startH2O, startH, startEnthalpy, startCO2, startFood;
            try {
                defaultConfig.TryGetValue("starting_O", out startO);
                defaultConfig.TryGetValue("starting_H2O", out startH2O);
                defaultConfig.TryGetValue("starting_H", out startH);
                defaultConfig.TryGetValue("starting_Thermal", out startEnthalpy);
                defaultConfig.TryGetValue("starting_CO2", out startCO2);
                defaultConfig.TryGetValue("starting_CO2", out startFood);
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


        }

        public void initiate(Dictionary<string, int> customConfig){
            customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
            initiate();
        }

        public void registerModule(IModule module){

        }

        public void deregisterModule(IModule module){

        }

        public List<Module> getModules(){

        }

    }
}