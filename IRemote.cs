using System;

namespace ACRemote {
	public enum modes {
		ac,
		dehumid,
		fan
	};
	public enum speeds {
		low,
		medium,
		high
	};
	public interface IRemote {
		bool power {get; set;}
		int temp {get; set;}
		modes mode {get; set;}
		speeds speed {get; set;}
	}
}

