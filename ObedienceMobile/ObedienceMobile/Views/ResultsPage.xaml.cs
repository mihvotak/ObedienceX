using ObedienceX;
using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultsPage : ContentPage
	{
		public ResultsPage()
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
			if (Model.Competition == null)
				Model.Competition = new Competition();
			var competition = Model.Competition;
			{
				List<Pair> pairs = new List<Pair>(competition.Pairs);
				pairs.Sort((p1, p2) => p2.StartNumber.CompareTo(p1.StartNumber));
				competition.Pairs = new System.Collections.ObjectModel.ObservableCollection<Pair>(pairs);
			}
			competition.RecalculateResults();
			for (int i = 0; i < competition.Pairs.Count; i++)
				competition.Pairs[i].SetNumber(i + 1);
			collectionView.ItemsSource = competition.Pairs;
			if (competition.LastPairIndex >= 0 && competition.LastPairIndex < competition.Pairs.Count)
				competition.Pairs[competition.LastPairIndex].DispatchNamesChanged();
			collectionView.SelectedItem = null;
			Title = competition.Name;
			ToolbarItems[0].BindingContext = competition;
			ToolbarItems[1].BindingContext = competition;
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				var pair = (Pair)e.CurrentSelection.FirstOrDefault();
				if (pair != null)
				{
					Model.Competition.LastPairIndex = Model.Competition.Pairs.IndexOf(pair);
					AppShell.Current.GoToAsync(nameof(PairMarksPage));
				}
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