using System;
using System.Configuration;
using System.ComponentModel;

namespace ACRemote {
	public class AppSettingNotFoundException : Exception {
		public AppSettingNotFoundException (string key) {
		}
	}

	public static class AppSettings {
		public static T Get<T>(string key) {
			var appSetting = ConfigurationManager.AppSettings[key];
			if (string.IsNullOrWhiteSpace(appSetting))
				throw new AppSettingNotFoundException(key);

			var converter = TypeDescriptor.GetConverter(typeof(T));
			return (T)(converter.ConvertFromInvariantString(appSetting));
		}
	}
}