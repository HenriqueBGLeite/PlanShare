using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;

namespace PlanShare.App.ViewModels.Pages.User.Connection;

public partial class UserCodeConnectionViewModel(INavigationService navigationService) : ViewModelBase(navigationService)
{
    [RelayCommand]
    public async Task UserCompletedCode(string code)
    {

    }
}
