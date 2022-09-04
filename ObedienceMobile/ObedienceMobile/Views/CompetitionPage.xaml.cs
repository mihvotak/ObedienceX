using ObedienceX.Data;
using ObedienceX.Utils;
using System;
using System.IO;

using Xamarin.Forms;

namespace ObedienceX.Views
{
	public partial class CompetitionPage : ContentPage
	{
		private ExcelProxy _excelProxy;

		public CompetitionPage()
		{
			InitializeComponent();

			_excelProxy = new ExcelProxy();
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

		void OnSaveExcelClicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Model.Competition.ExcelName))
				_excelProxy.WriteExcel(Model.Competition.ExcelName);
			else
			{
				_excelProxy.WriteExcel(Model.Competition.ExcelName);
			}
		}
	}
}
