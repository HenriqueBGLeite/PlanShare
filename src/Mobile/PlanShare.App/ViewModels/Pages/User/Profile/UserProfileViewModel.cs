using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.User.Profile;

namespace PlanShare.App.ViewModels.Pages.User.Profile;

public partial class UserProfileViewModel(INavigationService navigationService, IGetUserProfileUseCase getUserProfileUseCase) : ViewModelBase(navigationService)
{
    [ObservableProperty]
    public partial Models.User Model { get; set; } = new();

    [RelayCommand]
    public async Task Initialize()
    {
        StatusPage = Models.StatusPage.Loading;

        var result = await getUserProfileUseCase.Execute();            

        if (result.IsSuccess)
            Model = result.Response!;
        else
            await GoToPageWithErrors(result);

        StatusPage = Models.StatusPage.Default;
    }
}
