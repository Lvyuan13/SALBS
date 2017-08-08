using System;
using System.Collections;
using System.Collections.Generic;
using LunarNumericSimulator.ResourceManagers;

namespace LunarNumericSimulator {
	public class Thermodynamic_ResourceManager : ResourceManager<float>
    {

		public AtmosphericResourceManager[] resourceManagers;

		public Thermodynamic_ResourceManager(int initialTemp, float initialPressure, ref AtmosphericResourceManager[] rms){
			resourceManagers = rms;

			setEnvironmentState(initialTemp, initialPressure);
		}

		public void setEnvironmentState(int temp, float pressure) {
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