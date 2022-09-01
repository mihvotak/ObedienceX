using ObedienceX.Views;
using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX
{
	public partial class App : Application
	{
		public static string FolderPath { get; set; }
		public static string FileExtention { get; private set; }

		public App()
		{
			var cultureInfo = new CultureInfo("ru-RU", false);
			cultureInfo.NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." };
			CultureInfo.CurrentCulture = cultureInfo;
			CultureInfo.CurrentUICulture = cultureInfo;

			InitializeComponent();

			FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			FileExtention = ".txt";

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
