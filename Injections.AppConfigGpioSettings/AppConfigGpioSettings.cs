using System;
using System.Configuration;
using ACRemote;

namespace Injections.AppConfigGpioSettings {
	public class AppConfigGpioSettings : Defaultable, IRemoteGpioSettings {
		public int reset {
			get {
				return AppSettings.Get<int>("reset_pin");
			}
		}
		public int power { 
			get {
				return AppSettings.Get<int>("power_pin");
			}
		}
		public int temp_down { 
			get {
				return AppSettings.Get<int>("temp_down_pin");
			}
		}
		public int temp_up { 
			get {
				return AppSettings.Get<int>("temp_up_pin");
			}
		}
		public int mode { 
			get {
				return AppSettings.Get<int>("mode_pin");
			}
		}
		public int speed { 
			get {
				return AppSettings.Get<int>("speed_pin");
			}
		}
		public AppConfigGpioSettings () {
			
		}
	}
}

