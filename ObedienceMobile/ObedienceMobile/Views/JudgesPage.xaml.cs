using ObedienceX.Views;
using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JudgesPage : ContentPage
	{
		private Judge _selectedJudge;

		public JudgesPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			UpdateNumbers();
			base.OnAppearing();

			collectionView.ItemsSource = Model.Competition.Judges;
			Title = Model.Competition.Name;
			_selectedJudge = null;
			ToolbarItems[0].BindingContext = Model.Competition;
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			if (Model.Competition.Judges.Count < Competition.MaxJudgesCount)
			{
				var judge = new Judge();
				judge.SetNumber(Model.Competition.Judges.Count + 1);
				Model.Competition.Judges.Add(judge);
				Model.Competition.DispatchJudgesChanged();
				Model.Competition.Saved = false;
			}
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				var judge = (Judge)e.CurrentSelection.FirstOrDefault();
				if (judge == _selectedJudge)
				{
					_selectedJudge = null;
				}
				else
					_selectedJudge = judge;
			}
		}

		public void OnRemoveClicked(object sender, EventArgs e)
		{
			if (Model.Competition.Judges.Count > 1)
			{
				Model.Competition.Judges.RemoveAt(Model.Competition.Judges.Count - 1);
				Model.Competition.DispatchJudgesChanged();
				Model.Competition.Saved = false;
			}
		}

		public void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent(this.Handler.MauiContext);
		}

		private void UpdateNumbers()
		{
			for (int i = 0; i < Model.Competition.Judges.Count; i++)
				Model.Competition.Judges[i].SetNumber(i + 1);
		}

		void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.OldTextValue != null && e.NewTextValue != null)
				Model.Competition.Saved = false;
		}

	}
}