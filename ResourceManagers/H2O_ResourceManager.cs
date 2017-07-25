using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2O_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalH2O; //[kg]

	public void addResource(float resource) {
		totalH2O += resource;
	}

	public void consumeResource(float resource) {
		totalH2O -= resource;
	}

	public float getLevel() {
		return totalH2O;
	}
}