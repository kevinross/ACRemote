using System;
using TinyIoC;
using ACRemote;

namespace Injections.NullModules {
	public class NullDHT11 : Defaultable, IDHT11 {
		public static void register(TinyIoCContainer kern) {
			if (!non_default_exists<IDHT11>(typeof(NullDHT11))) {
				kern.Register<IDHT11, NullDHT11>().AsSingleton();
			}
		}
		public NullDHT11() {
			
		}
		public double temp {
			get {
				return 25;
			}
		}
		public double humidity {
			get {
				return 80;
			}
		}
	}
}

