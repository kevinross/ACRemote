using System;
using Gtk;

namespace piUI {
	class MainClass {
		public static void Main(string[] args) {
			Application.Init();
			MainWindow win = new MainWindow();
			win.Show();
			Application.Run();
		}
	}
}
