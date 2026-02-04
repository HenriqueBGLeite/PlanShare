using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.Login.DoLogin;

namespace PlanShare.App.ViewModels.Pages.Login.DoLogin;

public partial class DoLoginViewModel(INavigationService navigationService, IDoLoginUseCase loginUseCase) : ViewModelBase(navigationService)
{
    [ObservableProperty]
    public partial Models.Login Model { get; set; } = new();

    [RelayCommand]
    public async Task DoLogin()
    {
        StatusPage = StatusPage.Sending;

        var result = await loginUseCase.Execute(Model);

        if (result.IsSuccess == false)
            await _navigationService.GoToAsync($"//{RoutePages.DASHBOARD_PAGE}");
        else
            await GoToPageWithErrors(result);

        StatusPage = StatusPage.Default;
    }
}
