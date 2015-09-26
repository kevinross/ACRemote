using System;
using System.Linq;

namespace ACRemote {
	public abstract class Defaultable {
		public static bool non_default_exists(Type type, Type default_type) {
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => type.IsAssignableFrom(p))
				.Where(p => p != type)
				.Where(p => !p.IsAbstract)
				.Where(p => p != default_type);
			bool has_types = types.Count() > 0;
			if (has_types) {
				Type t = types.First<Type>();
				bool done = false;
				Array.ForEach(KernelInstance.FindImplementations(), cls => {
					if (done) {
						return;
					}
					ILoadable load = (ILoadable)Activator.CreateInstance(cls);
					if (load.ImplementingClass() == t && !load.Enable()) {
						has_types = false;
						done = true;
					}
				});
			}
			return has_types;
		}
		public static bool non_default_exists<T>(Type default_type) {
			return non_default_exists(typeof(T), default_type);
		}
	}
}

