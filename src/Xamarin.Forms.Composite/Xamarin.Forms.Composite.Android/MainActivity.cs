using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms.Composite.Droid.Services;

namespace Xamarin.Forms.Composite.Droid
{
    [Activity(Label = "Xamarin.Forms.Composite", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            DependencyService.Register<ProfilerService>();
            LoadApplication(new App());
        }
    }
}