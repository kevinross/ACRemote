using System;
using ACRemote;
using TinyIoC;

namespace Injections.NullModules {
	public abstract class NullModules<I, T> : ILoadable where T : I, Defaultable {
		public bool Enable() {
			//typeof(T).Get
			//var result = typeof(T).GetMethod("non_default_exists").MakeGenericMethod(new Type[]{ typeof(I) }).Invoke(null, new object[]{typeof(T)});
			//return (bool)result;
			return !Defaultable.non_default_exists<I>(typeof(T));
		}
		public void Load(TinyIoCContainer kern) {
			kern.Register(typeof(I), typeof(T)).AsSingleton();
		}
		public Type ImplementingClass() {
			return typeof(T);
		}
	}
	public class NullDhtModule : NullModules<IDHT11, NullDHT11> {
		
	}
	public class NullGpioModule : NullModules<IPlatformGPIO, NullGPIO> {
		
	}
	public class NullGpioSettingsModule : NullModules<IRemoteGpioSettings, NullGPIOSettings> {
		
	}
}

