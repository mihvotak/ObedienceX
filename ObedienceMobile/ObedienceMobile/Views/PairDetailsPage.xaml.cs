using ObedienceX.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PairDetailsPage : ContentPage
	{
		public PairDetailsPage()
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

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Title = "Пара #" + (Model.Competition.LastPairIndex + 1);
			BindingContext = CurrentPair;
			ToolbarItems[0].BindingContext = Model.Competition;
		}

		void OnSaveClicked(object sender, EventArgs e)
		{
			Model.ReSaveCurrent();
		}

		void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.OldTextValue != null && e.NewTextValue != null)
				Model.Competition.Saved = false;
		}

		void OnDateSelected(object sender, DateChangedEventArgs e)
		{
			if (e.OldDate != null && e.NewDate != null && e.OldDate != DateTime.Now.Date)
				Model.Competition.Saved = false;
		}
	}
}