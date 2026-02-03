using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.OnBoarding;

public partial class OnBoardingViewModel(INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    public async Task LoginWithEmailAndPassword() => await navigationService.GoToAsync(RoutePages.DO_LOGIN_PAGE);
    

    [RelayCommand]
    public void LoginWithGoogle()
    {

    }

    [RelayCommand]
    public async Task RegisterUserAccount() => await navigationService.GoToAsync(RoutePages.USER_REGISTER_ACCOUNT_PAGE);
}
