using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Shapes;
using PlanShare.App.Constants;
using PlanShare.App.Extensions;
using PlanShare.App.Resources;
using PlanShare.App.ViewModels.Popups;

namespace PlanShare.App.Navigation;

public class NavigationService(IPopupService popupService) : INavigationService
{
    public async Task GoToAsync(ShellNavigationState state) => await Shell.Current.GoToAsync(state);

    public async Task GoToAsync(ShellNavigationState state, Dictionary<string, object> parameters) 
        => await Shell.Current.GoToAsync(state, parameters);

    public async Task ClosePage() => await GoToAsync("..");

    public async Task GoToDashboardPage() => await GoToAsync($"//{RoutePages.DASHBOARD_PAGE}");

    public async Task GoToOnBoardingPage() => await GoToAsync($"//{RoutePages.ONBOARDING_PAGE}");

    public async Task ShowSuccessFeedback(string message)
    {
        var font = Microsoft.Maui.Font.OfSize(FontFamily.MAIN_FONT_BLACK, 14);
        var actionButtonFont = Microsoft.Maui.Font.OfSize(FontFamily.SECONDARY_FONT_REGULAR, 14);

        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Application.Current!.GetHighlightColor(),
            TextColor = Application.Current!.GetSecondaryColor(),
            CornerRadius = new CornerRadius(10),
            ActionButtonTextColor = Application.Current!.GetSecondaryColor(),
            ActionButtonFont = actionButtonFont,
            Font = font,
            CharacterSpacing = 0.01
        };

        var duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(message, action: null, actionButtonText: ResourceTexts.TITLE_CLOSE, duration, snackbarOptions);

        await snackbar.Show();
    }

    public async Task<TResult> ShowPopup<TViewModel, TResult>() 
        where TViewModel : ViewModelBaseForPopups
        where TResult : notnull
    {
        var popupOptions = new PopupOptions
        {
            Shadow = null,
            Shape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(10),
                StrokeThickness = 0
            },
            CanBeDismissedByTappingOutsideOfPopup = false
        };

        var result = await popupService.ShowPopupAsync<TViewModel, TResult>(Shell.Current, popupOptions);

        return result.Result!;
    }
}
