
namespace PlanShare.App.Navigation;

public class NavigationService : INavigationService
{
    public async Task GoToAsync(ShellNavigationState state) => await Shell.Current.GoToAsync(state);
    public async Task GoToAsync(ShellNavigationState state, Dictionary<string, object> parameters) 
        => await Shell.Current.GoToAsync(state, parameters);
}
