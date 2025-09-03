using ObedienceX.Views;
using System;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ObedienceX
{
	public partial class App : Application
	{
		public static string FolderPath { get; set; }
		public static string FileExtention { get; private set; }
		public static string ExcelTemplate { get; private set; }

		public App()
		{
			var cultureInfo = new CultureInfo("ru-RU", false);
			cultureInfo.NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." };
			CultureInfo.CurrentCulture = cultureInfo;
			CultureInfo.CurrentUICulture = cultureInfo;

			InitializeComponent();

			FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			FileExtention = ".xlsx";

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
