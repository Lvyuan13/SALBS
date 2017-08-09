using CsvHelper;
using LunarNumericSimulator.Utilities;
using System;
using System.IO;
using System.Linq;

namespace LunarNumericSimulator.ResourceManagers
{
    public abstract class AtmosphericResourceManager : ResourceManager<float>
    {
        protected float enthalpyPerUnitMass;
        protected float specificVolume;
        protected float pressure;
        protected float temperature;
        protected float totalResource;
        abstract public string fluidName { get; }
        abstract override public Resources managedResource { get; }
        public float molarWeight { get; protected set; }

        public AtmosphericResourceManager(){
            double MW = CoolProp.PropsSI("M", "", 0, "", 0, fluidName);
            molarWeight = Convert.ToSingle(MW);

        }

        public abstract override void addResource(float resource);

        public abstract override void consumeResource(float resource);

        public abstract override float getLevel();

        public void setState(float temp, float press, float enth, float specvol){
			temperature = temp;
			pressure = press;
			enthalpyPerUnitMass = enth;
            specificVolume = specvol;
		}

		public float getEnthalpyPerUnitMass(){
			return enthalpyPerUnitMass;
		}

        public float getSpecificVolume()
        {
            return specificVolume;
        }

        public float getTemperature(){
			return temperature;
		}

		public float getPressure(){
			return pressure;
		}

		public float getTotalEnthalpy(){
			return totalResource * enthalpyPerUnitMass;
		}

        public abstract float LitresToKG(float litres);

		public EnthalpyAndSpecVolume getEnthalpyAndSpecVolumeAtPoint(float temp, float pressure){
            var enth = CoolProp.PropsSI("H", "T", temp+273.15F, "P", pressure*0.001, fluidName);
            var specvol = 1 / CoolProp.PropsSI("D", "T",temp + 273.15F, "P", pressure * 0.001, fluidName);
            return new EnthalpyAndSpecVolume(enth, specvol);
        }

        public TempAndPressure getTempAndPressureAtPoint(float enthalpy, float specificVolume)
        {
            var temp = CoolProp.PropsSI("T", "H", enthalpy*0.001, "D", (1/specificVolume), fluidName);
            var press = 1 / CoolProp.PropsSI("P", "H", enthalpy* 0.001, "D", (1 / specificVolume), fluidName);
            return new TempAndPressure(temp, press);
        }

        public class TempAndPressure
        {
            public TempAndPressure(double tmp, double prs)
            {
                Temperature = Convert.ToSingle(tmp);
                Pressure = Convert.ToSingle(prs);
            }
            public float Temperature;
            public float Pressure;
        }

        public class EnthalpyAndSpecVolume
        {
            public EnthalpyAndSpecVolume(double enth, double specvol)
            {
                Enthalpy = Convert.ToSingle(enth);
                Enthalpy = Convert.ToSingle(specvol);
            }
            public float Enthalpy;
            public float SpecificVolume;
        }
    }
}