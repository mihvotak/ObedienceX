using System;
using Newtonsoft.Json;
using System.IO;
using ObedienceX.Views;
using ObedienceX.Utils;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ObedienceX.Data
{
	internal class Model
	{
		private static Competition _competition;
		public static Competition Competition
		{
			get 
			{
				return _competition;
			}
			set 
			{
				_prev = _competition;
				_competition = value;
			}
		}

		private static Competition _prev;
		public static Competition Prev
		{
			get
			{
				return _prev;
			}
			set
			{
				_prev = value;
			}
		}

		public static void Exchange()
		{
			if (_prev != null)
			{
				Competition current = _competition;
				_competition = _prev;
				_prev = current;
			}
		}

		public static bool ChangeFolderMode;

		public static ExcelProxy ExcelProxy = new ExcelProxy();

		public static async void ReSaveCurrent()
		{
			var competition = Model.Competition;
			if (string.IsNullOrWhiteSpace(competition.ExcelName))
			{
				var checker = DependencyService.Get<IPermissionChecker>();
				if (checker.CheckAllFilesPermission())
					await Shell.Current.GoToAsync($"{nameof(SaveAsPage)}");
				else
				{
					if (await Shell.Current.DisplayAlert(checker.GetConfirmCaption(), checker.GetConfirmSaveText(), checker.GetAgreeButton(), checker.GetCancelButton()))
						checker.ShowSettingsAllFilesPermission();
				}
			}
			else
			{
				string ext = Path.GetExtension(competition.ExcelName);
				if (ext == ".xlsx")
					ExcelProxy.WriteExcel(competition.ExcelName);
				//else
				//	SaveCurrentAs(competition.ExcelName);
			}
		}

		/*public static bool SaveCurrentAs(string fileName)
		{
			var competition = Model.Competition;
			try
			{
				var jsonText = JsonConvert.SerializeObject(competition);
				File.WriteAllText(fileName, jsonText);
				competition.FileName = fileName;
				return true;
			}
			catch (Exception e)
			{
				LastError = e.Message;
				return false;
			}
		}*/
	}
}
