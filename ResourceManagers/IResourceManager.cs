using System.Collections;

namespace LunarParametricNumeric {
	public interface IResourceManager<T> {
		void addResource(T resource);
		void consumeResource(T resource);
		T getLevel();
	}
}