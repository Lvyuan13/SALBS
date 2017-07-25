using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {

    Dictionary<string, string> defaultConfig = new Dictionary<string, string>()
        {
            {"number_of_Humans", 4},
            {"starting_O", 1000},
            {"starting_H2O", 1000},
            {"starting_H", 1000},
            {"starting_Thermal", 1000},
            {"starting_CO2", 1000}
        };
	
    public void initiate(){

    }

    public void initiate(Dictionary<string, string> customConfig){
        customConfig.ToList().ForEach(x => defaultConfig[x.Key] = x.Value);
        initiate();
    }

    public void registerModule(IModule module){

    }

    public void deregisterModule(IModule module){

    }

    public List<IModule> getModules(){

    }

}