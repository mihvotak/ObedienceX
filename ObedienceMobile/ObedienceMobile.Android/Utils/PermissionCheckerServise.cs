using ObedienceX.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PermissionCheckerServise))]
namespace ObedienceX.Droid
{
	public class PermissionCheckerServise : IPermissionChecker
	{
		public static MainActivity MainActivity;

		public bool CheckAllFilesPermission()
		{
			return Android.OS.Environment.IsExternalStorageManager;
		}

		public void ShowSettingsAllFilesPermission()
		{
			MainActivity.StartActivityForResult(new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission), 3);
		}

		public string GetConfirmCaption()
		{
			return "Доступ к файлам";
		}

		private const string ActionText = "необходимо 1) открыть настройки, 2) найти в списке данное приложение, и 3) разрешить ему \"доступ ко всем файлам\"";
		
		public string GetConfirmReadText()
		{
			return "Для отображения файлов соревнований на этом устройстве " + ActionText;
		}

		public string GetConfirmSaveText()
		{
			return "Для сохранения файлов соревнований на этом устройстве " + ActionText;
		}

		public string GetAgreeButton()
		{
			return "Перейти к настройкам";
		}

		public string GetCancelButton()
		{
			return "Не сейчас";
		}
	}
}
