using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ACRemote {
	class AppDomainMethods {
		
		public static string AssemblyDirectory {
			get {
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public static Assembly[] GetLocalAssemblies() {
			return GetAssemblyList().ToArray();
		}

		private static IEnumerable<Assembly> GetAssemblyList() {
			List<Assembly> assemblies = new List<Assembly>();
			foreach (String file in Directory.GetFiles(AssemblyDirectory)) {
				if (Path.GetExtension(file) == ".dll") {
					try {
						Assembly asm = Assembly.LoadFile(file);
						assemblies.Add(asm);
					} catch {
					}
				}
			}
			assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().AsEnumerable());

			return new List<Assembly>(new HashSet<Assembly>(assemblies));
		}
	}
}