using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PairMarksPage : ContentPage
	{
		public PairMarksPage()
		{
			InitializeComponent();
		}

		private Pair CurrentPair
		{
			get
			{
				int index = 0;
				if (Model.Competition.LastPairIndex >= 0 && Model.Competition.LastPairIndex < Model.Competition.Pairs.Count)
					index = Model.Competition.LastPairIndex;
				return Model.Competition.Pairs[index];
			}
		}

		private Layout _marksSelectionLayout;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Competition competition = Model.Competition;
			Title = "Пара #" + (competition.LastPairIndex + 1);

			Pair pair = CurrentPair;
			if (pair.Marks == null)
				pair.Marks = new ObservableCollection<Mark>();
			while (pair.Marks.Count < competition.Examinations.Count)
				pair.Marks.Add(new Mark());
			while (pair.Marks.Count > competition.Examinations.Count)
				pair.Marks.RemoveAt(pair.Marks.Count - 1);
			for (int i = 0; i < pair.Marks.Count; i++)
			{
				competition.Examinations[i].SetNumber(i + 1);
				pair.Marks[i].Pair = pair;
				pair.Marks[i].Examination = competition.Examinations[i];
			}

			BindingContext = pair;
			collectionView.ItemsSource = pair.Marks;
			ToolbarItems[0].BindingContext = competition;
			_marksSelectionLayout = (Layout)FindByName("MarkSelector");
			_marksSelectionLayout.IsVisible = false;
		}

		void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
		}

		public void OnMarkClicked(object sender, EventArgs e)
		{
			int index = (int)((Button)sender).CommandParameter - 1;
			if (index >= 0 && index <= CurrentPair.Marks.Count - 1)
			{
				CurrentPair.CurrentMarkIndex = index;
				_marksSelectionLayout.IsVisible = true;
			}
		}

		public void OnMarkSelected(object sender, EventArgs e)
		{
			string val = (string)((Button)sender).CommandParameter;
			CurrentPair.Marks[CurrentPair.CurrentMarkIndex].ValueStr = val;
			_marksSelectionLayout.IsVisible = false;
		}
	}
}