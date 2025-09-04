using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Java.Lang;
using Microsoft.Maui.Controls.PlatformConfiguration;
using ObedienceX.Droid;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(PermissionCheckerServise))]
namespace ObedienceX.Droid
{
	public class PermissionCheckerServise : IPermissionChecker
	{
		public bool CheckAllFilesPermission()
		{
			return Environment.IsExternalStorageManager;				
		}

		public void ShowSettingsAllFilesPermission()
		{
			if (!Environment.IsExternalStorageManager)
			{
				try
				{
					var intent = new Intent(Settings.ActionManageAllFilesAccessPermission);
					intent.SetData(Uri.Parse($"package:{Application.Context.PackageName}"));
					intent.SetFlags(ActivityFlags.NewTask);
					Application.Context.StartActivity(intent);
				}
				catch(Exception _)
				{
					Intent intent = new Intent();
					intent.SetAction(Settings.ActionManageAllFilesAccessPermission);
					intent.SetFlags(ActivityFlags.NewTask);
					Application.Context.StartActivity(intent);
				}
			}
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
