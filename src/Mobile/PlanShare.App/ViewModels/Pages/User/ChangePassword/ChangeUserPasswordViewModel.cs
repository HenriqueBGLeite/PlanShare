using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;
using PlanShare.App.Resources;
using PlanShare.App.UseCases.User.ChangePassword;

namespace PlanShare.App.ViewModels.Pages.User.ChangePassword;

public partial class ChangeUserPasswordViewModel : ViewModelBase
{
    private readonly IChangeUserPasswordUseCase _changeUserPasswordUseCase;

    [ObservableProperty]
    public partial Models.ChangePassword Model { get; set; } = new();

    public ChangeUserPasswordViewModel(INavigationService navigationService, 
        IChangeUserPasswordUseCase changeUserPasswordUseCase) : base(navigationService)
    {
        _changeUserPasswordUseCase = changeUserPasswordUseCase;
    }

    [RelayCommand]
    public async Task ChangePassword()
    {
        StatusPage = Models.StatusPage.Sending;

        var result = await _changeUserPasswordUseCase.Execute(Model);

        if (result.IsSuccess)
        {
            await _navigationService.ShowSuccessFeedback(ResourceTexts.PASSWORD_SUCCESSFULLY_CHANGED);
            await _navigationService.ClosePage();
        }
        else
            await GoToPageWithErrors(result);

        StatusPage = Models.StatusPage.Default;
    }
}
