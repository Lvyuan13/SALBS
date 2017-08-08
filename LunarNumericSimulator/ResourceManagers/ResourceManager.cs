using System.Collections;

namespace LunarNumericSimulator {
	public abstract class ResourceManager<T> {
		public abstract void addResource(T resource);
		public abstract void consumeResource(T resource);
		public abstract T getLevel();
	}
}