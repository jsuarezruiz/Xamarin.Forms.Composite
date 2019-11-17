namespace Xamarin.Forms.Composite
{
    public class TemplateView : ContentView
    {
        public static readonly BindableProperty TemplateProperty = BindableProperty.Create(nameof(Template), typeof(ControlTemplate), typeof(TemplateView), null);

        public ControlTemplate Template
        {
            get => (ControlTemplate)GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }
    }
}
