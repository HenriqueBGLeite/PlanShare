using PlanShare.App.ViewModels.Pages.Errors;

namespace PlanShare.App.Views.Errors;

public partial class ErrorsPage : ContentPage
{
	public ErrorsPage(ErrorsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}