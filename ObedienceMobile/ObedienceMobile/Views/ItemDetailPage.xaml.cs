using ObedienceMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ObedienceMobile.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}