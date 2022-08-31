using ObedienceMobile.ViewModels;
using ObedienceMobile.Views;
using ObedienceX.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ObedienceMobile
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ExaminationsPage), typeof(ExaminationsPage));
			Routing.RegisterRoute(nameof(PairsPage), typeof(PairsPage));
			Routing.RegisterRoute(nameof(PairDetailsPage), typeof(PairDetailsPage));
		}

	}
}
