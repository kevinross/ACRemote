﻿using System;
using System.IO;

namespace ACRemote {
	public class FileDHT11 : DHT11 {
		private String base_path = "/sys/bus/iio/devices/iio:device0/";
		private String temp_part = "in_temp_input";
		private String humid_part = "in_humidityrelative_input";
		protected override void update() {
			this.temp_ = get_part(temp_part);
			this.humid_ = get_part(humid_part);
		}
		private int get_part(String part) {
			bool passed = false;
			while (!passed) {
				try {
					String lines = File.ReadAllText(base_path + part);
					passed = true;
					return Int32.Parse(lines.Trim());
				} catch (IOException e) {
					System.Threading.Thread.Sleep(10);
				}
			}
			return 0;
		}
	}
}
