using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Models.Enums;

namespace PlanShare.App.ViewModels.Popups.Connection;

public partial class OptionsForConnectionByCodeViewModel(IPopupService popupService) : ViewModelBaseForPopups
{
    [RelayCommand]
    public async Task OptionSelected(ChooseCodeConnectionOption option) => await popupService.ClosePopupAsync(Shell.Current, option);
}
