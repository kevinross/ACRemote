using System;
using System.Web;
using AustinHarris.JsonRpc;

namespace ConsoleServer {
	public class RemoteService : JsonRpcService {
		public RemoteService () {
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
			ConsoleServer.MainClass.global_remote.power = (bool)val;
		}
		[JsonRpcMethod]
		public bool get_power() {
			return ConsoleServer.MainClass.global_remote.power;
		}
		[JsonRpcMethod]
		public void set_temp(int val) {
			ConsoleServer.MainClass.global_remote.temp = val;
		}
		[JsonRpcMethod]
		public int get_temp() {
			return ConsoleServer.MainClass.global_remote.temp;
		}
		[JsonRpcMethod]
		public String get_mode() {
			return ConsoleServer.MainClass.global_remote.mode.ToString();
		}
		[JsonRpcMethod]
		public void set_mode(String val) {
			ConsoleServer.MainClass.global_remote.mode = (ACRemote.Remote.modes)System.Enum.Parse(typeof(ACRemote.Remote.modes), val);
		}
		[JsonRpcMethod]
		public String get_speed() {
			return ConsoleServer.MainClass.global_remote.speed.ToString();
		}
		[JsonRpcMethod]
		public void set_speed(object val) {
			ConsoleServer.MainClass.global_remote.speed = (ACRemote.Remote.speeds)System.Enum.Parse(typeof(ACRemote.Remote.speeds), (String)val);
		}
		[JsonRpcMethod]
		public double actual_temp() {
			return ConsoleServer.MainClass.global_temp.temp;
		}
		[JsonRpcMethod]
		public double actual_humidity() {
			return ConsoleServer.MainClass.global_temp.humidity;
		}
	}
}

