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

			var competition = Model.Competition;
			competition.RecalculateResults();
			for (int i = 0; i < competition.Pairs.Count; i++)
				competition.Pairs[i].SetNumber(i + 1);
			collectionView.ItemsSource = competition.Pairs;
			if (competition.LastPairIndex >= 0 && competition.LastPairIndex < competition.Pairs.Count)
				competition.Pairs[competition.LastPairIndex].DispatchNamesChanged();
			collectionView.SelectedItem = null;
			Title = competition.Name;
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			var pair = new Pair();
			Model.Competition.Pairs.Add(pair);
			pair.SetNumber(Model.Competition.Pairs.Count);
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
	}
}