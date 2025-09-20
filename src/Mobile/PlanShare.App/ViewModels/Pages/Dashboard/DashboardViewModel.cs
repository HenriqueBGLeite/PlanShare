using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public string userName;

    public DashboardViewModel(IUserStorage userStorage, 
        INavigationService navigationService)
    {
        UserName = userStorage.Get().Name;

        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task SeeProfile() => await _navigationService.GoToAsync(RoutePages.USER_UPDATE_PROFILE_PAGE);
}
