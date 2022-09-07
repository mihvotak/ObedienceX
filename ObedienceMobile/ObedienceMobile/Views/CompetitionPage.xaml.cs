using ObedienceX.Data;
using ObedienceX.Utils;
using System;
using System.IO;

using Xamarin.Forms;

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

			if (Model.Competition == null)
				Model.Competition = new Competition();
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
			if (await Shell.Current.DisplayAlert("Удаление", "Текущий файл соревнований будет удален насовсем. Вы уверены?", "Удалить", "Отменить"))
			{
				var competition = (Competition)BindingContext;

				// Delete the file.
				//if (File.Exists(competition.FileName))
				//	File.Delete(competition.FileName);
				if (File.Exists(competition.ExcelName))
					File.Delete(competition.ExcelName);

				// Navigate backwards
				Model.Competition = null;
				await Shell.Current.GoToAsync($"//{nameof(FirstPage)}");
			}
		}

	}
}
