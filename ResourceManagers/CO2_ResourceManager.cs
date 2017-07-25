using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalCO2; //[kg]

	public void addResource(float resource) {
		totalCO2 += resource;
	}

	public void consumeResource(float resource) {
		totalCO2 -= resource;
	}

	public float getLevel() {
		return totalCO2;
	}
}