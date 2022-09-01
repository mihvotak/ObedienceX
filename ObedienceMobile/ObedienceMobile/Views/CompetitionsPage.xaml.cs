using Newtonsoft.Json;
using ObedienceX.Data;
using ObedienceX.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompetitionsPage : ContentPage
	{
		private PermissionService _permissionService;
		private Label _pathLabel;

		public CompetitionsPage()
		{
			InitializeComponent();
			_permissionService = new PermissionService();
			_pathLabel = (Label)FindByName("CurrentPath");
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			UpdateList();
		}

		private async Task<bool> CheckPermissions()
		{
			var status = await _permissionService.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
			if (status != PermissionStatus.Granted)
			{
				await App.Current.MainPage.DisplayAlert("Error", "You do not have writing permissions", "Ok");
				return false;
			}
			return true;
		}

		async private void UpdateList()
		{
			_pathLabel.Text = App.FolderPath;
			if (await CheckPermissions())
			{
				try
				{
					var files = Directory.EnumerateFileSystemEntries(App.FolderPath);

					var competitionFiles = new List<CompetitionFile>();

					foreach (var filename in files)
					{
						var file = new CompetitionFile
						{
							FileName = filename
						};
						if (Directory.Exists(filename))
						{
							file.Date = File.GetCreationTime(filename);
							file.ShortName = Path.GetFileName(filename);
						}
						else if (File.Exists(filename))
						{
							file.IsFile = true;
							file.ShortName = Path.GetFileName(filename);
							file.Date = File.GetCreationTime(filename);
						}
						competitionFiles.Add(file);
					}

					collectionView.ItemsSource = competitionFiles
						.OrderBy(d => d.Date)
						.ToList();
				}
				catch (Exception e)
				{

				}
			}
		}

		async void OnAddClicked(object sender, EventArgs e)
		{
			Model.Competition = new Competition();
			// Navigate to the CompetitionPage, without passing any data.
			await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
		}

		void OnGoHomeClick(object sender, EventArgs e)
		{
			App.FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			UpdateList();
		}

		void OnGoRootClick(object sender, EventArgs e)
		{
			App.FolderPath = "/storage/emulated/0/";
			UpdateList();
		}

		void OnGoUpClick(object sender, EventArgs e)
		{
			App.FolderPath = Directory.GetParent(App.FolderPath).FullName;
			UpdateList();
		}

		async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				// Navigate to the CompetitionPage, passing the filename as a query parameter.
				CompetitionFile file = (CompetitionFile)e.CurrentSelection.FirstOrDefault();
				if (Directory.Exists(file.FileName))
				{
					App.FolderPath = file.FileName;
					UpdateList();
				}
				else if (file.FileName.Length > 4 && file.FileName.Substring(file.FileName.Length - 4, 4) == ".txt" && File.Exists(file.FileName))
				{
					LoadCompetition(file.FileName);
					//await Shell.Current.GoToAsync("//Competition");
					//((CompetitionPage)(Shell.Current.CurrentPage)).FileName = competition.FileName;
					await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
				}
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