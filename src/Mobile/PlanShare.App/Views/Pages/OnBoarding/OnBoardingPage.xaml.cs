using PlanShare.App.ViewModels.Pages.OnBoarding;

namespace PlanShare.App.Views.Pages.OnBoarding;

public partial class OnBoardingPage : ContentPage
{
	public OnBoardingPage(OnBoardingViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}