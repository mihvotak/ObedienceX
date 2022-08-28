using ObedienceMobile.Views;
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
			base.OnAppearing();

			collectionView.ItemsSource = Model.Competition.Examinations;
			_selectedExamination = null;
		}

		public void OnAddClicked(object sender, EventArgs e)
		{
			Model.Competition.Examinations.Add(new Examination());
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection != null)
			{
				if (_selectedExamination != null)
					_selectedExamination.SetSelected(false);

				var exam = (Examination)e.CurrentSelection.FirstOrDefault();
				if (exam == _selectedExamination)
				{
					_selectedExamination = null;
				}
				else
					_selectedExamination = exam;

				if (_selectedExamination != null)
					_selectedExamination.SetSelected(true);
			}
		}

		public void OnRemoveClicked(object sender, EventArgs e)
		{
			if (_selectedExamination != null)
				Model.Competition.Examinations.Remove(_selectedExamination);
		}
	}
}