using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using ObedienceX;
using ObedienceX.Droid;
using System;

namespace ObedienceMobile.Droid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseSharedMauiApp();
		builder.Services.AddSingleton<IPermissionChecker, PermissionCheckerServise>();

		return builder.Build();
    }
}
