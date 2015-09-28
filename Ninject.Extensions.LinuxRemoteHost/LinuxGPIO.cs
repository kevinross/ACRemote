using System;
using Raspberry.IO.GeneralPurpose;

namespace ACRemote {
	public class LinuxGPIO : Defaultable, IPlatformGPIO {
		private GpioConnection conn;
		public LinuxGPIO() {
			GpioConnectionSettings settings = new GpioConnectionSettings{Driver = GpioConnectionSettings.DefaultDriver};
			this.conn = new GpioConnection(settings, new PinConfiguration[]{});
		}
		public void setup(int pin, direction dir) {
			ProcessorPin p = (ProcessorPin)System.Enum.Parse(typeof(ProcessorPin), String.Format("Pin{0}", pin));
			switch (dir) {
			case direction.in_:
				this.conn.Add(new InputPinConfiguration(p));
				break;
			case direction.out_:
				this.conn.Add(new OutputPinConfiguration(p));
				break;
			}
		}
		public bool this[int pin] {
			get {
				ProcessorPin p = (ProcessorPin)System.Enum.Parse(typeof(ProcessorPin), String.Format("Pin{0}", pin));
				return this.conn[p];
			}
			set {
				ProcessorPin p = (ProcessorPin)System.Enum.Parse(typeof(ProcessorPin), String.Format("Pin{0}", pin));
				this.conn[p] = value;
			}
		}
	}
}

