using Android.Content;
using Android.Util;
using System;
using Xamarin.Forms;
using XamarinGoogleMapSample.Droid;
using XamarinGoogleMapSample.Interfaces;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AppSettingsInterface))]
namespace XamarinGoogleMapSample.Droid
{
    public class AppSettingsInterface : IAppSettingsHelper
    {
        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            //string package_name = "com.socialfamily.pvit";
            string package_name = "Application.Context.PackageName";
            var uri = Android.Net.Uri.FromParts("package", package_name, null);
            intent.SetData(uri);
            Application.Context.StartActivity(intent);
        }

        public void OpenLocationSetting()
        {
            try
            {
                var intent = new Android.Content.Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                Xamarin.Essentials.Platform.CurrentActivity.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Log.Debug("OpenLocationSetting", ex.Message);
            }
        }
    }
}