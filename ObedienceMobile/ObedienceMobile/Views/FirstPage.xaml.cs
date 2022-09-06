using ObedienceX.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObedienceX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstPage : ContentPage
	{
		public FirstPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Label nameLabel = (Label)FindByName("FileName");
			Competition competition = Model.Competition;
			nameLabel.Text = competition == null ? "Пока тут пусто" :
				string.IsNullOrEmpty(competition.ExcelName) ? "Файл соревнований не сохранён" :
				$"Файл:\n{competition.Name}\nиз папки:\n{Path.GetDirectoryName(competition.ExcelName)}";
		}

		async void OnOpenClick(object sender, EventArgs args)
		{
			Model.ChangeFolderMode = false;
			await Shell.Current.GoToAsync($"{nameof(CompetitionsPage)}");
		}

		async void OnAddClicked(object sender, EventArgs e)
		{
			Model.Competition = new Competition();
			// Navigate to the CompetitionPage, without passing any data.
			await Shell.Current.GoToAsync($"//{nameof(CompetitionPage)}");
		}
	}
}