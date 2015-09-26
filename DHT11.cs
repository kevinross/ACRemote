using System;
using System.IO;

namespace ACRemote {
	public abstract class DHT11 : Defaultable, IDHT11 {
		protected int temp_;
		protected int humid_;
		private DateTime last_check;
		public double temp {
			get {
				wait_until_sample_possible();
				update();
				return this.temp_ / 1000.0; // value is 1000*actual
			}
		}
		public double humidity {
			get {
				wait_until_sample_possible();
				update();
				return this.humid_ / 1000.0; // same as temp
			}
		}
		public DHT11 () {
			last_check = DateTime.Now;
		}
		private void wait_until_sample_possible() {
			// less than 100 milliseconds isn't really worth waiting again
			if ((DateTime.Now - last_check) < (new TimeSpan(100*10000))) {
				return;
			}
			while ((DateTime.Now - last_check) < (new TimeSpan(1000*10000))) {
				System.Threading.Thread.Sleep(100);
			}
			last_check = DateTime.Now;
		}
		protected abstract void update();
	}
}

