using System;
using System.Reflection;
using ACRemote.API;

namespace CliClient {
	class MainClass {
		public static void Main(string[] args) {
			if (args.Length < 3) {
				Console.WriteLine("Usage: CliClient.exe host prop_name get|set [value] ");
				//return;
			}
			String endpoint = "http://localhost:8080/api/";//args[0];
			String method = "power";//args[1];
			String action = "set";//args[2];
			String param = "False";//null;
			if (args.Length > 3) {
				param = args[3];
			}
			RemoteServiceClient client = new RemoteServiceClient(endpoint);
			if (action == "get") {
				Console.WriteLine(PropertyUtil.get_prop(client, method));
			} else {
				PropertyUtil.set_prop(client, method, param);
				Console.WriteLine(PropertyUtil.get_prop(client, method));
			}
		}
	}
}
