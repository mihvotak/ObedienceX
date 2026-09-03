using ObedienceX.Views;
using System;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace ObedienceX
{
	public partial class App : Application
	{
		private const string LastExcelFolderKey = "LastExcelFolder";

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

			string defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string savedFolder = Preferences.Default.Get(LastExcelFolderKey, defaultFolder);
			FolderPath = Directory.Exists(savedFolder) ? savedFolder : defaultFolder;
			FileExtention = ".xlsx";

			MainPage = new AppShell();
		}

		public static void RememberExcelFolder(string fileName)
		{
			string folder = Path.GetDirectoryName(fileName);
			if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
			{
				FolderPath = folder;
				Preferences.Default.Set(LastExcelFolderKey, folder);
			}
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
