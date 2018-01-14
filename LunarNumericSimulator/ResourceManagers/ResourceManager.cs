using System;
using System.Collections;

namespace LunarNumericSimulator {
	public abstract class ResourceManager<T> {
		public abstract void addResource(T resource);
		public abstract void consumeResource(T resource);
		public abstract T getLevel();
        abstract public Resources managedResource { get; protected set; }
        public static double getMolarWeight(Resources res)
        {
            double MW = 0;
            switch (res)
            {
                case Resources.CO2:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "CO2");
                    return MW;
                case Resources.O2:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Oxygen");
                    return MW;
                case Resources.N2:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Nitrogen");
                    return MW;
                case Resources.CH4:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Methane");
                    return MW;
                case Resources.Humidity:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Water");
                    return MW;
                default:
                    throw new Exception("Cannot get molar weight for unknown resource");
            }
        }
    }
}