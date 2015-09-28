using System;
using ACRemote;

namespace ACRemote.API {
	public interface IRemoteService {
		void set_power(bool val);
		bool get_power();
		void set_temp(int val);
		int get_temp();
		void set_mode(string mode);
		string get_mode();
		void set_speed(string speed);
		string get_speed();
		double actual_temp();
		double actual_humidity();
	}
}

