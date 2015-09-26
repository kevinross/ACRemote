using System;
using ACRemote;
using AustinHarris.JsonRpc;
using System.Net;
using SimpleWebServer;

namespace ConsoleServer {
	class MainClass {
		public static IDHT11 global_temp;
		public static IPlatformGPIO global_gpio;
		public static IRemoteGpioSettings global_gpio_settings;
		public static Remote global_remote;
		static object[] services = new object[] {
			new RemoteService()

		};

		public static void setup() {
			global_temp = KernelInstance.Resolve<IDHT11>();
			global_gpio = KernelInstance.Resolve<IPlatformGPIO>();
			global_gpio_settings = KernelInstance.Resolve<IRemoteGpioSettings>();
			global_remote = new Remote(global_gpio_settings, global_temp, global_gpio);
		}
		public static void Main(string[] args) {
			setup();
			WebServer ws = new WebServer(new string[]{"http://localhost:8080/test/", "http://127.0.0.1:8080/test/"}, SendResponse);
			ws.Run();
			Console.ReadKey();
			ws.Stop();
		}
		private static string SendResponse(HttpListenerRequest req) {
			System.Threading.Tasks.Task<string> tsk = JsonRpcProcessor.Process(GetRequestPostData(req), (object)null);
			return tsk.Result;
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
