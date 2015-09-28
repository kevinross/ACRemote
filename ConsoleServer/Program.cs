using System;
using ACRemote;
using AustinHarris.JsonRpc;
using System.Net;
using SimpleWebServer;
using ACRemote.API;

namespace ConsoleServer {
	class MainClass {
		public static IDHT11 global_temp;
		public static IPlatformGPIO global_gpio;
		public static IRemoteGpioSettings global_gpio_settings;
		public static Remote global_remote;
		public static object service = null;

		public static void setup() {
			global_temp = KernelInstance.Resolve<IDHT11>();
			global_gpio = KernelInstance.Resolve<IPlatformGPIO>();
			global_gpio_settings = KernelInstance.Resolve<IRemoteGpioSettings>();
			global_remote = new Remote(global_gpio_settings, global_temp, global_gpio);
			service = new RemoteServiceServer(global_remote, global_temp);
		}
		public static void Main(string[] args) {
			setup();
			WebServer ws = new WebServer(new string[]{"http://*:8080/api/", "http://*:8080/remote/"}, SendResponse);
			ws.Run();
			Console.ReadKey();
			ws.Stop();
		}
		private static string SendResponse(HttpListenerRequest req) {
			string data = GetRequestPostData(req);
			if (data != null && data.Contains("json")) {
				System.Threading.Tasks.Task<string> tsk = JsonRpcProcessor.Process(GetRequestPostData(req), (object)null);
				return tsk.Result;
			} else {
				string lastelem = req.Url.Segments[req.Url.Segments.Length - 1];
				switch (req.HttpMethod) {
				case "GET":
					return PropertyUtil.get_prop(global_remote, lastelem).ToString();
				case "POST":
					PropertyUtil.set_prop(global_remote, lastelem, data.Trim());
					return PropertyUtil.get_prop(global_remote, lastelem).ToString();
				}
				return "";
			}
		}
		private static string GetRequestPostData(HttpListenerRequest request)
		{
			if (!request.HasEntityBody)
			{
				return null;
			}
			using (System.IO.Stream body = request.InputStream) // here we have data
			{
				
				using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
