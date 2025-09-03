using ObedienceX.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PairsPage : ContentPage
	{

		public PairsPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			UpdateNumbers(false);
			base.OnAppearing();

			collectionView.ItemsSource = Model.Competition.Pairs;
			if (Model.Competition.LastPairIndex >= 0 && Model.Competition.LastPairIndex < Model.Competition.Pairs.Count)
				Model.Competition.Pairs[Model.Competition.LastPairIndex].DispatchNamesChanged();
			collectionView.SelectedItem = null;
			Title = Model.Competition.Name;
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			var pair = new Pair() { Competition = Model.Competition, Marks = new ObservableCollection<MarksSet>() };
			Model.Competition.Pairs.Add(pair);
			pair.SetNumber(Model.Competition.Pairs.Count);
			pair.StartNumber = pair.Number;
			Model.Competition.Saved = false;
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				var pair = (Pair)e.CurrentSelection.FirstOrDefault();
				if (pair != null)
				{
					Model.Competition.LastPairIndex = Model.Competition.Pairs.IndexOf(pair);
					AppShell.Current.GoToAsync(nameof(PairDetailsPage));
				}
			}
		}

		public void OnRemoveClicked(object sender, EventArgs e)
		{
			int index = (int)((Button)sender).CommandParameter - 1;
			if (index >= 0 && index <= Model.Competition.Pairs.Count - 1)
			{
				Model.Competition.Pairs.RemoveAt(index);
				Model.Competition.Saved = false;
			}
			UpdateNumbers(true);
		}

		private void UpdateNumbers(bool resetStartNums)
		{
			for (int i = 0; i < Model.Competition.Pairs.Count; i++)
			{
				Model.Competition.Pairs[i].SetNumber(i + 1);
				if (resetStartNums)
					Model.Competition.Pairs[i].StartNumber = i + 1;
			}
		}
	}
}