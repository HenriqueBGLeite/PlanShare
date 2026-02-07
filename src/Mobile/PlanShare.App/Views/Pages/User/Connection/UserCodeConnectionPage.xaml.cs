using PinCodes.Authorization.Views.Pages;
using PlanShare.App.ViewModels.Pages.User.Connection;

namespace PlanShare.App.Views.Pages.User.Connection;

public partial class UserCodeConnectionPage : CodePage
{
	public UserCodeConnectionPage(UserCodeConnectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}