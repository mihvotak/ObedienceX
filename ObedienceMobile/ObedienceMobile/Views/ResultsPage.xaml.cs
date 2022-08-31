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

			Model.Competition.RecalculateResults();
			collectionView.ItemsSource = Model.Competition.Pairs;
			if (Model.Competition.LastPairIndex >= 0 && Model.Competition.LastPairIndex < Model.Competition.Pairs.Count)
				Model.Competition.Pairs[Model.Competition.LastPairIndex].DispatchNamesChanged();
			collectionView.SelectedItem = null;
			Title = Model.Competition.Name;
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