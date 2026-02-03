using CommunityToolkit.Mvvm.ComponentModel;
using PlanShare.App.Models;

namespace PlanShare.App.ViewModels.Pages;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    public partial StatusPage StatusPage { get; set; }

    protected ViewModelBase()
    {
        StatusPage = StatusPage.Default;
    }
}
