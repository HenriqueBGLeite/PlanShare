using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.Resources;
using PlanShare.App.UseCases.Authentication.Refresh;
using PlanShare.Communication.Responses;

namespace PlanShare.App.ViewModels.Pages.User.Connection;

public partial class UserConnectionGeneratorViewModel : ViewModelBase
{
    private readonly HubConnection _connection;
    private readonly IUseRefreshTokenUseCase _useRefreshTokenUseCase;

    [ObservableProperty]
    public new partial ConnectionByCodeStatusPage StatusPage { get; set; }

    [ObservableProperty]
    public partial string? ConnectionCode { get; set; }

    [ObservableProperty]
    public partial JoinerUser JoinerUser { get; set; } = new();

    public UserConnectionGeneratorViewModel(INavigationService navigationService,
    IUseRefreshTokenUseCase useRefreshTokenUseCase,
    IUserConnectionByCodeClient userConnectionByCodeClient) : base(navigationService)
    {
        _connection = userConnectionByCodeClient.CreateClient();
        _useRefreshTokenUseCase = useRefreshTokenUseCase;

        _connection.On<ResponseConnectionUserJson>("OnUserJoined", OnUserJoined);
    }

    [RelayCommand]
    public async Task Initialize()
    {
        StatusPage = ConnectionByCodeStatusPage.GeneratingCode;

        //Codigo temporário
        await _useRefreshTokenUseCase.Execute();

        await _connection.StartAsync();

        var result = await _connection.InvokeAsync<HubOperationResult<string>>("GenerateCode");
        if (result.IsSuccess)
        {
            ConnectionCode = result.Response!;

            StatusPage = ConnectionByCodeStatusPage.WaitingForJoiner;
        }
        else
        {
            await _connection.StopAsync();

            await _navigationService.ClosePage();

            await _navigationService.ShowFailureFeedback(result.ErrorMessage);
        }
    }

    [RelayCommand]
    public async Task Cancel()
    {
        await _connection.InvokeAsync("Cancel", ConnectionCode);

        await _connection.StopAsync();

        await _navigationService.ClosePage();
    }

    [RelayCommand]
    public async Task Approve()
    {
        var result = await _connection.InvokeAsync<HubOperationResult<string>>("ConfirmCodeJoin", ConnectionCode);
        if (result.IsSuccess)
            await _navigationService.ShowSuccessFeedback(string.Format(ResourceTexts.USER_JOINED_SUCCESSFULLY, JoinerUser.Name));        
        else
            await _navigationService.ShowFailureFeedback(result.ErrorMessage);

        await _connection.StopAsync();

        await _navigationService.ClosePage();
    }

    private void OnUserJoined(ResponseConnectionUserJson response)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            JoinerUser = new JoinerUser
            {
                Name = response.Name
            };

            StatusPage = ConnectionByCodeStatusPage.JoinerConnectedPendingApproval;
        });
    }
}
