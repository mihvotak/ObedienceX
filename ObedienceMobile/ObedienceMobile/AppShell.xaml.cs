using ObedienceX.ViewModels;
using ObedienceX.Views;
using ObedienceX.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ObedienceX
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ExaminationsPage), typeof(ExaminationsPage));
			Routing.RegisterRoute(nameof(PairsPage), typeof(PairsPage));
			Routing.RegisterRoute(nameof(PairDetailsPage), typeof(PairDetailsPage));
			Routing.RegisterRoute(nameof(PairMarksPage), typeof(PairMarksPage));
		}

	}
}
