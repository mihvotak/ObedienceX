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
	public partial class ExaminationsPage : ContentPage
	{
		private Examination _selectedExamination;

		public ExaminationsPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			UpdateNumbers();
			base.OnAppearing();

			collectionView.ItemsSource = Model.Competition.Examinations;
			Title = Model.Competition.Name;
			_selectedExamination = null;
			ToolbarItems[0].BindingContext = Model.Competition;
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			var exam = new Examination();
			exam.SetNumber(Model.Competition.Examinations.Count + 1);
			Model.Competition.Examinations.Add(exam);
			Model.Competition.Saved = false;
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				var exam = (Examination)e.CurrentSelection.FirstOrDefault();
				if (exam == _selectedExamination)
				{
					_selectedExamination = null;
				}
				else
					_selectedExamination = exam;
			}
		}

		public void OnRemoveClicked(object sender, EventArgs e)
		{
			if (Model.Competition.Examinations.Count > 0)
			{
				Model.Competition.Examinations.RemoveAt(Model.Competition.Examinations.Count - 1);
				Model.Competition.Saved = false;
			}
		}

		public void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
		}

		private void UpdateNumbers()
		{
			for (int i = 0; i < Model.Competition.Examinations.Count; i++)
				Model.Competition.Examinations[i].SetNumber(i + 1);
		}

		void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.OldTextValue != null && e.NewTextValue != null)
				Model.Competition.Saved = false;
		}

	}
}