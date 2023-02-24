using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.Content;
using System.Collections.Generic;

namespace XamarinGoogleMapSample.Droid
{
    [Activity(Label = "XamarinGoogleMapSample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            PermissionCheck();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void PermissionCheck()
        {
            List<string> permissions = new List<string>();
            List<string> checkPermissions = new List<string>();
            checkPermissions.Add(Android.Manifest.Permission.AccessNetworkState);
            checkPermissions.Add(Android.Manifest.Permission.ChangeNetworkState);
            checkPermissions.Add(Android.Manifest.Permission.AccessCoarseLocation);
            checkPermissions.Add(Android.Manifest.Permission.AccessFineLocation);

            foreach (var checkPermission in checkPermissions)
            {
                if (this.CheckSelfPermission(checkPermission) != (int)Permission.Granted)
                {
                    permissions.Add(checkPermission);
                }
            }

            if (permissions.Count > 0)
            {
                this.RequestPermissions(permissions.ToArray(), 1);

            }

        }
    }
}