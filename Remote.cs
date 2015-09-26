using System;

namespace ACRemote {
	public class Remote {
		private IDHT11 temp_sensor;
		private IPlatformGPIO gpio;
		private int rst, pwr, t_down, t_up, md, spd;
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
		private bool power_ = false;
		private int temp_ = 76;
		private modes mode_ = modes.ac;
		private speeds speed_ = speeds.low;
		public bool power {
			get {
				double tmp = this.temp_sensor.temp;
				if (tmp < 20 && !power_) {
					this.reset();
					this.press(pwr);
					this.go_to_mode(mode_);
					if (mode_ == modes.ac) {
						this.go_to_temp(temp_);
					}
					this.go_to_speed(speed_);
					power_ = true;
				}
				return power_;
			}
			set {
				this.go_to_power(value);
				this.power_ = value;
			}
		}
		public int temp {
			get {
				return this.temp_;
			}
			set {
				if (this.power) {
					this.go_to_temp(value);
				}
				this.temp_ = value;
			}
		}
		public modes mode {
			get {
				return this.mode_;
			}
			set {
				if (this.power) {
					this.go_to_mode(value);
				}
				this.mode_ = value;
			}
		}
		public speeds speed {
			get {
				return this.speed_;
			}
			set {
				if (this.power) {
					this.go_to_speed(value);
				}
				this.speed_ = value;
			}
		}
		public Remote(IRemoteGpioSettings settings, IDHT11 temp_sensor, IPlatformGPIO gpio) {
			this.gpio = gpio;
			this.temp_sensor = temp_sensor;
			this.rst = settings.reset;
			this.pwr = settings.power;
			this.t_down = settings.temp_down;
			this.t_up = settings.temp_up;
			this.md = settings.mode;
			this.spd = settings.speed;
		}
		public void setup() {
			foreach(int p in new []{rst, pwr, t_down, t_up, md, spd}) {
				this.gpio.setup(p, direction.out_);
			}
		}
		private void press(int pin) {
			this.gpio[pin] = true;
			System.Threading.Thread.Sleep(250);
			this.gpio[pin] = false;
		}
		private void press(int pin, int times) {
			for (int i = 0; i < times; i++) {
				press(pin);
			}
		}
		private void go_to_power(bool power) {
			if (this.power_ == power) {
				return;
			}
			press(pwr);
		}
		private void go_to_temp(int temp) {
			if (temp > this.temp) {
				press(t_down, temp - this.temp);
			} else if (temp < this.temp) {
				press(t_up, this.temp - temp);
			}
		}
		private void go_to_mode(modes mode) {
			if (this.mode == mode) {
				return;
			}

			// ac, dehumid, fan
			// currently ac, going to...
			if (this.mode == modes.ac) {
				if (mode == modes.dehumid) {
					press(md, 1);
				} else if (mode == modes.fan) {
					press(md, 2);
				}
			} else if (this.mode == modes.dehumid) {
				if (mode == modes.fan) {
					press(md, 1);
				} else if (mode == modes.ac) {
					press(md, 2);
				}
			} else if (this.mode == modes.fan) {
				if (mode == modes.ac) {
					press(md, 1);
				} else if (mode == modes.dehumid) {
					press(md, 2);
				}
			}
		}

		private void go_to_speed(speeds speed) {
			if (speed == this.speed) {
				return;
			}
			// high, medium, low

			// currently high, going to...
			if (this.speed == speeds.high) {
				if (speed == speeds.medium) {
					press(spd, 1);
				} else if (speed == speeds.low) {
					press(spd, 2);
				}
			} else if (this.speed == speeds.medium) {
				if (speed == speeds.low) {
					press(spd, 1);
				} else if (speed == speeds.high) {
					press(spd, 2);
				}
			} else if (this.speed == speeds.low) {
				if (speed == speeds.high) {
					press(spd, 1);
				} else if (speed == speeds.medium) {
					press(spd, 2);
				}
			}
		}
		public void reset() {
			press(rst, 1);
			power_ = false;
			temp_ = 76;
			mode_ = modes.ac;
			speed_ = speeds.low;
		}
	}
}

