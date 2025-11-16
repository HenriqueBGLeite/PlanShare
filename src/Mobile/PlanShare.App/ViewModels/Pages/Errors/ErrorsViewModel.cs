using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;
using System.Collections.ObjectModel;

namespace PlanShare.App.ViewModels.Pages.Errors;

public partial class ErrorsViewModel : ObservableObject, IQueryAttributable
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial ObservableCollection<string> ErrorsList { get; set; } = [];

    public ErrorsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task Close() => await _navigationService.ClosePage();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            var errosList = query["errors"] as List<string>;

            if (errosList is not null)
                ErrorsList = new ObservableCollection<string>(errosList);
        }
    }
}
