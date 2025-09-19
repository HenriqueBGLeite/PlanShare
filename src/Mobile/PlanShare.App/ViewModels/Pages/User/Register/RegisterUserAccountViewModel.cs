using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.User.Register;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserAccountViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IRegisterUserUserCase _registerUserUserCase;

    [ObservableProperty]
    public UserRegisterAccount model;

    public RegisterUserAccountViewModel(INavigationService navigationService, 
        IRegisterUserUserCase registerUserUserCase)
    {
        Model = new UserRegisterAccount();

        _navigationService = navigationService;
        _registerUserUserCase = registerUserUserCase;
    }

    [RelayCommand]
    public async Task GoToLogin() => await _navigationService.GoToAsync($"../{RoutePages.LOGIN_PAGE}");

    [RelayCommand]
    public async Task RegisterAccount()
    {
        StatusPage = StatusPage.Sending;

        await Task.Delay(4000);

        await _registerUserUserCase.Execute(Model);

        StatusPage = StatusPage.Default;
    }
}
