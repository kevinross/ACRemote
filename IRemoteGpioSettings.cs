using System;

namespace ACRemote {
	public interface IRemoteGpioSettings {
		int reset { get;}
		int power { get;}
		int temp_down { get;}
		int temp_up { get;}
		int mode { get;}
		int speed { get;}
	}
}

