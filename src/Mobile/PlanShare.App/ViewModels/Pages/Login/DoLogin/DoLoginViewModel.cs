using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.Login.DoLogin;

namespace PlanShare.App.ViewModels.Pages.Login.DoLogin;

public partial class DoLoginViewModel : ViewModelBase
{
    private readonly IDoLoginUseCase _loginUseCase;

    [ObservableProperty]
    public partial Models.Login Model {  get; set; }

    public DoLoginViewModel(IDoLoginUseCase loginUseCase, 
        INavigationService navigationService) : base(navigationService)
    {
        Model = new Models.Login();
        _loginUseCase = loginUseCase;
    }

    [RelayCommand]
    public async Task DoLogin()
    {
        StatusPage = StatusPage.Sending;

        var result = await _loginUseCase.Execute(Model);

        if (result.IsSuccess)
            await _navigationService.GoToAsync($"//{RoutePages.DASHBOARD_PAGE}");
        else
            await GoToPageWithErrors(result);
                
        StatusPage = StatusPage.Default;
    }
}
