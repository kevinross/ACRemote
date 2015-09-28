using System;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ACRemote.API {
	public class JsonRpcClient {
		private Uri endpoint = null;
		private int id = 1;
		public JsonRpcClient(Uri endpoint) {
			this.endpoint = endpoint;
		}
		public void InvokeMethod(string method, params object[] p) {
			this.InvokeMethod_(method, p);
		}
		public T InvokeMethod<T>(string method, params object[] p) {
			return (T)this.InvokeMethod_(method, p).SelectToken("result").ToObject<T>();
		}
		public JObject InvokeMethod_(string a_sMethod, params object[] a_params)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(endpoint);

			webRequest.ContentType = "application/json-rpc";
			webRequest.Method = "POST";

			JObject joe = new JObject();
			joe["jsonrpc"] = "1.0";
			joe["id"] = (id++).ToString();
			joe["method"] = a_sMethod;

			if (a_params != null) {
				if (a_params.Length > 0) {
					JArray props = new JArray();
					foreach (var p in a_params) {
						props.Add(p);
					}
					joe.Add(new JProperty("params", props));
				} else {
					joe.Add(new JProperty("params", new JArray()));
				}
			} 

			string s = JsonConvert.SerializeObject(joe);
			// serialize json for the request
			byte[] byteArray = Encoding.UTF8.GetBytes(s);
			webRequest.ContentLength = byteArray.Length;

			try
			{
				using (Stream dataStream = webRequest.GetRequestStream())
				{
					dataStream.Write(byteArray, 0, byteArray.Length);
				}
			}
			catch (WebException)
			{
				//inner exception is socket
				//{"A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 23.23.246.5:8332"}
				throw;
			}
			WebResponse webResponse = null;
			try
			{
				using (webResponse = webRequest.GetResponse())
				{
					using (Stream str = webResponse.GetResponseStream())
					{
						using (StreamReader sr = new StreamReader(str))
						{
							return JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
						}
					}
				}
			}
			catch (WebException webex)
			{

				using (Stream str = webex.Response.GetResponseStream())
				{
					using (StreamReader sr = new StreamReader(str))
					{
						var tempRet =  JsonConvert.DeserializeObject<JObject>(sr.ReadToEnd());
						return tempRet;
					}
				}

			} 
			catch (Exception)
			{

				throw;
			}
		}
	}
}

