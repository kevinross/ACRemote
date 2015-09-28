using System;
using System.Threading;
using AustinHarris.JsonRpc;
using ACRemote;

namespace ACRemote.API {
	public class RemoteServiceClient {
		private JsonRpcClient client;
		private T invoke<T>(string method) {
			return client.InvokeMethod<T>(method);
		}
		private T invoke<T>(string method, object param) {
			return client.InvokeMethod<T>(method, param);
		}
		private void invoke(string method, object param) {
			client.InvokeMethod(method, param);
		}
		private T get_val<T>(string param) {
			return invoke<T>(String.Format("get_{0}", param));
		}
		private void set_val<T>(string param, T val) {
			invoke(String.Format("set_{0}", param), val);
		}

		public bool power {
			get {
				return get_val<bool>("power");
			}
			set {
				set_val<bool>("power", value);
			}
		}

		public int temp {
			get {
				return get_val<int>("temp");
			}
			set {
				set_val<int>("temp", value);
			}
		}
		public modes mode {
			get {
				return PropertyUtil.convert_str_to_enum<modes>(get_val<string>("mode"));
			}
			set {
				set_val<string>("mode", value.ToString());
			}
		}
		public speeds speed {
			get {
				return PropertyUtil.convert_str_to_enum<speeds>(get_val<string>("speed"));
			}
			set {
				set_val<string>("speed", value.ToString());
			}
		}
		public RemoteServiceClient (string endpoint) {
			client = new JsonRpcClient(new Uri(endpoint));
		}
	}
}

