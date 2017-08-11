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
        protected float density;
        protected float pressure;
        protected float temperature;
        protected float internalEnergy;
        protected float entropy;
        protected float totalResource;
        abstract public string fluidName { get; }
        abstract override public Resources managedResource { get; }
        public float molarWeight { get; protected set; }
        public Thermodynamic_ResourceManager thermodynamics;

        public AtmosphericResourceManager(ref Thermodynamic_ResourceManager thermoMan){
            molarWeight = getMolarWeight(managedResource);
            thermodynamics = thermoMan;
        }

        public override void addResource(float resource)
        {

        }

        public override void consumeResource(float resource);

        public abstract override float getLevel();

        public void setState(float temp, float press, float enth, float entr, float dens, float internalener, float totalVolume){
			temperature = temp;
			pressure = press;
			enthalpyPerUnitMass = enth;
            entropy = entr;
            density = dens;
            internalEnergy = internalener;
            totalResource = density * totalVolume; 
		}

        public void setState(ThermoState state, float totalVolume)
        {
            setState(state.Temperature, state.Pressure, state.Enthalpy, state.Entropy, state.Density, state.InternalEnergy, totalVolume);
        }

        public void initiate(ThermoState state, float totalVolume)
        {
            setState(state, totalVolume);
        }

		public float getEnthalpyPerUnitMass(){
			return enthalpyPerUnitMass;
		}

        public float getDensity()
        {
            return density;
        }

        public float getEntropy()
        {
            return entropy;
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

        public float getTotalInternalEnergy()
        {
            return totalResource * internalEnergy;
        }

        public abstract float LitresToKG(float litres);

		public ThermoState getStateFromTempPres(float temp, float pressure){
            var enth = CoolProp.PropsSI("H", "T", temp+273.15F, "P", pressure*1000, fluidName);
            var entropy = CoolProp.PropsSI("S", "T",temp + 273.15F, "P", pressure * 1000, fluidName);
            var density = CoolProp.PropsSI("D", "T", temp + 273.15F, "P", pressure * 1000, fluidName);
            var intenergy = CoolProp.PropsSI("U", "T", temp + 273.15F, "P", pressure * 1000, fluidName);
            return new ThermoState(temp-273.15F, pressure * 0.001, enth*0.001, entropy * 0.001, density, intenergy * 0.001);
        }

        public ThermoState getStateFromInternDensity(float internalenergy, float dens)
        {
            var enth = CoolProp.PropsSI("H", "U", internalenergy* 1000, "D", dens, fluidName);
            var temp = CoolProp.PropsSI("T", "U", internalenergy * 1000, "D", dens, fluidName);
            var entropy = CoolProp.PropsSI("S", "U", internalenergy * 1000, "D", dens, fluidName);
            var density = CoolProp.PropsSI("D", "U", internalenergy * 1000, "D", dens, fluidName);
            return new ThermoState(temp - 273.15F, pressure * 0.001, enth * 0.001, entropy * 0.001, density, internalenergy * 0.001);
        }

        public ThermoState getStateFromEnthEntr(float enthalpy, float entropy)
        {
            var temp = CoolProp.PropsSI("T", "H", enthalpy* 1000, "S", entropy* 1000, fluidName);
            var press = 1 / CoolProp.PropsSI("P", "H", enthalpy* 1000, "S", entropy* 1000, fluidName);
            var density = CoolProp.PropsSI("D", "H", enthalpy * 1000, "S", entropy * 1000, fluidName);
            var intenergy = CoolProp.PropsSI("U", "H", enthalpy * 1000, "S", entropy * 1000, fluidName);
            return new ThermoState(temp - 273.15F, pressure * 0.001, enthalpy * 0.001, entropy * 0.001, density, intenergy * 0.001);
        }

        public class ThermoState
        {
            public ThermoState(double temp, double press, double enth, double entropy, double den, double intener)
            {
                Temperature = Convert.ToSingle(temp);
                Pressure = Convert.ToSingle(press);
                Enthalpy = Convert.ToSingle(enth);
                Entropy = Convert.ToSingle(entropy);
                Density = Convert.ToSingle(den);
                InternalEnergy = Convert.ToSingle(intener);
            }
            public float Temperature;
            public float Pressure;
            public float Enthalpy;
            public float Entropy;
            public float Density;
            public float InternalEnergy;
        }

    }
}