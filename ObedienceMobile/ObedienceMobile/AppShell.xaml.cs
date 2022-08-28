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
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			Routing.RegisterRoute(nameof(ExaminationsPage), typeof(ExaminationsPage));
		}

	}
}
