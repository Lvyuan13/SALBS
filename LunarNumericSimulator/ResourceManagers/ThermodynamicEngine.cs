using System;
using System.Collections;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;
using System.Linq;
using static LunarNumericSimulator.ResourceManagers.AtmosphericResourceManager;

namespace LunarNumericSimulator {
	public class Thermodynamic_ResourceManager : ResourceManager<float>
    {

		public AtmosphericResourceManager[] resourceManagers;

        public override Resources managedResource
        {
            get
            {
                return Resources.Enthalpy;
            }
        }

        public Thermodynamic_ResourceManager(ref AtmosphericResourceManager[] rms, Dictionary<string, float> config){
			resourceManagers = rms;

            float startO, startN, startCO2, initialTemp, initialPressure;
            float startCH4 = 0;
            try
            {
                config.TryGetValue("starting_Temp", out initialTemp);
                config.TryGetValue("atmospheric_CO2_start", out startCO2);
                config.TryGetValue("atmospheric_O_start", out startO);
                config.TryGetValue("atmospheric_N_start", out startN);
                config.TryGetValue("starting_Pressure", out initialPressure);
            }
            catch (Exception e)
            {
                throw e;
            }

            float averageMolarMass = calculateAverageMolarMass(ref rms, startCO2, startO, startN, startCH4);

            setupEnvironment(ref rms, startCO2, startO, startN, startCH4, initialPressure, initialTemp, averageMolarMass);
		}

        protected float calculateAverageMolarMass(ref AtmosphericResourceManager[] rms, float startCO2, float startO, float startN, float startCH4)
        {
            float averageMolarMass = 0;
            foreach (var rm in rms)
            {
                switch (rm.managedResource)
                {
                    case Resources.CO2:
                        averageMolarMass += (startCO2 / rm.molarWeight);
                        break;
                    case Resources.O:
                        averageMolarMass += (startO / rm.molarWeight);
                        break;
                    case Resources.N:
                        averageMolarMass += (startN / rm.molarWeight);
                        break;
                    case Resources.CH4:
                        averageMolarMass += (startCH4 / rm.molarWeight);
                        break;
                    default:
                        throw new Exception("Unexpected resource in the atmosphere");
                }
            }
            return averageMolarMass;
        }

        protected void setupEnvironment(ref AtmosphericResourceManager[] rms, float startCO2, float startO, float startN, float startCH4, float startingPressure, float startingTemp, float averageMolarMass)
        {
            float molarFraction = 0;
            float partialPressure = 0;
            EnthalpyAndSpecVolume thermoStateVars;
            for (int i = 0; i < rms.Length; i++)
            {
                switch (rms[i].managedResource)
                {
                    case Resources.CO2:
                        molarFraction = startCO2 * (averageMolarMass/rms[i].molarWeight);
                        partialPressure = molarFraction * startingPressure;
                        thermoStateVars = rms[i].getEnthalpyAndSpecVolumeAtPoint(startingTemp, partialPressure);
                        rms[i].setState(startingTemp, partialPressure, thermoStateVars.Enthalpy, thermoStateVars.SpecificVolume);
                        break;
                    case Resources.O:
                        molarFraction = startO * (averageMolarMass / rms[i].molarWeight);
                        partialPressure = molarFraction * startingPressure;
                        thermoStateVars = rms[i].getEnthalpyAndSpecVolumeAtPoint(startingTemp, partialPressure);
                        rms[i].setState(startingTemp, partialPressure, thermoStateVars.Enthalpy, thermoStateVars.SpecificVolume);
                        break;
                    case Resources.N:
                        molarFraction = startN * (averageMolarMass / rms[i].molarWeight);
                        partialPressure = molarFraction * startingPressure;
                        thermoStateVars = rms[i].getEnthalpyAndSpecVolumeAtPoint(startingTemp, partialPressure);
                        rms[i].setState(startingTemp, partialPressure, thermoStateVars.Enthalpy, thermoStateVars.SpecificVolume);
                        break;
                    case Resources.CH4:
                        molarFraction = startCH4 * (averageMolarMass / rms[i].molarWeight);
                        partialPressure = molarFraction * startingPressure;
                        thermoStateVars = rms[i].getEnthalpyAndSpecVolumeAtPoint(startingTemp, partialPressure);
                        rms[i].setState(startingTemp, partialPressure, thermoStateVars.Enthalpy, thermoStateVars.SpecificVolume);
                        break;
                    default:
                        throw new Exception("Unexpected resource in the atmosphere");
                }
            }
        }

        public void setEnvironmentState(float temp, float pressure) {
            float[] gasMasses = new float[resourceManagers.Length];
            for (int i = 0; i < resourceManagers.Length; i++)
            {
                gasMasses[i] = resourceManagers[i].getLevel();
            }

            float totalGasMass = 0;
            foreach (float mass in gasMasses)
                totalGasMass += mass;

            float[] molarFractions = new float[resourceManagers.Length];

            foreach (AtmosphericResourceManager rm in resourceManagers){
				var result = rm.getEnthalpyAndSpecVolumeAtPoint(temp, pressure);
                rm.setState(temp, pressure, result.Enthalpy, result.SpecificVolume);
			}
		}

        /* 
         * This function handles the addition of or removal of Enthalpy from atmospheric gas. The total enthalpy within the atmosphere is first 
         * computed, and then the mass fractions of individual gasses are calculated. The enthalpy to be added is split among the atmospheric gasses
         * proportionately to their mass fraction
         * 
         * For example, if Nitrogen constitutes 71% of the atmosphere, it will absorb 71% of the incoming enthalpy.
         * */
		public void addEnthalpy(float quantity) {
			float totalEnthalpyInSystem = 0;
			float[] gasMasses = new float[resourceManagers.Length];
			for(int i = 0; i < resourceManagers.Length; i++){
                totalEnthalpyInSystem += resourceManagers[i].getTotalEnthalpy();
				gasMasses[i] = resourceManagers[i].getLevel();
			}
			
			float totalGasMass = 0;
			foreach (float mass in gasMasses)
				totalGasMass += mass;
			
			for(int i = 0; i < resourceManagers.Length; i++){
				float massPortion = gasMasses[i]/totalGasMass;
                float totalEnthalpyInGas = massPortion * (totalEnthalpyInSystem + quantity);
                float enthalpyPerUnitMassInGas = totalEnthalpyInGas / resourceManagers[i].getLevel();
                float specificVolumeOfGas = resourceManagers[i].getSpecificVolume();

                var newState = resourceManagers[i].getTempAndPressureAtPoint(enthalpyPerUnitMassInGas, specificVolumeOfGas);
                resourceManagers[i].setState(newState.Temperature, newState.Pressure, enthalpyPerUnitMassInGas, specificVolumeOfGas);
            }
		}

		public void addAtmosphere(float quantity){
			
		}

        public override void addResource(float resource)
        {
            addEnthalpy(resource);
        }

        public override void consumeResource(float resource)
        {
            addEnthalpy(-resource);
        }

        public override float getLevel()
        {
            
        }
    }
}