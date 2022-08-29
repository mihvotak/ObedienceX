using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ObedienceMobile.Models;
using ObedienceX.Data;
using ObedienceX.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;
using Json = Newtonsoft.Json;

namespace ObedienceMobile.Views
{
	public partial class CompetitionPage : ContentPage
	{

		public CompetitionPage()
		{
			InitializeComponent();

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			BindingContext = Model.Competition;
		}

		async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			var competition = Model.Competition;
			var jsonText = JsonConvert.SerializeObject(competition);
			if (string.IsNullOrWhiteSpace(competition.FileName))
			{
				// Save the file.
				var filename = Path.Combine(App.FolderPath, $"{competition.Name}.txt");
				competition.FileName = filename;
				File.WriteAllText(filename, jsonText);
			}
			else
			{
				// Update the file.
				File.WriteAllText(competition.FileName, jsonText);
			}

			// Navigate backwards
			await Shell.Current.GoToAsync("..");
		}

		async void OnExaminationsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(ExaminationsPage)}");
		}

		async void OnPairsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(ExaminationsPage)}");
		}

		async void OnDeleteButtonClicked(object sender, EventArgs e)
		{
			var competition = (Competition)BindingContext;

			// Delete the file.
			if (File.Exists(competition.FileName))
			{
				File.Delete(competition.FileName);
			}

			// Navigate backwards
			await Shell.Current.GoToAsync("..");
		}
	}
}
