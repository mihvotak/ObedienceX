
namespace ObedienceX
{
	public interface IPermissionChecker
	{
		bool CheckAllFilesPermission();
		void ShowSettingsAllFilesPermission();
		string GetConfirmCaption();
		string GetConfirmReadText();
		string GetConfirmSaveText();
		string GetAgreeButton();
		string GetCancelButton();
	}
}
