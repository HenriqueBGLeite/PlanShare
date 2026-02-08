using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
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

        ConnectionCode = result.Response!;

        StatusPage = ConnectionByCodeStatusPage.WaitingForJoiner;
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
        await _connection.InvokeAsync("ConfirmCodeJoin", ConnectionCode);

        await _connection.StopAsync();

        await _navigationService.ClosePage();
    }

    private void OnUserJoined(ResponseConnectionUserJson response)
    {
        JoinerUser = new JoinerUser
        {
            Name = response.Name
        };

        StatusPage = ConnectionByCodeStatusPage.JoinerConnectedPendingApproval;
    }
}
