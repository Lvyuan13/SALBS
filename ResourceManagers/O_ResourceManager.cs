using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_ResourceManager : MonoBehaviour, IResourceManager<float> {
	private float totalO; //[kg]

	public void addResource(float resource) {
		totalO += resource;
	}

	public void consumeResource(float resource) {
		totalO -= resource;
	}

	public float getLevel() {
		return totalO;
	}
}