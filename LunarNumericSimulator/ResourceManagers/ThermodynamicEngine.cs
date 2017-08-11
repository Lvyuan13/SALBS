using System;
using System.Collections;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;
using System.Linq;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator
{
    public partial class Thermodynamic_ResourceManager : ResourceManager<float>
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

        public Thermodynamic_ResourceManager(CH4_ResourceManager CH4RMS, CO2_ResourceManager CO2RMS, O_ResourceManager ORMS, N_ResourceManager NRMS, Dictionary<string, float> config, float totalVolume)
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

        protected float getSystemAverageMolarWeight()
        {
            float systemMass = getTotalSystemMass();
            float massFractionCO2 = co2ResourceManager.getLevel() / systemMass;
            float massFractionCH4 = ch4ResourceManager.getLevel() / systemMass;
            float massFractionO = oResourceManager.getLevel() / systemMass;
            float massFractionN = nResourceManager.getLevel() / systemMass;

            float averageMolarWeight = (massFractionCO2 / co2ResourceManager.molarWeight);
            averageMolarWeight += (massFractionO / oResourceManager.molarWeight);
            averageMolarWeight += (massFractionN / nResourceManager.molarWeight);
            averageMolarWeight += (massFractionCH4 / ch4ResourceManager.molarWeight);
            return averageMolarWeight;
        }

        protected float calculateMolarMassFromMassFraction(float molarWeight, float massFraction)
        {
            return massFraction * (getSystemAverageMolarWeight() / molarWeight);
        }

        protected float getTotalSystemMass()
        {
            float totalMass = ch4ResourceManager.getLevel();
            totalMass += co2ResourceManager.getLevel();
            totalMass += oResourceManager.getLevel();
            totalMass += nResourceManager.getLevel();
            return totalMass;
        }

        protected void setupEnvironment(float startCO2, float startO, float startN, float startCH4, float startingPressure, float startingTemp)
        {
            float partialPressure = 0;
            ThermoState thermoStateVars;

            float molarFraction = startCO2;
            partialPressure = molarFraction * startingPressure;
            thermoStateVars = co2ResourceManager.getStateFromTempPres(startingTemp, partialPressure);
            co2ResourceManager.initiate(thermoStateVars, systemVolume);

            molarFraction = startCH4;
            partialPressure = molarFraction * startingPressure;
            thermoStateVars = ch4ResourceManager.getStateFromTempPres(startingTemp, partialPressure);
            ch4ResourceManager.initiate(thermoStateVars, systemVolume);

            molarFraction = startO;
            partialPressure = molarFraction * startingPressure;
            thermoStateVars = oResourceManager.getStateFromTempPres(startingTemp, partialPressure);
            oResourceManager.initiate(thermoStateVars, systemVolume);

            molarFraction = startN;
            partialPressure = molarFraction * startingPressure;
            thermoStateVars = nResourceManager.getStateFromTempPres(startingTemp, partialPressure);
            ch4ResourceManager.initiate(thermoStateVars, systemVolume);
        }

        /* 
  * This function is called whenever atmospheric resources are consumed or produced. It recalculates the properties all gasses in the atmosphere
  * and treating the process as isothermal and isochoric
  * */
        public void updateAtmosphere(float quantity)
        {
            float totalInternalEnergyInSystem = 0;
            float CO2, O, N, CH4, systemMass = getTotalSystemMass();
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

        /* 
         * This function handles the addition of or removal of heat from atmospheric gas. The total internal energy within the atmosphere is first 
         * computed, and then the mass fractions of individual gasses are calculated. The heat to be added is split among the atmospheric gasses
         * proportionately to their molar fraction
         * 
         * For example, if Nitrogen constitutes 71% of the atmosphere, it will absorb 71% of the incoming heat.
         * */
        public void addHeat(float quantity)
        {
            float totalInternalEnergyInSystem = 0;
            int numberOfResources = Enum.GetNames(typeof(Resources)).Length;
            float[] gasMasses = new float[numberOfResources];
            for (int i = 0; i < resourceManagers.Length; i++)
            {
                totalInternalEnergyInSystem += resourceManagers[i].getTotalInternalEnergy();
                gasMasses[(int)resourceManagers[i].managedResource] = resourceManagers[i].getLevel();
            }

            float totalGasMass = (from element in gasMasses
                                  select element).Sum();

            float[] massFractions = new float[numberOfResources];

            for (int i = 0; i < resourceManagers.Length; i++)
            {
                massFractions[i] = gasMasses[i] / totalGasMass;
            }

            float CO2, O, N, CH4;
            CO2 = massFractions[(int)Resources.CO2];
            O = massFractions[(int)Resources.O];
            N = massFractions[(int)Resources.N];
            CH4 = massFractions[(int)Resources.CH4];

            float averageMolarWeight = calculateAverageMolarWeight(CO2, O, N, CH4);

            float molarFraction = 0;
            float internalener = 0;
            ThermoState thermoStateVars;
            for (int i = 0; i < resourceManagers.Length; i++)
            {
                float density = resourceManagers[i].getDensity(); // This is an isochoric heat addition (density is constant)
                switch (resourceManagers[i].managedResource)
                {
                    case Resources.CO2:
                        molarFraction = CO2 * (averageMolarWeight / resourceManagers[i].molarWeight);
                        break;
                    case Resources.O:
                        molarFraction = O * (averageMolarWeight / resourceManagers[i].molarWeight);
                        break;
                    case Resources.N:
                        molarFraction = N * (averageMolarWeight / resourceManagers[i].molarWeight);
                        break;
                    case Resources.CH4:
                        molarFraction = CH4 * (averageMolarWeight / resourceManagers[i].molarWeight);
                        break;
                    default:
                        throw new Exception("Unexpected resource in the atmosphere");
                }

                internalener = molarFraction * (totalInternalEnergyInSystem + quantity);
                thermoStateVars = resourceManagers[i].getStateFromInternDensity(internalener, density);
                resourceManagers[i].setState(thermoStateVars, systemVolume);
            }

        }

        public void addAtmosphere(float quantity)
        {

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
            float result = 0;
            foreach (var rm in resourceManagers)
                result += rm.getTotalEnthalpy();
            return result;
        }
    }
}