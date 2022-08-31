using ObedienceX.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
		}

	}
}