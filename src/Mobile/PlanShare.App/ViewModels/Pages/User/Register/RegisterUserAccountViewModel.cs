using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserAccountViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public UserRegisterAccount model;

    public RegisterUserAccountViewModel(INavigationService navigationService)
    {
        Model = new UserRegisterAccount();

        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task GoToLogin() => await _navigationService.GoToAsync($"../{RoutePages.LOGIN_PAGE}");

    [RelayCommand]
    public async Task RegisterAccount()
    {
        var x = Model;
    }
}
