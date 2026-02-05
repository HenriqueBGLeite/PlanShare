
namespace PlanShare.App.Navigation;

public interface INavigationService
{
    Task ClosePage();
    Task GoToAsync(ShellNavigationState state);
    Task GoToAsync(ShellNavigationState state, Dictionary<string, object> parameters);
    Task GoToDashboardPage();
    Task ShowSuccessFeedback(string message);
}
