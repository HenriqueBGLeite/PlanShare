using CommunityToolkit.Mvvm.ComponentModel;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Models;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Profile;

public partial class UserProfileViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public Models.User model;

    public UserProfileViewModel(IUserStorage userStorage,
        INavigationService navigationService)
    {
        var user = userStorage.Get();

        Model = new()
        {
            Name = "Henrique Batista",
            Email = "henrique@gmail.com"
        };

        _navigationService = navigationService;
    }
}
