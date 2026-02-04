using CommunityToolkit.Mvvm.ComponentModel;
using PlanShare.App.Data.Storage.Preferences.User;

namespace PlanShare.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel(IUserStorage userStorage) : ViewModelBase
{
    [ObservableProperty]
    public partial string UserName { get; set; } = userStorage.Get().Name;
}
