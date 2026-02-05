using CommunityToolkit.Maui.Views;
using PlanShare.App.ViewModels.Popups.File;

namespace PlanShare.App.Views.Popups.Files;

public partial class OptionsForProfilePhotoPopup : Popup
{
	public OptionsForProfilePhotoPopup(OptionsForProfilePhotoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		var screenWidht = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

		WidthRequest = screenWidht * 0.8;
    }
}