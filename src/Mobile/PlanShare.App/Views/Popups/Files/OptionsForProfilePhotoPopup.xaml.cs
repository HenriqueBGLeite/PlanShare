using CommunityToolkit.Maui.Views;
using PlanShare.App.Extensions;
using PlanShare.App.ViewModels.Popups.Files;

namespace PlanShare.App.Views.Popups.Files;

public partial class OptionsForProfilePhotoPopup : Popup
{
	public OptionsForProfilePhotoPopup(OptionsForProfilePhotoViewModel model, IDeviceDisplay deviceDisplay)
	{
		InitializeComponent();
		BindingContext = model;

		WidthRequest = deviceDisplay.GetWidthForPopup();
	}
}