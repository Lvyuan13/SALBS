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
        public double EnthalpyPerUnitMass { protected set; get; }
        public double Density { protected set; get; }
        public double Pressure { protected set; get; }
        public double Temperature { protected set; get; }
        public double InternalEnergy { protected set; get; }
        public double Entropy { protected set; get; }
        public double TotalResource { protected set; get; }
        public double ThermalConductivity { protected set; get; }
        public double Viscosity { protected set; get; }
        public double SpecificHeat { protected set; get; }

        public string FluidName { get; protected set; }
        public string propsDatabase = "HEOS";
        override public Resources managedResource { get; protected set; }
        public double molarWeight { get; protected set; }
        public ThermodynamicEngine thermodynamics;

        public AtmosphericResourceManager(Resources resource, string fluidname)
        {
            managedResource = resource;
            FluidName = fluidname;
            molarWeight = getMolarWeight(managedResource);
        }

        public void setThermoManager(ref ThermodynamicEngine ThermoMan)
        {
            thermodynamics = ThermoMan;
        }

        public override void addResource(double resource)
        {
            TotalResource += resource;
            thermodynamics.updateAtmosphere();
        }

        public override void consumeResource(double resource)
        {
            TotalResource -= resource;
            thermodynamics.updateAtmosphere();
        }

        public override double getLevel()
        {
            return TotalResource;
        }

        public void setState(double temp, double press, double enth, double dens, double internalener, double thermcond, double visc, double specheat, double totalVolume)
        {
            Temperature = temp;
            Pressure = press;
            EnthalpyPerUnitMass = enth;
            Density = dens;
            InternalEnergy = internalener;
            ThermalConductivity = thermcond;
            Viscosity = visc;
            SpecificHeat = specheat;
            TotalResource = Density * totalVolume;
        }

        public void setState(ThermoState state, double totalVolume)
        {
            setState(state.Temperature, state.Pressure, state.Enthalpy, state.Density, state.InternalEnergy, state.ThermalConductivity, state.Viscosity, state.SpecificHeat, totalVolume);
        }

        public void initiate(ThermoState state, double totalVolume)
        {
            setState(state, totalVolume);
        }


        public double getTotalEnthalpy()
        {
            return TotalResource * EnthalpyPerUnitMass;
        }

        public double getTotalInternalEnergy()
        {
            return TotalResource * InternalEnergy;
        }

        public double LitresToKG(double litres)
        {
            var kg_L = Density * 0.001;
            return litres * kg_L;
        }

        public ThermoState getStateFromTempPres(double tmp, double pres)
        {

            StringVector outputs = new StringVector(new string[]{
                "H",
                "D",
                "U",
                "L",
                "V",
                "C"
            });
            DoubleVector temps = new DoubleVector(new double[] { tmp + 273.15 });
            DoubleVector press = new DoubleVector(new double[] { pres * 1000 });
            double[] result = CoolProp.PropsSImulti(outputs, "T", temps, "P", press, propsDatabase, new StringVector(new string[] { FluidName }), new DoubleVector(new double[] { 1 }))[0].ToArray();
            return new ThermoState(tmp, pres, result[0] * 0.001, result[1], result[2] * 0.001, result[3], result[4], result[5]);

        }

        public ThermoState getStateFromInternDensity(double internalenergy, double dens)
        {

            StringVector outputs = new StringVector(new string[]{
                "T",
                "P",
                "H",
                "L",
                "V",
                "C"
            });
            DoubleVector umass = new DoubleVector(new double[] { internalenergy * 1000 });
            DoubleVector dmass = new DoubleVector(new double[] { dens });
            double[] result = CoolProp.PropsSImulti(outputs, "U", umass, "D", dmass, propsDatabase, new StringVector(new string[] { FluidName }), new DoubleVector(new double[] { 1 }))[0].ToArray();
            return new ThermoState(result[0] - 273.15, result[1] * 0.001, result[2] * 0.001, dens, internalenergy, result[3], result[4], result[5]);
        }

        public ThermoState getStateFromTempDensity(double tmp, double dens)
        {
            StringVector outputs = new StringVector(new string[]{
                "P",
                "U",
                "H",
                "L",
                "V",
                "C"
            });
            double tmp2 = CoolProp.Props1SI(FluidName, "P_REDUCING");
            DoubleVector temps = new DoubleVector(new double[] { tmp + 273.15 });
            DoubleVector dmass = new DoubleVector(new double[] { dens });
            double[] result = CoolProp.PropsSImulti(outputs, "T", temps, "D", dmass, propsDatabase, new StringVector(new string[] { FluidName }), new DoubleVector(new double[] { 1 }))[0].ToArray();
            return new ThermoState(tmp, result[0] * 0.001, result[2] * 0.001, dens, result[1] * 0.001, result[3], result[4], result[5]);
        }

        public class ThermoState
        {
            public ThermoState(double temp, double press, double enth, double den, double intener, double thermcond, double visc, double speche)
            {
                Temperature = temp;
                Pressure = press;
                Enthalpy = enth;
                Density = den;
                InternalEnergy = intener;
                ThermalConductivity = thermcond;
                Viscosity = visc;
                SpecificHeat = speche;
            }
            public double Temperature;
            public double Pressure;
            public double Enthalpy;
            public double Density;
            public double InternalEnergy;
            public double ThermalConductivity;
            public double Viscosity;
            public double SpecificHeat;
        }

    }
}