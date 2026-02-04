using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlanShare.App.Navigation;
using System.Collections.ObjectModel;

namespace PlanShare.App.ViewModels.Pages.Errors;

public partial class ErrorsViewModel(INavigationService navigationService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial ObservableCollection<string> ErrorsList { get; set; } = [];

    [RelayCommand]
    public async Task Close() => await navigationService.GoToAsync("..");

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            var errorsList = query["errors"] as IList<string>;
            
            if (errorsList is not null)
                ErrorsList = new ObservableCollection<string>(errorsList);
        }
    }
}
