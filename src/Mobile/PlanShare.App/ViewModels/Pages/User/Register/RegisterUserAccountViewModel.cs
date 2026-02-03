using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.User.Register;

namespace PlanShare.App.ViewModels.Pages.User.Register;

public partial class RegisterUserAccountViewModel(INavigationService navigationService, IRegisterUserUseCase registerUserUseCase) : ViewModelBase
{
    [ObservableProperty]
    public partial UserRegisterAccount Model { get; set; } = new();

    [RelayCommand]
    public async Task GoToLogin() => await navigationService.GoToAsync($"../{RoutePages.DO_LOGIN_PAGE}");

    [RelayCommand]
    public async Task RegisterAccount()
    {
        StatusPage = StatusPage.Sending;

        await registerUserUseCase.Execute(Model);

        StatusPage = StatusPage.Default;
    }
}
