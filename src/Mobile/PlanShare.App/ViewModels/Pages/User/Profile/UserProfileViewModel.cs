using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using PlanShare.App.Models.Enums;
using PlanShare.App.Navigation;
using PlanShare.App.Resources;
using PlanShare.App.UseCases.User.Profile;
using PlanShare.App.UseCases.User.Update;
using PlanShare.App.ViewModels.Popups.File;

namespace PlanShare.App.ViewModels.Pages.User.Profile;

public partial class UserProfileViewModel(INavigationService navigationService, 
    IGetUserProfileUseCase getUserProfileUseCase, 
    IUpdateUserUseCase updateUserUseCase,
    IPopupService popupService) : ViewModelBase(navigationService)
{
    [ObservableProperty]
    public partial Models.User Model { get; set; } = new();

    [RelayCommand]
    public async Task Initialize()
    {
        //StatusPage = Models.StatusPage.Loading;

        //var result = await getUserProfileUseCase.Execute();            

        //if (result.IsSuccess)
        //    Model = result.Response!;
        //else
        //    await GoToPageWithErrors(result);

        //StatusPage = Models.StatusPage.Default;
    }

    [RelayCommand]
    public async Task UpdateProfile()
    {
        StatusPage = Models.StatusPage.Sending;

        var result = await updateUserUseCase.Execute(Model);

        if (result.IsSuccess)
        {
            await _navigationService.ShowSuccessFeedback(ResourceTexts.PROFILE_INFORMATION_SUCCESSFULLY_UPDATED);
        }
        else
            await GoToPageWithErrors(result);

        StatusPage = Models.StatusPage.Default;
    }

    [RelayCommand]
    public async Task ChangePassword() => await _navigationService.GoToAsync(RoutePages.USER_CHANGE_PASSWORD_PAGE);

    [RelayCommand]
    public async Task ChangeProfilePhoto()
    {
        var optionsSelected = await _navigationService.ShowPopup<OptionsForProfilePhotoViewModel, ChooseFileOption>();
    }
}
