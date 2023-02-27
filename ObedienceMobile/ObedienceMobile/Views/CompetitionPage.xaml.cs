using ObedienceX.Data;
using System.ComponentModel;
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
			bool saved = Model.Competition.Saved;
			BindingContext = Model.Competition;
			Model.Competition.Saved = saved;
			ToolbarItems[0].BindingContext = Model.Competition;
		}

		void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.OldTextValue != null && e.NewTextValue != null)
				Model.Competition.Saved = false;
		}

		void OnDateSelected(object sender, DateChangedEventArgs e)
		{
			if (e.OldDate != null && e.NewDate != null)
				Model.Competition.Saved = false;
		}
		
		void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
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

	}
}
