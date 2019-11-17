using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Composite.Models;
using Xamarin.Forms.Composite.Services;

namespace Xamarin.Forms.Composite.ViewModels
{
    public class TestCompositePerfViewModel : BindableObject
    {
        string _startupTime;
        List<Timing> _timings;
        Timing _firstTiming;

        private readonly IProfilerService _profilerService;

        public TestCompositePerfViewModel(IProfilerService profilerService)
        {
            _profilerService = profilerService;
        }

        public string StartupTime
        {
            get => _startupTime;
            set
            {
                _startupTime = value;
                OnPropertyChanged();
            }
        }

        public List<Timing> Timings
        {
            get => _timings;
            set
            {
                _timings = value;
                OnPropertyChanged();
            }
        }

        public Timing FirstTiming
        {
            get => _firstTiming;
            set
            {
                _firstTiming = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadDataAsync()
        {
            StartupTime = _profilerService.GetStartupTime()?.ToLocalTime().ToString() ?? "-";
            var timings = await _profilerService.GetTimingsSinceStartup();

            Timings = timings
                .Select(t => new Timing(t.Key, t.Value))
                .ToList();
            FirstTiming = Timings.FirstOrDefault();
        }
    }
}