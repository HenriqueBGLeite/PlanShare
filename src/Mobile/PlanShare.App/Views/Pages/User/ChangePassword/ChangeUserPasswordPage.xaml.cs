using PlanShare.App.ViewModels.Pages.User.ChangePassword;

namespace PlanShare.App.Views.Pages.User.ChangePassword;

public partial class ChangeUserPasswordPage : ContentPage
{
	public ChangeUserPasswordPage(ChangeUserPasswordViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}