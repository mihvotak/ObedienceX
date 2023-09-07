using ObedienceX.Views;
using Xamarin.Forms;

namespace ObedienceX
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(CompetitionsPage), typeof(CompetitionsPage));
			Routing.RegisterRoute(nameof(ExaminationsPage), typeof(ExaminationsPage));
			Routing.RegisterRoute(nameof(JudgesPage), typeof(JudgesPage));
			Routing.RegisterRoute(nameof(PairsPage), typeof(PairsPage));
			Routing.RegisterRoute(nameof(PairDetailsPage), typeof(PairDetailsPage));
			Routing.RegisterRoute(nameof(PairMarksPage), typeof(PairMarksPage));
			Routing.RegisterRoute(nameof(SaveAsPage), typeof(SaveAsPage));
		}

	}
}
