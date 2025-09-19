using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.UseCases.Login.DoLogin;

namespace PlanShare.App.ViewModels.Pages.Login.DoLogin;

public partial class DoLoginViewModel : ViewModelBase
{
    private readonly IDoLoginUseCase _loginUseCase;

    [ObservableProperty]
    public Models.Login model;

    public DoLoginViewModel(IDoLoginUseCase loginUseCase)
    {
        Model = new Models.Login();
        _loginUseCase = loginUseCase;
    }

    [RelayCommand]
    public async Task DoLogin()
    {
        StatusPage = StatusPage.Sending;

        await Task.Delay(4000);

        await _loginUseCase.Execute(Model);

        StatusPage = StatusPage.Default;
    }
}
