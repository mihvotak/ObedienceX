using Newtonsoft.Json;
using ObedienceX.ViewModels;
using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompetitionsPage : ContentPage
	{
		public CompetitionsPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var competitionFiles = new List<CompetitionFile>();

			// Create a Competition object from each file.
			var files = Directory.EnumerateFiles(App.FolderPath, "*.txt");
			foreach (var filename in files)
			{
				competitionFiles.Add(new CompetitionFile
				{
					FileName = filename,
					Date = File.GetCreationTime(filename)
				});
			}

			// Set the data source for the CollectionView to a
			// sorted collection of competitions.
			collectionView.ItemsSource = competitionFiles
				.OrderBy(d => d.Date)
				.ToList();
		}

		async void OnAddClicked(object sender, EventArgs e)
		{
			Model.Competition = new Competition();
			// Navigate to the CompetitionPage, without passing any data.
			await Shell.Current.GoToAsync(nameof(CompetitionPage));
		}

		async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				// Navigate to the CompetitionPage, passing the filename as a query parameter.
				CompetitionFile competitionFile = (CompetitionFile)e.CurrentSelection.FirstOrDefault();
				LoadCompetition(competitionFile.FileName);
				//await Shell.Current.GoToAsync("//Competition");
				//((CompetitionPage)(Shell.Current.CurrentPage)).FileName = competition.FileName;
				await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
			}
		}

		private void LoadCompetition(string filename)
		{
			try
			{
				// Retrieve the competition and set it as the BindingContext of the page.
				var text = File.ReadAllText(filename);
				Competition competition = JsonConvert.DeserializeObject<Competition>(text);
				if (competition != null)
				{
					competition.FileName = filename;
					Model.Competition = competition;
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to load competition.");
			}
		}
	}
}