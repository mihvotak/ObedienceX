using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ObedienceX.Utils
{
	public class PermissionService
	{
		public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission
		{
			var status = await permission.CheckStatusAsync();
			if (status != PermissionStatus.Granted)
			{
				status = await permission.RequestAsync();
			}

			return status;
		}
	}
}
