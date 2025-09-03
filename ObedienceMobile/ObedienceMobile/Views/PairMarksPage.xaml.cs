using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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
				pair.Marks = new ObservableCollection<MarksSet>();
			while (pair.Marks.Count < competition.Examinations.Count)
				pair.Marks.Add(new MarksSet());
			while (pair.Marks.Count > competition.Examinations.Count)
				pair.Marks.RemoveAt(pair.Marks.Count - 1);
			for (int i = 0; i < pair.Marks.Count; i++)
			{
				competition.Examinations[i].SetNumber(i + 1);
				MarksSet marksSet = pair.Marks[i];
				marksSet.Pair = pair;
				marksSet.Examination = competition.Examinations[i];
				while (marksSet.Marks.Count > competition.JudgesCount)
					marksSet.Marks.RemoveAt(marksSet.Marks.Count - 1);
				while (marksSet.Marks.Count < competition.JudgesCount)
					marksSet.Marks.Add(new Mark() { MarksSet = marksSet});
				for (int j = 0; j < competition.JudgesCount; j++)
					marksSet.Marks[j].MarksSet = marksSet;
				marksSet.RecalculateMidValue(false);
			}
			pair.RecalculateSum();

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
			int judge = index / 100;
			index = index % 100;
			if (index >= 0 && index <= CurrentPair.Marks.Count - 1 && judge >= 0 && judge < Model.Competition.JudgesCount)
			{
				CurrentPair.CurrentExamIndex = index;
				CurrentPair.CurrentJudgeIndex = judge;
				_marksSelectionLayout.IsVisible = true;
			}
		}

		public void OnMarkUpClicked(object sender, EventArgs e)
		{
			int index = (int)((Button)sender).CommandParameter - 1;
			index = index % 100;
			if (Model.Competition.JudgesCount == 1)
			{
				MarksSet marksSet = CurrentPair.Marks[index];
				Mark mark = marksSet.Marks[0];
				if (mark.IsSet && mark.Value < 10)
				{
					mark.ValueStr = (mark.Value + 0.25).ToString();
					marksSet.DispatchValueChanged(0);
				}
			}
		}

		public void OnMarkSelected(object sender, EventArgs e)
		{
			string val = (string)((Button)sender).CommandParameter;
			CurrentPair.Marks[CurrentPair.CurrentExamIndex].Marks[CurrentPair.CurrentJudgeIndex].ValueStr = val;
			CurrentPair.Marks[CurrentPair.CurrentExamIndex].DispatchValueChanged(CurrentPair.CurrentJudgeIndex);
			_marksSelectionLayout.IsVisible = false;
		}
	}
}