using LunarNumericSimulator.ResourceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator
{
    public partial class ThermodynamicEngine : ResourceManager<double>
    {

        public AtmosphericResourceManager ch4ResourceManager;
        public AtmosphericResourceManager co2ResourceManager;
        public AtmosphericResourceManager oResourceManager;
        public AtmosphericResourceManager nResourceManager;
        public AtmosphericResourceManager h2oResourceManager;

        protected double systemVolume;

        public override Resources managedResource
        {
            get
            {
                return Resources.Heat;
            }
            protected set
            {

            }
        }

        public ThermodynamicEngine(ref AtmosphericResourceManager CH4RMS, ref AtmosphericResourceManager H2ORMS, ref AtmosphericResourceManager CO2RMS, ref AtmosphericResourceManager ORMS, ref AtmosphericResourceManager NRMS, Dictionary<string, double> config, double totalVolume)
        {
            ch4ResourceManager = CH4RMS;
            co2ResourceManager = CO2RMS;
            oResourceManager = ORMS;
            nResourceManager = NRMS;
            h2oResourceManager = H2ORMS;


            double startO, startN, startCO2, initialTemp, initialPressure, startCH4, initialRelativeHumidity;
            try
            {
                config.TryGetValue("starting_Temp", out initialTemp);
                config.TryGetValue("atmospheric_CO2_start", out startCO2);
                config.TryGetValue("atmospheric_O_start", out startO);
                config.TryGetValue("atmospheric_N_start", out startN);
                config.TryGetValue("atmospheric_CH4_start", out startCH4);
                config.TryGetValue("starting_Pressure", out initialPressure);
                config.TryGetValue("atmospheric_relative_humidity", out initialRelativeHumidity);
            }
            catch (Exception e)
            {
                throw e;
            }

            systemVolume = totalVolume;

            setupEnvironment(startCO2, startO, startN, startCH4, initialPressure, initialTemp, initialRelativeHumidity);
        }

        protected void setupEnvironment(double startCO2, double startO, double startN, double startCH4, double startingPressure, double startingTemp, double initialRelativeHumidity)
        {
            double partialPressure = 0;
            ThermoState thermoStateVars;

            double molarFraction = startCO2;
            if (startCO2 != 0) {
                partialPressure = molarFraction * startingPressure;
                thermoStateVars = co2ResourceManager.getStateFromTempPres(startingTemp, partialPressure);
                co2ResourceManager.initiate(thermoStateVars, systemVolume);
            }

            molarFraction = startCH4;
            if (startCH4 != 0) {
                partialPressure = molarFraction * startingPressure;
                thermoStateVars = ch4ResourceManager.getStateFromTempPres(startingTemp, partialPressure);
                ch4ResourceManager.initiate(thermoStateVars, systemVolume);
            }

            molarFraction = startO;
            if (startO != 0) {
                partialPressure = molarFraction * startingPressure;
                thermoStateVars = oResourceManager.getStateFromTempPres(startingTemp, partialPressure);
                oResourceManager.initiate(thermoStateVars, systemVolume);
            }

            molarFraction = startN;
            if (startN != 0)
            {
                partialPressure = molarFraction * startingPressure;
                thermoStateVars = nResourceManager.getStateFromTempPres(startingTemp, partialPressure);
                nResourceManager.initiate(thermoStateVars, systemVolume);
            }

            double eqVapourPressure = ArdenBuckEquation(startingTemp);
            partialPressure = initialRelativeHumidity * eqVapourPressure;
            thermoStateVars = h2oResourceManager.getStateFromTempPres(startingTemp, partialPressure);
            h2oResourceManager.initiate(thermoStateVars, systemVolume);
        }

        private double ArdenBuckEquation(double T)
        {
            return 0.61121 * Math.Exp((18.678 - (T / 234.5)) * (T / (257.14 + T))); // Implementation of the Arden Buck Equation, which estimates the equilibrium vapour pressure.
        }

        public override void addResource(double resource)
        {
            addHeat(resource);
        }

        public override void consumeResource(double resource)
        {
            addHeat(-resource);
        }

        public override double getLevel()
        {
            double totalEnthalpy = (from element in getResourceManagers()
                                    select element.getTotalEnthalpy()).Sum();
            return totalEnthalpy;
        }

        /*
         * Calculates system pressure by summation of gas partial pressures
         * */
        public double getSystemPressure()
        {
            return (from element in getResourceManagers()
                    select element.Pressure).Sum();
        }

        public double getSystemMass()
        {
            return (from element in getResourceManagers()
                    select element.getLevel()).Sum();
        }

        public double getSystemEnthalpy()
        {
            double totalEnthalpy = (from element in getResourceManagers()
                                    select element.getTotalEnthalpy()).Sum();
            return totalEnthalpy;
        }

        public double getSystemInternalEnergy()
        {
            double total = (from element in getResourceManagers()
                                    select element.getTotalInternalEnergy()).Sum();
            return total;
        }


        public AirThermoState getAverageAirState()
        {
            double CO2, O, N, CH4, H2O, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;
            H2O = h2oResourceManager.getLevel() / systemMass;

            double averageMolarWeight = getSystemAverageMolarWeight();

            double molarFractionCO2 = CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
            double molarFractionCH4 = CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
            double molarFractionO = O * (averageMolarWeight / oResourceManager.molarWeight);
            double molarFractionN = N * (averageMolarWeight / nResourceManager.molarWeight);
            double molarFractionH2O = H2O * (averageMolarWeight / h2oResourceManager.molarWeight);

            var systemPressure = getSystemPressure();

            var weightedAverageSpecHeat = molarFractionCO2 * co2ResourceManager.SpecificHeat +
                molarFractionCH4 * ch4ResourceManager.SpecificHeat +
                molarFractionO * oResourceManager.SpecificHeat +
                molarFractionN * nResourceManager.SpecificHeat +
                molarFractionH2O * h2oResourceManager.SpecificHeat;

            var weightedAverageInternalEnergy = molarFractionCO2 * co2ResourceManager.InternalEnergy +
                molarFractionCH4 * ch4ResourceManager.InternalEnergy +
                molarFractionO * oResourceManager.InternalEnergy +
                molarFractionN * nResourceManager.InternalEnergy +
                molarFractionH2O * h2oResourceManager.InternalEnergy;

            var weightedAverageEnthalpy = molarFractionCO2 * co2ResourceManager.EnthalpyPerUnitMass +
                molarFractionCH4 * ch4ResourceManager.EnthalpyPerUnitMass +
                molarFractionO * oResourceManager.EnthalpyPerUnitMass +
                molarFractionN * nResourceManager.EnthalpyPerUnitMass +
                molarFractionH2O * h2oResourceManager.EnthalpyPerUnitMass;

            var weightedAverageCond = molarFractionCO2 * co2ResourceManager.ThermalConductivity +
                molarFractionCH4 * ch4ResourceManager.ThermalConductivity +
                molarFractionO * oResourceManager.ThermalConductivity +
                molarFractionN * nResourceManager.ThermalConductivity +
                molarFractionH2O * h2oResourceManager.ThermalConductivity;

            var weightedAverageVisc = molarFractionCO2 * co2ResourceManager.Viscosity +
                molarFractionCH4 * ch4ResourceManager.Viscosity +
                molarFractionO * oResourceManager.Viscosity +
                molarFractionN * nResourceManager.Viscosity +
                molarFractionH2O * h2oResourceManager.Viscosity;

            var weightedAverageDens = molarFractionCO2 * co2ResourceManager.Density +
                molarFractionCH4 * ch4ResourceManager.Density +
                molarFractionO * oResourceManager.Density +
                molarFractionN * nResourceManager.Density +
                molarFractionH2O * h2oResourceManager.Density;

            var weightedAverageTemp = molarFractionCO2 * co2ResourceManager.Temperature +
                molarFractionCH4 * ch4ResourceManager.Temperature +
                molarFractionO * oResourceManager.Temperature +
                molarFractionN * nResourceManager.Temperature +
                molarFractionH2O * h2oResourceManager.Temperature;

            double relativeHumidity = h2oResourceManager.Pressure / ArdenBuckEquation(h2oResourceManager.Temperature);

            return new AirThermoState(weightedAverageTemp, systemPressure, weightedAverageEnthalpy, weightedAverageDens, weightedAverageInternalEnergy, weightedAverageCond, weightedAverageVisc, weightedAverageSpecHeat, relativeHumidity);
        }



        /* 
        * This function is called whenever atmospheric resources are consumed or produced. It recalculates the properties all gasses in the atmosphere
        * and treating the process as isothermal. This is carried out by calculating the new density of the system using the known system mass, and then 
        * holding temperature constant to find the new thermodynamic state.
        * */
        public void updateAtmosphere()
        {
            double CO2, O, N, CH4, H2O;
            CO2 = co2ResourceManager.getLevel() / systemVolume;
            O = oResourceManager.getLevel() / systemVolume;
            N = nResourceManager.getLevel() / systemVolume;
            CH4 = ch4ResourceManager.getLevel() / systemVolume;
            H2O = h2oResourceManager.getLevel() / systemVolume;

            double temp = 0;
            ThermoState thermoStateVars;

            if (CO2 != 0)
            {
                temp = co2ResourceManager.Temperature; // This is an isothermal density adjustment
                thermoStateVars = co2ResourceManager.getStateFromTempDensity(temp, CO2);
                co2ResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (CH4 != 0)
            {
                temp = ch4ResourceManager.Temperature; // This is an isothermal density adjustment
                thermoStateVars = ch4ResourceManager.getStateFromTempDensity(temp, CH4);
                ch4ResourceManager.setState(thermoStateVars, systemVolume);
            }
            if (O != 0)
            {
                temp = oResourceManager.Temperature; // This is an isothermal density adjustment
                thermoStateVars = oResourceManager.getStateFromTempDensity(temp, O);
                oResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (N != 0)
            {
                temp = nResourceManager.Temperature; // This is an isothermal density adjustment
                thermoStateVars = nResourceManager.getStateFromTempDensity(temp, N);
                nResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (H2O != 0)
            {
                temp = h2oResourceManager.Temperature; // This is an isothermal density adjustment
                thermoStateVars = h2oResourceManager.getStateFromTempDensity(temp,H2O);
                h2oResourceManager.setState(thermoStateVars, systemVolume);
            }

        }

        /* 
         * This function handles the addition of or removal of heat from atmospheric gas. The total internal energy within the atmosphere is first 
         * computed, and then the mass fractions of individual gasses are calculated. The heat to be added is split among the atmospheric gasses
         * proportionately to their molar fraction
         * 
         * For example, if Nitrogen constitutes 71% of the atmosphere, it will absorb 71% of the incoming heat.
         * */
        public void addHeat(double quantity)
        {
            double totalInternalEnergyInSystem = getSystemInternalEnergy();
            double CO2, O, N, CH4, H2O, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;
            H2O = h2oResourceManager.getLevel() / systemMass;

            double averageMolarWeight = getSystemAverageMolarWeight();

            double molarFraction = 0;
            double internalenergy = 0;
            double density = 0;
            ThermoState thermoStateVars;

            if (CO2 != 0)
            {
                density = co2ResourceManager.Density; // This is an isochoric heat addition (density is constant)
                molarFraction = CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
                internalenergy = (molarFraction * (quantity) / co2ResourceManager.getLevel()) + co2ResourceManager.InternalEnergy;
                thermoStateVars = co2ResourceManager.getStateFromInternDensity(internalenergy, density);
                co2ResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (CH4 != 0)
            {
                density = ch4ResourceManager.Density; // This is an isochoric heat addition (density is constant)
                molarFraction = CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
                internalenergy = (molarFraction * (quantity) / ch4ResourceManager.getLevel()) + ch4ResourceManager.InternalEnergy;
                thermoStateVars = ch4ResourceManager.getStateFromInternDensity(internalenergy, density);
                ch4ResourceManager.setState(thermoStateVars, systemVolume);
            }
            if (O != 0)
            {
                density = oResourceManager.Density; // This is an isochoric heat addition (density is constant)
                molarFraction = O * (averageMolarWeight / oResourceManager.molarWeight);
                internalenergy = (molarFraction * (quantity) / oResourceManager.getLevel()) + oResourceManager.InternalEnergy;
                thermoStateVars = oResourceManager.getStateFromInternDensity(internalenergy, density);
                oResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (N != 0)
            {
                density = nResourceManager.Density; // This is an isochoric heat addition (density is constant)
                molarFraction = N * (averageMolarWeight / nResourceManager.molarWeight);
                internalenergy = (molarFraction * (quantity) / nResourceManager.getLevel()) + nResourceManager.InternalEnergy;
                thermoStateVars = nResourceManager.getStateFromInternDensity(internalenergy, density);
                nResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (H2O != 0)
            {
                density = h2oResourceManager.Density; // This is an isochoric heat addition (density is constant)
                molarFraction = H2O * (averageMolarWeight / h2oResourceManager.molarWeight);
                internalenergy = (molarFraction * (quantity) / h2oResourceManager.getLevel()) + h2oResourceManager.InternalEnergy;
                thermoStateVars = h2oResourceManager.getStateFromInternDensity(internalenergy, density);
                h2oResourceManager.setState(thermoStateVars, systemVolume);
            }

        }

        protected double getSystemAverageMolarWeight()
        {
            double systemMass = getSystemMass();
            double massFractionCO2 = co2ResourceManager.getLevel() / systemMass;
            double massFractionCH4 = ch4ResourceManager.getLevel() / systemMass;
            double massFractionO = oResourceManager.getLevel() / systemMass;
            double massFractionN = nResourceManager.getLevel() / systemMass;
            double massFractionH2O = h2oResourceManager.getLevel() / systemMass;

            double averageMolarWeight = (massFractionCO2 * co2ResourceManager.molarWeight);
            averageMolarWeight += (massFractionO * oResourceManager.molarWeight);
            averageMolarWeight += (massFractionN * nResourceManager.molarWeight);
            averageMolarWeight += (massFractionCH4 * ch4ResourceManager.molarWeight);
            averageMolarWeight += (massFractionH2O * h2oResourceManager.molarWeight);
            return averageMolarWeight;
        }

        public double getMassFraction(Resources res)
        {
            double CO2, O, N, CH4, H2O, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;
            H2O = h2oResourceManager.getLevel() / systemMass;

            switch (res)
            {
                case Resources.CO2:
                    return CO2;
                case Resources.O2:
                    return O;
                case Resources.N2:
                    return N;
                case Resources.CH4:
                    return CH4;
                case Resources.Humidity:
                    return H2O;
                default:
                    throw new Exception("Resource is not an atomospheric resource");
            }
        }

        public double getMolarFraction(Resources res)
        {
            double CO2, O, N, CH4, H2O, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;
            H2O = h2oResourceManager.getLevel() / systemMass;

            double averageMolarWeight = getSystemAverageMolarWeight();

            switch (res)
            {
                case Resources.CO2:
                    return CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
                case Resources.O2:
                    return O * (averageMolarWeight / oResourceManager.molarWeight);
                case Resources.N2:
                    return N * (averageMolarWeight / nResourceManager.molarWeight);
                case Resources.CH4:
                    return CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
                case Resources.H2O:
                    return H2O * (averageMolarWeight / h2oResourceManager.molarWeight);
                default:
                    throw new Exception("Resource is not an atomospheric resource");
            }
        }

        protected double calculateMolarMassFromMassFraction(double molarWeight, double massFraction)
        {
            return massFraction * (getSystemAverageMolarWeight() / molarWeight);
        }

        protected AtmosphericResourceManager[] getResourceManagers()
        {
            return new AtmosphericResourceManager[]
            {
                nResourceManager,
                oResourceManager,
                co2ResourceManager,
                ch4ResourceManager,
                h2oResourceManager
            };
        }
    }


}