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
			String endpoint = args[0];
			String method = args[1];
			String action = args[2];
			String param = null;
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
