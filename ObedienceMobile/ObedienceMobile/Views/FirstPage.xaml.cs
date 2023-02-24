using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstPage : ContentPage
	{
		public FirstPage()
		{
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
		}

		async void OnOpenClick(object sender, EventArgs args)
		{
			bool force = true;
			if (Model.Competition != null && !Model.Competition.Saved)
				force = await DisplayAlert("Файл не сохранён.", "Всё равно продолжить?", "Да", "Нет");
			if (force)
			{
				Model.ChangeFolderMode = false;
				await Shell.Current.GoToAsync($"{nameof(CompetitionsPage)}");
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
	}
}