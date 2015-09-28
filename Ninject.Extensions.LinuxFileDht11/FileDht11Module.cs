using System;
using System.IO;
using ACRemote;
using TinyIoC;

namespace Injections.LinuxFileDht11 {
	public class FileDht11Module : ILoadable {
		public bool Enable() {
			Type dht22type = Type.GetType("Raspberry.IO.Components.Sensors.Temperature.Dht.Dht22Connection");
			bool fallback = File.Exists(FileDHT11.base_path + Path.DirectorySeparatorChar + FileDHT11.temp_part);
			return dht22type != null || fallback;
		}
		public void Load(TinyIoCContainer kernel) {
			kernel.Register<IDHT11, FileDHT11>().AsSingleton();
		}
		public Type ImplementingClass() {
			return typeof(FileDHT11);
		}
	}
}

