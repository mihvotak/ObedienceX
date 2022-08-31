﻿using ObedienceX.Views;
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
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			var exam = new Examination();
			exam.SetNumber(Model.Competition.Examinations.Count + 1);
			Model.Competition.Examinations.Add(exam);
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
			int index = (int)((Button)sender).CommandParameter - 1;
			if (index >= 0 && index <= Model.Competition.Examinations.Count - 1)
			Model.Competition.Examinations.RemoveAt(index);
			UpdateNumbers();
		}

		private void UpdateNumbers()
		{
			for (int i = 0; i < Model.Competition.Examinations.Count; i++)
				Model.Competition.Examinations[i].SetNumber(i + 1);
		}
	}
}