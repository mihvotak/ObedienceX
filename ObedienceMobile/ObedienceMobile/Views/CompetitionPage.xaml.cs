using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

namespace ObedienceX.Views
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

		void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
		}

		async void OnSaveAsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(SaveAsPage)}");
		}

		async void OnExaminationsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(ExaminationsPage)}");
		}

		async void OnPairsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(PairsPage)}");
		}

		async void OnResultsClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"//{nameof(ResultsPage)}");
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
