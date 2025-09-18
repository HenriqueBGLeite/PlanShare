namespace PlanShare.App.Views.Components.Inputs;

public partial class EntryAndLabelPasswordComponent : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty
    .Create(nameof(Title), typeof(string), typeof(EntryAndLabelPasswordComponent), string.Empty);

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public EntryAndLabelPasswordComponent()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private void ShowHidePassword(object sender, TappedEventArgs e)
    {
        if (EntryPassword.IsPassword)
        {
            EntryPassword.IsPassword = false;
            EntryPassword.Keyboard = Keyboard.Default;
            ImageEye.Source = "icon_eye.png";
        }
        else
        {
            EntryPassword.IsPassword = true;
            EntryPassword.Keyboard = Keyboard.Password;
            ImageEye.Source = "icon_eye_hidden.png";
        }
    }
}