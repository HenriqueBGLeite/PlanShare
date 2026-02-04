using PlanShare.App.Data.Storage.Preferences.User;

namespace PlanShare.App;

public partial class App : Application
{
    private readonly IUserStorage _userStorage;

    public App(IUserStorage userStorage)
    {
        InitializeComponent();
        _userStorage = userStorage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(_userStorage));
    }
}