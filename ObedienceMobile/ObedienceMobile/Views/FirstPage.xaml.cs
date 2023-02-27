using ObedienceX.Data;
using ObedienceX.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstPage : ContentPage
	{
		private PermissionService _permissionService;

		public FirstPage()
		{
			_permissionService = new PermissionService();
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ReloadPage();
		}

		private void ReloadPage()
		{
			Competition prev = Model.Prev;
			Label prevNameLabel = (Label)FindByName("PrevFileName");
			prevNameLabel.Text = prev == null ? "Предыдущий файл не выбран" :
				string.IsNullOrEmpty(prev.ExcelName) ? "Предыдущие соревнования не сохранены в файл" :
				$"Предыдущий файл:\n{prev.Name}";
			Label prevFolderLabel = (Label)FindByName("PrevFolder");
			prevFolderLabel.Text = prev == null ? "-" :
				string.IsNullOrEmpty(prev.ExcelName) ? "-" :
				$"Папка:\n{Path.GetDirectoryName(prev.ExcelName)}";

			Competition competition = Model.Competition;
			Label currentNameLabel = (Label)FindByName("CurrentFileName");
			currentNameLabel.Text = competition == null ? "Текущий файл не выбран" :
				string.IsNullOrEmpty(competition.ExcelName) ? "Текущие соревнования не сохранены в файл" :
				$"Текущий файл:\n{competition.Name}";
			Label currentFolderLabel = (Label)FindByName("CurrentFolder");
			currentFolderLabel.Text = competition == null ? "-" :
				string.IsNullOrEmpty(competition.ExcelName) ? "-" :
				$"Папка:\n{Path.GetDirectoryName(competition.ExcelName)}";

			ToolbarItems[0].BindingContext = competition;
			ToolbarItems[1].BindingContext = competition;

			Button saveAsButton = (Button)FindByName("SaveAsButton");
			saveAsButton.IsVisible = competition != null;
			Button deleteButton = (Button)FindByName("DeleteButton");
			deleteButton.IsVisible = competition != null;
		}

		private async Task<bool> CheckPermissions()
		{
			/*var status = await _permissionService.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
			if (status != PermissionStatus.Granted)
			{
				await App.Current.MainPage.DisplayAlert("Error", "You do not have writing permissions", "Ok");
				return false;
			}
			else*/
			{
				var checker = DependencyService.Get<IPermissionChecker>();
				if (!checker.CheckAllFilesPermission())
				{
					if (await Shell.Current.DisplayAlert(checker.GetConfirmCaption(), checker.GetConfirmReadText(), checker.GetAgreeButton(), checker.GetCancelButton()))
						checker.ShowSettingsAllFilesPermission();
					return false;
				}
			}
			return true;
		}

		async void OnOpenClick(object sender, EventArgs args)
		{
			bool force = true;
			if (Model.Competition != null && !Model.Competition.Saved)
				force = await DisplayAlert("Файл не сохранён.", "Всё равно продолжить?", "Да", "Нет");
			if (force)
			{
				if (await CheckPermissions())
				{
					Model.ChangeFolderMode = false;
					await Shell.Current.GoToAsync($"{nameof(CompetitionsPage)}");
				}
			}
		}

		async void OnAddClicked(object sender, EventArgs e)
		{
			bool force = true;
			if (Model.Competition != null && !Model.Competition.Saved)
				force = await DisplayAlert("Файл не сохранён.", "Всё равно продолжить?", "Да", "Нет");
			if (force)
			{
				Model.Competition = new Competition();
				// Navigate to the CompetitionPage, without passing any data.
				await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
			}
		}

		void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
		}

		async void OnExchangeClicked(object sender, EventArgs e)
		{
			if (Model.Prev != null)
			{
				bool force = true;
				if (Model.Competition != null && !Model.Competition.Saved)
					force = await DisplayAlert("Файл не сохранён.", "Всё равно продолжить?", "Да", "Нет");
				if (force)
				{
					Model.Exchange();
					ReloadPage();
				}
			}
		}

		async void OnDeleteButtonClicked(object sender, EventArgs e)
		{
			if (await Shell.Current.DisplayAlert("Удаление", "Текущий файл соревнований будет удален насовсем. Вы уверены?", "Удалить", "Отменить"))
			{
				var competition = Model.Competition;

				// Delete the file.
				//if (File.Exists(competition.FileName))
				//	File.Delete(competition.FileName);
				if (competition != null && File.Exists(competition.ExcelName))
					File.Delete(competition.ExcelName);

				Competition savedPrev = Model.Prev;
				Model.Competition = null;
				Model.Prev = savedPrev;
				ReloadPage();
			}
		}

		async void OnSaveAsClicked(object sender, EventArgs e)
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
	}
}