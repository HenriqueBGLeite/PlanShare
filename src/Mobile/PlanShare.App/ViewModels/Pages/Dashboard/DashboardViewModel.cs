using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Data.Storage.Preferences.User;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel(INavigationService navigationService, IUserStorage userStorage) : ViewModelBase
{
    [ObservableProperty]
    public partial string UserName { get; set; } = userStorage.Get().Name;

    [RelayCommand]
    public async Task SeeProfile() => await navigationService.GoToAsync(RoutePages.USER_UPDATE_PROFILE_PAGE);
}
