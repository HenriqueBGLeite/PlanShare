using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.OnBoarding;

public partial class OnBoardingViewModel(INavigationService navigationService) : ViewModelBase(navigationService)
{
    [RelayCommand]
    public async Task LoginWithEmailAndPassword() => await _navigationService.GoToAsync(RoutePages.DO_LOGIN_PAGE);
    

    [RelayCommand]
    public void LoginWithGoogle()
    {

    }

    [RelayCommand]
    public async Task RegisterUserAccount() => await _navigationService.GoToAsync(RoutePages.USER_REGISTER_ACCOUNT_PAGE);
}
