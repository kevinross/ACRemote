using System;
using ACRemote;
using TinyIoC;

namespace Injections.LinuxPlatformGPIO {
	public class LinuxPlatformGPIOModule : ILoadable {
		public bool Enable() {
			if (Type.GetType("Mono.Runtime") != null) {
				Mono.Unix.Native.Utsname results;
				var res = Mono.Unix.Native.Syscall.uname(out results);
				if(res != 0)
				{
					throw new Exception("Syscall failed!");
				}
				return results.sysname == "Linux";
			}
			return false;
		}
		public void Load(TinyIoCContainer kernel) {
			kernel.Register<IPlatformGPIO, LinuxGPIO>().AsSingleton();
		}
		public Type ImplementingClass() {
			return typeof(LinuxGPIO);
		}
	}
}

