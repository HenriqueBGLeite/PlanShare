using CommunityToolkit.Mvvm.ComponentModel;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Profile;

public partial class UserProfileViewModel(INavigationService navigationService, IUserStorage userStorage) : ViewModelBase
{
    [ObservableProperty]
    public partial Models.User Model { get; set; } = new()
    {
        Name = userStorage.Get().Name,
        Email = "eduardo@gmail.com"
    };
}
