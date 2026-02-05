using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models.Enums;

namespace PlanShare.App.ViewModels.Popups.File;

public partial class OptionsForProfilePhotoViewModel(IPopupService popupService) : ViewModelBaseForPopups
{

    [RelayCommand]
    public async Task OptionSelected(ChooseFileOption option) => await popupService.ClosePopupAsync(Shell.Current, option);
    

    [RelayCommand]
    public async Task Cancel() => await popupService.ClosePopupAsync(Shell.Current, ChooseFileOption.None);
}
