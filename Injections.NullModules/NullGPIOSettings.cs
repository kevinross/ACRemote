using System;
using TinyIoC;
using ACRemote;

namespace Injections.NullModules {
	public class NullGPIOSettings : Defaultable, IRemoteGpioSettings {
		public int reset { get { return 20; }}
		public int power { get { return 24; }}
		public int temp_down { get { return 23; }}
		public int temp_up { get { return 22; }}
		public int mode { get { return 10; }}
		public int speed { get { return 9; }}
		public NullGPIOSettings () {
		}
		public static void register(TinyIoCContainer kern) {
			if (!non_default_exists<IRemoteGpioSettings>(typeof(NullGPIOSettings))) {
				kern.Register<IRemoteGpioSettings, NullGPIOSettings>().AsSingleton();
			}
		}
	}
}

