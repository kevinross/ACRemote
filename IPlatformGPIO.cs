using System;

namespace ACRemote {
	public enum direction {
		in_,
		out_
	}
	public interface IPlatformGPIO {
		void setup(int pin, direction dir);
		bool this[int val] {
			get;
			set;
		}
	}
}