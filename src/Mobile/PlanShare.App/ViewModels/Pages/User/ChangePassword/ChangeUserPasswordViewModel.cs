using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.User.ChangePassword;

namespace PlanShare.App.ViewModels.Pages.User.ChangePassword;

public partial class ChangeUserPasswordViewModel(INavigationService navigationService, IChangeUserPasswordUseCase changeUserPasswordUseCase) : ViewModelBase(navigationService)
{
    [ObservableProperty]
    public partial Models.ChangePassword Model { get; set; } = new();

    [RelayCommand]
    public async Task ChangePassword()
    {
        StatusPage = Models.StatusPage.Sending;

        var result = await changeUserPasswordUseCase.Execute(Model);

        if (result.IsSuccess)
        {
            await _navigationService.GoToAsync("..");
        }
        else
            await GoToPageWithErrors(result);

        StatusPage = Models.StatusPage.Default;
    }
}
