
namespace PlanShare.App.Navigation;

public interface INavigationService
{
    Task GoToAsync(ShellNavigationState state);
    Task GoToAsync(ShellNavigationState state, Dictionary<string, object> parameters);
}
