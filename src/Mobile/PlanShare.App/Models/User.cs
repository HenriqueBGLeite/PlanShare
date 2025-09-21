using CommunityToolkit.Mvvm.ComponentModel;

namespace PlanShare.App.Models;

public partial class User : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }
    public string Email { get; set; } = string.Empty;
}
