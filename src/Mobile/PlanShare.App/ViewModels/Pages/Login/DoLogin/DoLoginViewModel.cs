using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.UseCases.Login.DoLogin;
using PlanShare.App.Models;

namespace PlanShare.App.ViewModels.Pages.Login.DoLogin;

public partial class DoLoginViewModel(IDoLoginUseCase loginUseCase) : ViewModelBase
{
    [ObservableProperty]
    public partial Models.Login Model { get; set; } = new();

    [RelayCommand]
    public async Task DoLogin()
    {
        StatusPage = StatusPage.Sending;

        await Task.Delay(2000);

        await loginUseCase.Execute(Model);

        StatusPage = StatusPage.Default;
    }
}
