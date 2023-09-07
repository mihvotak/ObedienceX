using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompetitionsPage : ContentPage
	{
		private Label _pathLabel;
		private Label _errorLabel;
		private Button _okButton;
		private ScrollView _scrollView;

		public CompetitionsPage()
		{
			InitializeComponent();
			_pathLabel = (Label)FindByName("CurrentPath");
			_errorLabel = (Label)FindByName("Error");
			_okButton = (Button)FindByName("OkButton");
			_scrollView = (ScrollView)FindByName("MainScrollView");
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			UpdateList();
			Title = Model.ChangeFolderMode ? "Выберите папку" : "Выберите файл соревнований";
			_okButton.IsVisible = Model.ChangeFolderMode;
		}

		private void UpdateList()
		{
			_pathLabel.Text = App.FolderPath;
			_errorLabel.IsVisible = false;
			_errorLabel.Text = "";
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
						if (file.IsFolder || ((file.IsExcel || file.IsXls) && !Model.ChangeFolderMode))
							competitionFiles.Add(file);
					}

					collectionView.ItemsSource = competitionFiles
						.OrderBy(d => d.Date)
						.ToList();
					_scrollView.ScrollToAsync(0, 0, false);
				}
				catch (Exception )
				{

				}
			}
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
				/*else if (file.FileName.Length > 4 && file.FileName.Substring(file.FileName.Length - 4, 4) == ".txt" && File.Exists(file.FileName))
				{
					var competition = LoadCompetition(file.FileName);
					//await Shell.Current.GoToAsync("//Competition");
					//((CompetitionPage)(Shell.Current.CurrentPage)).FileName = competition.FileName;
					if (competition != null)
					{
						Model.Competition = competition;
						await Shell.Current.GoToAsync("..");
						await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
					}
				}*/
				else if (file.FileName.Length > 5 && Path.GetExtension(file.FileName) == ".xls" && File.Exists(file.FileName))
				{
					await Shell.Current.DisplayAlert("Не поддерживается", "Файлы с расширением 'xls' не поддерживаются. Только современные 'xlsx'.", "OK");
				}
				else if (file.FileName.Length > 5 && Path.GetExtension(file.FileName) == ".xlsx" && File.Exists(file.FileName))
				{
					Competition competition = Model.ExcelProxy.ReadExcel(file.FileName);
					if (competition != null)
					{
						Competition savedPrev = Model.Prev;
						Model.Competition = competition;
						if (Model.Prev!= null && !string.IsNullOrEmpty(Model.Prev.ExcelName) && !string.IsNullOrEmpty(Model.Competition.ExcelName) && Model.Prev.ExcelName == Model.Competition.ExcelName)
							Model.Prev = savedPrev;
						await Shell.Current.GoToAsync("..");
						await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
					}
					else
					{
						_errorLabel.Text = Model.ExcelProxy.LastError;
						_errorLabel.IsVisible = true;
						Model.ExcelProxy.LastError = null;
					}
				}
			}
		}

		private void OnOkClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync("..");
		}

		/*private Competition LoadCompetition(string filename)
		{
			try
			{
				// Retrieve the competition and set it as the BindingContext of the page.
				var text = File.ReadAllText(filename);
				Competition competition = JsonConvert.DeserializeObject<Competition>(text);
				if (competition != null)
				{
					competition.FileName = filename;
					return competition;
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to load competition.");
			}
			return null;
		}*/
	}
}