using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Composite.Controls;
using Xamarin.Forms.Composite.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(CompositeSlider), typeof(CompositeElementRenderer))]
namespace Xamarin.Forms.Composite.Droid.Renderers
{
    public class CompositeElementRenderer : ViewRenderer<View, AView>
    {
        public CompositeElementRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement != null)
            {
                (Element as ICompositeElementConfiguration)?.ElementChanged(true);
            }

            if (e?.OldElement != null)
            {
                (Element as ICompositeElementConfiguration)?.ElementChanged(false);
            }
        }
    }
}