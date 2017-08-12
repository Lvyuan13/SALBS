using System;
using System.Collections;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;
using System.Linq;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator
{
    public partial class ThermodynamicEngine : ResourceManager<float>
    {

        public CH4_ResourceManager ch4ResourceManager;
        public CO2_ResourceManager co2ResourceManager;
        public O_ResourceManager oResourceManager;
        public N_ResourceManager nResourceManager;

        protected float systemVolume;

        public override Resources managedResource
        {
            get
            {
                return Resources.Heat;
            }
        }

        public ThermodynamicEngine(ref CH4_ResourceManager CH4RMS, ref CO2_ResourceManager CO2RMS, ref O_ResourceManager ORMS, ref N_ResourceManager NRMS, Dictionary<string, float> config, float totalVolume)
        {
            ch4ResourceManager = CH4RMS;
            co2ResourceManager = CO2RMS;
            oResourceManager = ORMS;
            nResourceManager = NRMS;

            float startO, startN, startCO2, initialTemp, initialPressure, startCH4;
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

        protected void setupEnvironment(float startCO2, float startO, float startN, float startCH4, float startingPressure, float startingTemp)
        {
            float partialPressure = 0;
            ThermoState thermoStateVars;

            float molarFraction = startCO2;
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

        public override void addResource(float resource)
        {
            addHeat(resource);
        }

        public override void consumeResource(float resource)
        {
            addHeat(-resource);
        }

        public override float getLevel()
        {
            float totalEnthalpy = ch4ResourceManager.getTotalEnthalpy();
            totalEnthalpy += co2ResourceManager.getTotalEnthalpy();
            totalEnthalpy += oResourceManager.getTotalEnthalpy();
            totalEnthalpy += nResourceManager.getTotalEnthalpy();
            return totalEnthalpy;
        }

        /*
         * Calculates system pressure by summation of gas partial pressures
         * */
        public float getSystemPressure()
        {
            var CO2 = co2ResourceManager.getPressure();
            var O = oResourceManager.getPressure();
            var N = nResourceManager.getPressure();
            var CH4 = ch4ResourceManager.getPressure();
            return CO2 + O + N + CH4;
        }

        /*
         * Calculates system temperature by taking weighted average of the constituent gas temperatures
         * */
        public float getSystemTemperature()
        {
            float CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

            float averageMolarWeight = getSystemAverageMolarWeight();

            float molarFractionCO2 = CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
            float molarFractionCH4 = CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
            float molarFractionO = O * (averageMolarWeight / oResourceManager.molarWeight);
            float molarFractionN = N * (averageMolarWeight / nResourceManager.molarWeight);

            var weightedAverageTemp = molarFractionCO2 * co2ResourceManager.getTemperature() +
                molarFractionCH4 * ch4ResourceManager.getTemperature() +
                molarFractionO * oResourceManager.getTemperature() +
                molarFractionN * nResourceManager.getTemperature();

            return weightedAverageTemp;
        }

        public float getSystemMass()
        {
            float totalMass = ch4ResourceManager.getLevel();
            totalMass += co2ResourceManager.getLevel();
            totalMass += oResourceManager.getLevel();
            totalMass += nResourceManager.getLevel();
            return totalMass;
        }

        public float getSystemEnthalpy()
        {
            var CO2 = co2ResourceManager.getTotalEnthalpy();
            var O = oResourceManager.getTotalEnthalpy();
            var N = nResourceManager.getTotalEnthalpy();
            var CH4 = ch4ResourceManager.getTotalEnthalpy();
            return CO2 + O + N + CH4;
        }

        public float getSystemInternalEnergy()
        {
            var CO2 = co2ResourceManager.getTotalInternalEnergy();
            var O = oResourceManager.getTotalInternalEnergy();
            var N = nResourceManager.getTotalInternalEnergy();
            var CH4 = ch4ResourceManager.getTotalInternalEnergy();
            return CO2 + O + N + CH4;
        }

        /* 
        * This function is called whenever atmospheric resources are consumed or produced. It recalculates the properties all gasses in the atmosphere
        * and treating the process as isothermal. This is carried out by calculating the new density of the system using the known system mass, and then 
        * holding temperature constant to find the new thermodynamic state.
        * */
        public void updateAtmosphere()
        {
            float CO2, O, N, CH4;
            CO2 = co2ResourceManager.getLevel() / systemVolume;
            O = oResourceManager.getLevel() / systemVolume;
            N = nResourceManager.getLevel() / systemVolume;
            CH4 = ch4ResourceManager.getLevel() / systemVolume;

            float internalener = 0;
            float temp = 0;
            ThermoState thermoStateVars;

            if (CO2 != 0)
            {
                temp = co2ResourceManager.getTemperature(); // This is an isothermal density adjustment
                thermoStateVars = co2ResourceManager.getStateFromTempDensity(internalener, CO2);
                co2ResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (CH4 != 0)
            {
                temp = ch4ResourceManager.getTemperature(); // This is an isothermal density adjustment
                thermoStateVars = ch4ResourceManager.getStateFromTempDensity(internalener, CH4);
                ch4ResourceManager.setState(thermoStateVars, systemVolume);
            }
            if (O != 0)
            {
                temp = oResourceManager.getTemperature(); // This is an isothermal density adjustment
                thermoStateVars = oResourceManager.getStateFromTempDensity(internalener, O);
                oResourceManager.setState(thermoStateVars, systemVolume);
            }

            if (N != 0)
            {
                temp = nResourceManager.getTemperature(); // This is an isothermal density adjustment
                thermoStateVars = nResourceManager.getStateFromTempDensity(internalener, N);
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
        public void addHeat(float quantity)
        {
            float totalInternalEnergyInSystem = getSystemInternalEnergy();
            float CO2, O, N, CH4, systemMass = getSystemMass();
            CO2 = co2ResourceManager.getLevel() / systemMass;
            O = oResourceManager.getLevel() / systemMass;
            N = nResourceManager.getLevel() / systemMass;
            CH4 = ch4ResourceManager.getLevel() / systemMass;

            float averageMolarWeight = getSystemAverageMolarWeight();

            float molarFraction = 0;
            float internalener = 0;
            float density = 0;
            ThermoState thermoStateVars;

            density = co2ResourceManager.getDensity(); // This is an isochoric heat addition (density is constant)
            molarFraction = CO2 * (averageMolarWeight / co2ResourceManager.molarWeight);
            internalener = molarFraction * (totalInternalEnergyInSystem + quantity);
            thermoStateVars = co2ResourceManager.getStateFromInternDensity(internalener, density);
            co2ResourceManager.setState(thermoStateVars, systemVolume);

            density = ch4ResourceManager.getDensity(); // This is an isochoric heat addition (density is constant)
            molarFraction = CH4 * (averageMolarWeight / ch4ResourceManager.molarWeight);
            internalener = molarFraction * (totalInternalEnergyInSystem + quantity);
            thermoStateVars = ch4ResourceManager.getStateFromInternDensity(internalener, density);
            ch4ResourceManager.setState(thermoStateVars, systemVolume);

            density = oResourceManager.getDensity(); // This is an isochoric heat addition (density is constant)
            molarFraction = O * (averageMolarWeight / oResourceManager.molarWeight);
            internalener = molarFraction * (totalInternalEnergyInSystem + quantity);
            thermoStateVars = oResourceManager.getStateFromInternDensity(internalener, density);
            oResourceManager.setState(thermoStateVars, systemVolume);

            density = nResourceManager.getDensity(); // This is an isochoric heat addition (density is constant)
            molarFraction = N * (averageMolarWeight / nResourceManager.molarWeight);
            internalener = molarFraction * (totalInternalEnergyInSystem + quantity);
            thermoStateVars = nResourceManager.getStateFromInternDensity(internalener, density);
            nResourceManager.setState(thermoStateVars, systemVolume);

        }

        protected float getSystemAverageMolarWeight()
        {
            float systemMass = getSystemMass();
            float massFractionCO2 = co2ResourceManager.getLevel() / systemMass;
            float massFractionCH4 = ch4ResourceManager.getLevel() / systemMass;
            float massFractionO = oResourceManager.getLevel() / systemMass;
            float massFractionN = nResourceManager.getLevel() / systemMass;

            float averageMolarWeight = (massFractionCO2 * co2ResourceManager.molarWeight);
            averageMolarWeight += (massFractionO * oResourceManager.molarWeight);
            averageMolarWeight += (massFractionN * nResourceManager.molarWeight);
            averageMolarWeight += (massFractionCH4 * ch4ResourceManager.molarWeight);
            return averageMolarWeight;
        }

        protected float calculateMolarMassFromMassFraction(float molarWeight, float massFraction)
        {
            return massFraction * (getSystemAverageMolarWeight() / molarWeight);
        }
    }
}