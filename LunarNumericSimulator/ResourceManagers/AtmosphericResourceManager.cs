using CsvHelper;
using LunarNumericSimulator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LunarNumericSimulator.ResourceManagers
{
    public class AtmosphericResourceManager : ResourceManager<double>
    {
        protected double enthalpyPerUnitMass;
        protected double density;
        protected double pressure;
        protected double temperature;
        protected double internalEnergy;
        protected double entropy;
        protected double totalResource;

        public string fluidName { get; protected set; }
        AbstractState fluidTable;
        override public Resources managedResource { get; protected set; }
        public double molarWeight { get; protected set; }
        public ThermodynamicEngine thermodynamics;

        public AtmosphericResourceManager(Resources resource, string fluidname){
            managedResource = resource;
            fluidName = fluidname;
            molarWeight = getMolarWeight(managedResource);
            fluidTable = AbstractState.factory("BICUBIC&HEOS", fluidName);
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

        public override double getLevel()
        {
            return totalResource;
        }

        public void setState(double temp, double press, double enth, double dens, double internalener, double totalVolume){
			temperature = temp;
			pressure = press;
			enthalpyPerUnitMass = enth;
            density = dens;
            internalEnergy = internalener;
            totalResource = density * totalVolume; 
		}

        public void setState(ThermoState state, double totalVolume)
        {
            setState(state.Temperature, state.Pressure, state.Enthalpy, state.Density, state.InternalEnergy, totalVolume);
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

        public double LitresToKG(double litres)
        {
            var kg_L = density * 0.001;
            return litres * kg_L;
        }

		public ThermoState getStateFromTempPres(double tmp, double pres){

            StringVector outputs = new StringVector(new string[]{
                "H",
                "D",
                "U"
            });
            DoubleVector temps = new DoubleVector(new double[] { tmp + 273.15 });
            DoubleVector press = new DoubleVector(new double[] { pres *1000 });
            double[] result = CoolProp.PropsSImulti(outputs, "T", temps , "P", press, "HEOS", new StringVector(new string[] { fluidName }), new DoubleVector(new double[]{ 1 }))[0].ToArray();
            return new ThermoState(tmp, pres, result[0] * 0.001, result[1], result[2] * 0.001);
            
        }

        public ThermoState getStateFromInternDensity(double internalenergy, double dens)
        {

            StringVector outputs = new StringVector(new string[]{
                "T",
                "P",
                "H"
            });
            DoubleVector umass = new DoubleVector(new double[] { internalenergy*1000 });
            DoubleVector dmass = new DoubleVector(new double[] { dens });
            double[] result = CoolProp.PropsSImulti(outputs, "U", umass, "D", dmass, "HEOS", new StringVector(new string[] { fluidName }), new DoubleVector(new double[] { 1 }))[0].ToArray();
            return new ThermoState(result[0] - 273.15, result[1] * 0.001, result[2] * 0.001, dens, internalenergy);
        }

        public ThermoState getStateFromTempDensity(double tmp, double dens)
        {
            StringVector outputs = new StringVector(new string[]{
                "P",
                "U",
                "H"
            });
            DoubleVector temps = new DoubleVector(new double[] { tmp + 273.15 });
            DoubleVector dmass = new DoubleVector(new double[] { dens });
            double[] result = CoolProp.PropsSImulti(outputs, "T", temps, "D", dmass, "HEOS", new StringVector(new string[] { fluidName }), new DoubleVector(new double[] { 1 }))[0].ToArray();
            return new ThermoState(tmp, result[0] * 0.001, result[2] * 0.001, dens, result[1] * 0.001);
        }

        public class ThermoState
        {
            public ThermoState(double temp, double press, double enth, double den, double intener)
            {
                Temperature = temp;
                Pressure = press;
                Enthalpy = enth;
                Density = den;
                InternalEnergy = intener;
            }
            public double Temperature;
            public double Pressure;
            public double Enthalpy;
            public double Density;
            public double InternalEnergy;
        }

    }
}