using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.Login.DoLogin;

namespace PlanShare.App.ViewModels.Pages.Login.DoLogin;

public partial class DoLoginViewModel : ViewModelBase
{
    private readonly IDoLoginUseCase _loginUseCase;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public Models.Login model;

    public DoLoginViewModel(IDoLoginUseCase loginUseCase, 
        INavigationService navigationService)
    {
        Model = new Models.Login();
        _loginUseCase = loginUseCase;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task DoLogin()
    {
        StatusPage = StatusPage.Sending;

        var result = await _loginUseCase.Execute(Model);

        if (result.IsSuccess)
            await _navigationService.GoToAsync($"//{RoutePages.DASHBOARD_PAGE}");
        else 
        {
            var parameters = new Dictionary<string, object>
            {
                { "errors", result.ErrorMessages! }
            };

            await _navigationService.GoToAsync(RoutePages.ERROR_PAGE, parameters);
        }            
            
        StatusPage = StatusPage.Default;
    }
}
