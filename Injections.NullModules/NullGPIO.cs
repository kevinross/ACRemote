using System;
using TinyIoC;
using ACRemote;

namespace Injections.NullModules {
	public class NullGPIO : Defaultable, IPlatformGPIO {
		System.Collections.Generic.Dictionary<int, bool> pins = new System.Collections.Generic.Dictionary<int, bool>();
		public static void register(TinyIoCContainer kern) {
			if (!non_default_exists<IPlatformGPIO>(typeof(NullGPIO))) {
				kern.Register<IPlatformGPIO, NullGPIO>().AsSingleton();
			}
		}
		public void setup(int pin, direction dir) {
			pins[pin] = false;
		}
		public bool this[int val] {
			get {
				return pins[val];
			}
			set {
				pins[val] = value;
			}
		}
	}
}

