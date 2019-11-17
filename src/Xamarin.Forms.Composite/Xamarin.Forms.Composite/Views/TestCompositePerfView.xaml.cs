using Xamarin.Forms.Composite.Services;
using Xamarin.Forms.Composite.ViewModels;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Composite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestCompositePerfView : ProfilerPage
    {
        public TestCompositePerfView()
        {
            InitializeComponent();
            BindingContext = new TestCompositePerfViewModel(DependencyService.Get<IProfilerService>());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as TestCompositePerfViewModel).LoadDataAsync();
        }
    }
}