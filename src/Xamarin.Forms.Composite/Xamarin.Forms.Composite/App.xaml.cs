using Xamarin.Forms.Composite.Services;
using Xamarin.Forms.Composite.Views;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xamarin.Forms.Composite
{
    public partial class App : Application
    {
        public static IProfilerService ProfilerService { get; set; } = DependencyService.Get<IProfilerService>();

        public App()
        {
            InitializeComponent();

            //MainPage = new TestCompositeView();
            MainPage = new TestCompositePerfView();
        }

        protected override void OnStart()
        {
            if (ProfilerService != null)
                ProfilerService.RegisterEvent("Xamarin.Forms App OnStart");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
