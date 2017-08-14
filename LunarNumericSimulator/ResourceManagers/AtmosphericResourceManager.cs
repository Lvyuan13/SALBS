using CsvHelper;
using LunarNumericSimulator.Utilities;
using System;
using System.IO;
using System.Linq;

namespace LunarNumericSimulator.ResourceManagers
{
    public abstract class AtmosphericResourceManager : ResourceManager<double>
    {
        protected double enthalpyPerUnitMass;
        protected double density;
        protected double pressure;
        protected double temperature;
        protected double internalEnergy;
        protected double entropy;
        protected double totalResource;
        abstract public string fluidName { get; }
        abstract override public Resources managedResource { get; }
        public double molarWeight { get; protected set; }
        public ThermodynamicEngine thermodynamics;

        public AtmosphericResourceManager(){
            molarWeight = getMolarWeight(managedResource);
        }

        public void setThermoManager(ref ThermodynamicEngine ThermoMan)
        {
            thermodynamics = ThermoMan;
        }

        public override void addResource(double resource)
        {
            totalResource += resource;
            thermodynamics.updateAtmosphere();
        }

        public override void consumeResource(double resource)
        {
            totalResource -= resource;
            thermodynamics.updateAtmosphere();
        }

        public abstract override double getLevel();

        public void setState(double temp, double press, double enth, double entr, double dens, double internalener, double totalVolume){
			temperature = temp;
			pressure = press;
			enthalpyPerUnitMass = enth;
            entropy = entr;
            density = dens;
            internalEnergy = internalener;
            totalResource = density * totalVolume; 
		}

        public void setState(ThermoState state, double totalVolume)
        {
            setState(state.Temperature, state.Pressure, state.Enthalpy, state.Entropy, state.Density, state.InternalEnergy, totalVolume);
        }

        public void initiate(ThermoState state, double totalVolume)
        {
            setState(state, totalVolume);
        }

		public double getEnthalpyPerUnitMass(){
			return enthalpyPerUnitMass;
		}

        public double getInternalEnergyPerUnitMass()
        {
            return internalEnergy;
        }

        public double getDensity()
        {
            return density;
        }

        public double getEntropy()
        {
            return entropy;
        }

        public double getTemperature(){
			return temperature;
		}

		public double getPressure(){
			return pressure;
		}

		public double getTotalEnthalpy(){
			return totalResource * enthalpyPerUnitMass;
		}

        public double getTotalInternalEnergy()
        {
            return totalResource * internalEnergy;
        }

        public abstract double LitresToKG(double litres);

		public ThermoState getStateFromTempPres(double temp, double pressure){
            var enth = CoolProp.PropsSI("H", "T", temp+273.15F, "P", pressure*1000, fluidName);
            var entropy = CoolProp.PropsSI("S", "T",temp + 273.15F, "P", pressure * 1000, fluidName);
            var density = CoolProp.PropsSI("D", "T", temp + 273.15F, "P", pressure * 1000, fluidName);
            var intenergy = CoolProp.PropsSI("U", "T", temp + 273.15F, "P", pressure * 1000, fluidName);
            return new ThermoState(temp, pressure, enth*0.001, entropy * 0.001, density, intenergy * 0.001);
        }

        public ThermoState getStateFromInternDensity(double internalenergy, double dens)
        {
            var enth = CoolProp.PropsSI("H", "U", internalenergy * 1000 , "D", dens, fluidName);
            var temp = CoolProp.PropsSI("T", "U", internalenergy * 1000, "D", dens, fluidName);
            var entropy = CoolProp.PropsSI("S", "U", internalenergy * 1000, "D", dens, fluidName);
            var pressure = CoolProp.PropsSI("P", "U", internalenergy * 1000, "D", dens, fluidName);
            return new ThermoState(temp - 273.15F, pressure * 0.001, enth * 0.001, entropy * 0.001, dens, internalenergy);
        }

        public ThermoState getStateFromEnthEntr(double enthalpy, double entropy)
        {
            var temp = CoolProp.PropsSI("T", "H", enthalpy* 1000, "S", entropy* 1000, fluidName);
            var press = CoolProp.PropsSI("P", "H", enthalpy* 1000, "S", entropy* 1000, fluidName);
            var density = CoolProp.PropsSI("D", "H", enthalpy * 1000, "S", entropy * 1000, fluidName);
            var intenergy = CoolProp.PropsSI("U", "H", enthalpy * 1000, "S", entropy * 1000, fluidName);
            return new ThermoState(temp - 273.15F, pressure * 0.001, enthalpy, entropy, density, intenergy * 0.001);
        }

        public ThermoState getStateFromTempDensity(double temp, double dens)
        {
            var enth = CoolProp.PropsSI("H", "T", temp + 273.15F, "D", dens, fluidName);
            var press = CoolProp.PropsSI("P", "T", temp + 273.15F, "D", dens, fluidName);
            var entr = CoolProp.PropsSI("S", "T", temp + 273.15F, "D", dens, fluidName);
            var intenergy = CoolProp.PropsSI("U", "T", temp + 273.15F, "D", dens, fluidName);
            // Pressure should be x 0.001 but CoolProp is providing output in kPa
            return new ThermoState(temp, pressure, enth * 0.001, entr * 0.001, dens, intenergy * 0.001);
        }

        public class ThermoState
        {
            public ThermoState(double temp, double press, double enth, double entropy, double den, double intener)
            {
                Temperature = temp;
                Pressure = press;
                Enthalpy = enth;
                Entropy = entropy;
                Density = den;
                InternalEnergy = intener;
            }
            public double Temperature;
            public double Pressure;
            public double Enthalpy;
            public double Entropy;
            public double Density;
            public double InternalEnergy;
        }

    }
}