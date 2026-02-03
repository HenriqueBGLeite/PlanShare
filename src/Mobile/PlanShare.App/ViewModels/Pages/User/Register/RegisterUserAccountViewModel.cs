using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserAccountViewModel(INavigationService navigationService) : ViewModelBase
{
    [ObservableProperty]
    public partial UserRegisterAccount Model { get; set; } = new();

    [RelayCommand]
    public async Task GoToLogin() => await navigationService.GoToAsync($"../{RoutePages.DO_LOGIN_PAGE}");

    [RelayCommand]
    public async Task RegisterAccount()
    {
        var teste = Model;
    }
}
