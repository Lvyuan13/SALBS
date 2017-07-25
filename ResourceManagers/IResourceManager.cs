using UnityEngine;
using System.Collections;

public interface IResourceManager<T> {
	void addResource(T resource);
	void consumeResource(T resource);
	T getLevel();
}