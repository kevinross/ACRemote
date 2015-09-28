using System;
using System.Reflection;

namespace ACRemote.API {
	public class PropertyUtil {
		public static object get_prop(object client, string prop) {
			Type thisType = client.GetType();
			PropertyInfo theProp = thisType.GetProperty(prop);
			return theProp.GetValue(client);
		}
		public static void set_prop(object client, string prop, string param) {
			Type thisType = client.GetType();
			PropertyInfo theProp = thisType.GetProperty(prop);
			if (theProp.PropertyType == typeof(string)) {
				theProp.SetValue(client, param);
			} else if (theProp.PropertyType == typeof(bool)) {
				theProp.SetValue(client, Boolean.Parse(param));
			} else if (theProp.PropertyType == typeof(int)) {
				theProp.SetValue(client, int.Parse(param));
			} else if (theProp.PropertyType == typeof(ACRemote.modes)) {
				theProp.SetValue(client, convert_str_to_enum<ACRemote.modes>(param));
			} else if (theProp.PropertyType == typeof(ACRemote.speeds)) {
				theProp.SetValue(client, convert_str_to_enum<ACRemote.speeds>(param));
			} else {
				Console.WriteLine("type is " + theProp.PropertyType.ToString());
			}
		}
		public static T convert_str_to_enum<T>(string val) {
			return (T)System.Enum.Parse(typeof(T), val);
		}
	}
}

