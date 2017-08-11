using System;
using System.Collections;

namespace LunarNumericSimulator {
	public abstract class ResourceManager<T> {
		public abstract void addResource(T resource);
		public abstract void consumeResource(T resource);
		public abstract T getLevel();
        abstract public Resources managedResource { get; }
        public static float getMolarWeight(Resources res)
        {
            double MW = 0;
            switch (res)
            {
                case Resources.CO2:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "CarbonDioxide");
                    return Convert.ToSingle(MW);
                case Resources.O:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Oxygen");
                    return Convert.ToSingle(MW);
                case Resources.N:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Nitrogen");
                    return Convert.ToSingle(MW);
                case Resources.CH4:
                    MW = CoolProp.PropsSI("M", "", 0, "", 0, "Methane");
                    return Convert.ToSingle(MW);
                default:
                    throw new Exception("Cannot get molar weight for unknown resource");
            }
        }
    }
}