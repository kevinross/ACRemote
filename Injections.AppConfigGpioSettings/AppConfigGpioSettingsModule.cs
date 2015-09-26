using System;
using System.Configuration;
using ACRemote;
using TinyIoC;

namespace Injections.AppConfigGpioSettings {
	public class AppConfigGpioSettingsModule : ILoadable {
		public bool Enable() {
			try {
				AppSettings.Get<int>("reset_pin");
				return true;
			} catch {
			}
			return false;
		}
		public void Load(TinyIoCContainer kernel) {
			kernel.Register<IRemoteGpioSettings, AppConfigGpioSettings>().AsSingleton();
		}
		public Type ImplementingClass() {
			return typeof(AppConfigGpioSettings);
		}
	}
}

