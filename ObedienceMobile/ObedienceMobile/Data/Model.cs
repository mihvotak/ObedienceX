using System;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;
using ObedienceX.Views;

namespace ObedienceX.Data
{
	internal class Model
	{
		private static Competition _competition;
		public static Competition Competition
		{
			get 
			{
				if (_competition == null)
					_competition = new Competition();
				return _competition;
			}
			set 
			{
				_competition = value;
			}
		}

		public static void ReSaveCurrent()
		{
			var competition = Model.Competition;
			if (string.IsNullOrWhiteSpace(competition.FileName))
			{
				Shell.Current.GoToAsync($"{nameof(SaveAsPage)}");
			}
			else
			{
				SaveCurrentAs(competition.FileName);
			}
		}

		public static bool SaveCurrentAs(string fileName)
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
		}
		public static string LastError;
	}
}
