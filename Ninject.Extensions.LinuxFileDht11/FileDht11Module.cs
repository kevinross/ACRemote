using System;
using ACRemote;
using TinyIoC;

namespace Injections.LinuxFileDht11 {
	public class FileDht11Module : ILoadable {
		public bool Enable() {
			Type dht22type = Type.GetType("Raspberry.IO.Components.Sensors.Temperature.Dht.Dht22Connection");
			return dht22type == null;
		}
		public void Load(TinyIoCContainer kernel) {
			kernel.Register<IDHT11, FileDHT11>().AsSingleton();
		}
		public Type ImplementingClass() {
			return typeof(FileDHT11);
		}
	}
}

