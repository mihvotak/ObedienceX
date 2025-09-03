using ObedienceX.Views;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ObedienceX
{
	public partial class AppShell : Microsoft.Maui.Controls.Shell
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
