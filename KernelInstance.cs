using System;
using System.Collections;
using System.Collections.Generic;
using TinyIoC;

namespace ACRemote {
	public class KernelInstance {
		private static TinyIoCContainer kern;
		private KernelInstance () {
		}
		public static TinyIoCContainer kernel() {
			return kern;
		}
		public static T Resolve<T>() {
			if (kern == null) {
				CreateKernel();
			}

			return (T)kern.Resolve(typeof(T));
		}
		public static Type[] FindImplementations() {
			List<Type> impls = new List<Type>();
			Array.ForEach(AppDomainMethods.GetLocalAssemblies(), x => {
				Array.ForEach(x.GetTypes(), cls => {
					if (typeof(ILoadable).IsAssignableFrom(cls) && !cls.IsAbstract) {
						impls.Add(cls);
					}
				});
			});
			return new List<Type>(new HashSet<Type>(new List<Type>(impls))).ToArray();
		}
		public static void RegisterImplementations() {
			Array.ForEach(FindImplementations(), cls => {
				ILoadable load = (ILoadable)Activator.CreateInstance(cls);
				if (load.Enable()) {
					Console.WriteLine("Loading " + cls.Name + "...");
					load.Load(kern);
				}
			});
		}
		private static void CreateKernel() {
			kern = TinyIoC.TinyIoCContainer.Current;
			RegisterImplementations();
		}
	}
}

