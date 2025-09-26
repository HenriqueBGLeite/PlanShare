
using PlanShare.App.ViewModels.Popups;

namespace PlanShare.App.Navigation;

public interface INavigationService
{
    Task ClosePage();
    Task GoToAsync(ShellNavigationState state);
    Task GoToAsync(ShellNavigationState state, Dictionary<string, object> parameters);
    Task GoToDashboardPage();
    Task<TResult> ShowPopup<TViewModel, TResult>() 
        where TViewModel : ViewModelBaseForPopups 
        where TResult : notnull;
    Task ShowSuccessFeedback(string message);
}
