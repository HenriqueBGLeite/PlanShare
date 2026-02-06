using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Models.Enums;
using PlanShare.App.Navigation;
using PlanShare.App.ViewModels.Popups.Connection;

namespace PlanShare.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel(INavigationService navigationService, IUserStorage userStorage) : ViewModelBase(navigationService)
{
    [ObservableProperty]
    public partial string UserName { get; set; } = userStorage.Get().Name;

    [RelayCommand]
    public async Task SeeProfile() => await _navigationService.GoToAsync(RoutePages.USER_UPDATE_PROFILE_PAGE);

    [RelayCommand]
    public async Task ConnectionByCode()
    {
        var result = await _navigationService.ShowPopup<OptionsForConnectionByCodeViewModel, ChooseCodeConnectionOption>();
    }
}
