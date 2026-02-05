using CommunityToolkit.Maui.Views;
using PlanShare.App.ViewModels.Popups;

namespace PlanShare.App.Views.Popups.Files;

public partial class OptionsForProfilePhotoPopup : Popup
{
	public OptionsForProfilePhotoPopup(ViewModelBaseForPopups viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}