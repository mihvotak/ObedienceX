using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaveAsPage : ContentPage
	{
		private SaveAsData _data;
		public SaveAsPage()
		{
			_data = new SaveAsData();

			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			_data.FileName = Model.Competition.Name;
			_data.Error = null;
			_data.DispatchChanged();
			base.OnAppearing();
			BindingContext = _data;
		}

		async public void OnSaveClicked(object sender, EventArgs e)
		{
			_data.Error = null;
			string fullPath = Path.Combine(App.FolderPath, _data.FileName + App.FileExtention);
			if (Model.SaveCurrentAs(fullPath))
				await Shell.Current.GoToAsync("..");
			else
			{
				_data.Error = Model.LastError;
				Model.LastError = null;
			}
		}

		private async void OnBrowseClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"//{nameof(CompetitionsPage)}");
		}

		private async void OnCancelClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}