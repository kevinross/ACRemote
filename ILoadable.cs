using System;
using TinyIoC;

namespace ACRemote {
	public interface ILoadable {
		bool Enable();
		Type ImplementingClass();
		void Load(TinyIoCContainer kernel);
	}
}

