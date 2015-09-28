using System;
using System.Web;
using AustinHarris.JsonRpc;
using ACRemote;

namespace ACRemote.API {
	public class RemoteServiceServer : JsonRpcService, IRemoteService {
		IRemote remote = null;
		IDHT11 temp = null;
		public RemoteServiceServer (IRemote rmt, IDHT11 temp) {
			this.remote = rmt;
			this.temp = temp;
		}
		[JsonRpcMethod]
		public string[] __interface__() {
			System.Collections.ArrayList methods = new System.Collections.ArrayList();
			foreach (string p in new string[]{"set", "get"}) {
				foreach (string m in new string[]{"power", "temp", "mode", "speed"}) {
					methods.Add(string.Format("%s_%s", p, m));
				}
			}
			methods.Add("actual_temp");
			methods.Add("actual_humidity");
			return (string[])methods.ToArray();
		}
		[JsonRpcMethod]
		public void set_power(bool val) {
			remote.power = (bool)val;
		}
		[JsonRpcMethod]
		public bool get_power() {
			return remote.power;
		}
		[JsonRpcMethod]
		public void set_temp(int val) {
			remote.temp = val;
		}
		[JsonRpcMethod]
		public int get_temp() {
			return remote.temp;
		}
		[JsonRpcMethod]
		public String get_mode() {
			return remote.mode.ToString();
		}
		[JsonRpcMethod]
		public void set_mode(string val) {
			remote.mode = (ACRemote.modes)System.Enum.Parse(typeof(ACRemote.modes), val);
		}
		[JsonRpcMethod]
		public String get_speed() {
			return remote.speed.ToString();
		}
		[JsonRpcMethod]
		public void set_speed(string val) {
			remote.speed = (ACRemote.speeds)System.Enum.Parse(typeof(ACRemote.speeds), (String)val);
		}
		[JsonRpcMethod]
		public double actual_temp() {
			return temp.temp;
		}
		[JsonRpcMethod]
		public double actual_humidity() {
			return temp.humidity;
		}
	}
}

