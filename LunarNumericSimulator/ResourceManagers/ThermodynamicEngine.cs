using LunarNumericSimulator.ResourceManagers;
using System;
using System.Collections.Generic;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator
{
    public partial class ThermodynamicEngine : ResourceManager<double>
    {

        public AtmosphericResourceManager ch4ResourceManager;
        public AtmosphericResourceManager co2ResourceManager;
        public AtmosphericResourceManager oResourceManager;
        public AtmosphericResourceManager nResourceManager;

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

        public ThermodynamicEngine(ref AtmosphericResourceManager CH4RMS, ref AtmosphericResourceManager CO2RMS, ref AtmosphericResourceManager ORMS, ref AtmosphericResourceManager NRMS, Dictionary<string, double> config, double totalVolume)
        {
            ch4ResourceManager = CH4RMS;
            co2ResourceManager = CO2RMS;
            oResourceManager = ORMS;
            nResourceManager = NRMS;


            double startO, startN, startCO2, initialTemp, initialPressure, startCH4;
            try
            {
                config.TryGetValue("starting_Temp", out initialTemp);
                config.TryGetValue("atmospheric_CO2_start", out startCO2);
                config.TryGetValue("atmospheric_O_start", out startO);
                config.TryGetValue("atmospheric_N_start", out startN);
                config.TryGetValue("atmospheric_CH4_start", out startCH4);
                config.TryGetValue("starting_Pressure", out initialPressure);
            }
            catch (Exception e)
            {
                throw e;
            }

            systemVolume = totalVolume;

            setupEnvironment(startCO2, startO, startN, startCH4, initialPressure, initialTemp);
        }

        protected void setupEnvironment(double startCO2, double startO, double startN, double startCH4, double startingPressure, double startingTemp)
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
            double totalEnthalpy = ch4ResourceManager.getTotalEnthalpy();
            totalEnthalpy += co2ResourceManager.getTotalEnthalpy();
            totalEnthalpy += oResourceManager.getTotalEnthalpy();
            totalEnthalpy += nResourceManager.getTotalEnthalpy();
            return totalEnthalpy;
        }

        /*
         * Calculates system pressure by summation of gas partial pressures
         * */
        public double getSystemPressure()
        {
            var CO2 = co2ResourceManager.Pressure;
            var O = oResourceManager.Pressure;
            var N = nResourceManager.Pressure;
            var CH4 = ch4ResourceManager.Pressure;
            return CO2 + O + N + CH4;
        }

        public double getSystemMass()
        {
            double totalMass = ch4ResourceManager.getLevel();
            totalMass += co2ResourceManager.getLevel();
            totalMass += oResourceManager.getLevel();
            totalMass += nResourceManager.getLevel();
            return totalMass;
        }

        public double getSystemEnthalpy()
        {
            var CO2 = co2ResourceManager.getTotalEnthalpy();
            var O = oResourceManager.getTotalEnthalpy();
            var N = nResourceManager.getTotalEnthalpy();
            var CH4 = ch4ResourceManager.getTotalEnthalpy();
            return CO2 + O + N + CH4;
        }

        public double getSystemInternalEnergy()
        {
            var CO2 = co2ResourceManager.getTotalInternalEnergy();
            var O = oResourceManager.getTotalInternalEnergy();
            var N = nResourceManager.getTotalInternalEnergy();
            var CH4 = ch4ResourceManager.getTotalInternalEnergy();
            return CO2 + O + N + CH4;
        }


        public ThermoState getAverageAirState()
        {
            double CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

            double averageMolarWeight = getSystemAverageMolarWeight();

            double molarFractionCO2 = CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
            double molarFractionCH4 = CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
            double molarFractionO = O * (averageMolarWeight / oResourceManager.molarWeight);
            double molarFractionN = N * (averageMolarWeight / nResourceManager.molarWeight);

            var systemPressure = getSystemPressure();

            var weightedAverageSpecHeat = molarFractionCO2 * co2ResourceManager.SpecificHeat +
                molarFractionCH4 * ch4ResourceManager.SpecificHeat +
                molarFractionO * oResourceManager.SpecificHeat +
                molarFractionN * nResourceManager.SpecificHeat;

            var weightedAverageInternalEnergy = molarFractionCO2 * co2ResourceManager.InternalEnergy +
                molarFractionCH4 * ch4ResourceManager.InternalEnergy +
                molarFractionO * oResourceManager.InternalEnergy +
                molarFractionN * nResourceManager.InternalEnergy;

            var weightedAverageEnthalpy = molarFractionCO2 * co2ResourceManager.EnthalpyPerUnitMass +
                molarFractionCH4 * ch4ResourceManager.EnthalpyPerUnitMass +
                molarFractionO * oResourceManager.EnthalpyPerUnitMass +
                molarFractionN * nResourceManager.EnthalpyPerUnitMass;

            var weightedAverageCond = molarFractionCO2 * co2ResourceManager.ThermalConductivity +
                molarFractionCH4 * ch4ResourceManager.ThermalConductivity +
                molarFractionO * oResourceManager.ThermalConductivity +
                molarFractionN * nResourceManager.ThermalConductivity;

            var weightedAverageVisc = molarFractionCO2 * co2ResourceManager.Viscosity +
                molarFractionCH4 * ch4ResourceManager.Viscosity +
                molarFractionO * oResourceManager.Viscosity +
                molarFractionN * nResourceManager.Viscosity;

            var weightedAverageDens = molarFractionCO2 * co2ResourceManager.Density +
                molarFractionCH4 * ch4ResourceManager.Density +
                molarFractionO * oResourceManager.Density +
                molarFractionN * nResourceManager.Density;

            var weightedAverageTemp = molarFractionCO2 * co2ResourceManager.Temperature +
                molarFractionCH4 * ch4ResourceManager.Temperature +
                molarFractionO * oResourceManager.Temperature +
                molarFractionN * nResourceManager.Temperature;

            return new ThermoState(weightedAverageTemp, systemPressure, weightedAverageEnthalpy, weightedAverageDens, weightedAverageInternalEnergy, weightedAverageCond, weightedAverageVisc, weightedAverageSpecHeat);
        }



        /* 
        * This function is called whenever atmospheric resources are consumed or produced. It recalculates the properties all gasses in the atmosphere
        * and treating the process as isothermal. This is carried out by calculating the new density of the system using the known system mass, and then 
        * holding temperature constant to find the new thermodynamic state.
        * */
        public void updateAtmosphere()
        {
            double CO2, O, N, CH4;
            CO2 = co2ResourceManager.getLevel() / systemVolume;
            O = oResourceManager.getLevel() / systemVolume;
            N = nResourceManager.getLevel() / systemVolume;
            CH4 = ch4ResourceManager.getLevel() / systemVolume;

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
            double CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

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

        }

        protected double getSystemAverageMolarWeight()
        {
            double systemMass = getSystemMass();
            double massFractionCO2 = co2ResourceManager.getLevel() / systemMass;
            double massFractionCH4 = ch4ResourceManager.getLevel() / systemMass;
            double massFractionO = oResourceManager.getLevel() / systemMass;
            double massFractionN = nResourceManager.getLevel() / systemMass;

            double averageMolarWeight = (massFractionCO2 * co2ResourceManager.molarWeight);
            averageMolarWeight += (massFractionO * oResourceManager.molarWeight);
            averageMolarWeight += (massFractionN * nResourceManager.molarWeight);
            averageMolarWeight += (massFractionCH4 * ch4ResourceManager.molarWeight);
            return averageMolarWeight;
        }

        public double getMassFraction(Resources res)
        {
            double CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

            switch (res)
            {
                case Resources.CO2:
                    return CO2;
                case Resources.O:
                    return O;
                case Resources.N:
                    return N;
                case Resources.CH4:
                    return CH4;
                default:
                    throw new Exception("Resource is not an atomospheric resource");
            }
        }

        public double getMolarFraction(Resources res)
        {
            double CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

            double averageMolarWeight = getSystemAverageMolarWeight();

            switch (res)
            {
                case Resources.CO2:
                    return CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
                case Resources.O:
                    return O * (averageMolarWeight / oResourceManager.molarWeight);
                case Resources.N:
                    return N * (averageMolarWeight / nResourceManager.molarWeight);
                case Resources.CH4:
                    return CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
                default:
                    throw new Exception("Resource is not an atomospheric resource");
            }
        }

        protected double calculateMolarMassFromMassFraction(double molarWeight, double massFraction)
        {
            return massFraction * (getSystemAverageMolarWeight() / molarWeight);
        }
    }


}