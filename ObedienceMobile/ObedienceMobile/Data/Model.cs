using System;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;
using ObedienceX.Views;
using ObedienceX.Utils;

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
				_competition = value;
			}
		}

		public static bool ChangeFolderMode;

		public static ExcelProxy ExcelProxy = new ExcelProxy();

		public static bool ReSaveCurrent()
		{
			var competition = Model.Competition;
			if (string.IsNullOrWhiteSpace(competition.ExcelName))
			{
				Shell.Current.GoToAsync($"{nameof(SaveAsPage)}");
			}
			else
			{
				string ext = Path.GetExtension(competition.ExcelName);
				if (ext == ".xlsx")
					return ExcelProxy.WriteExcel(competition.ExcelName);
				//else
				//	SaveCurrentAs(competition.ExcelName);
			}
			return false;
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
