using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.App.UseCases.Authentication.Refresh;
using PlanShare.Communication.Responses;

namespace PlanShare.App.ViewModels.Pages.User.Connection;

[QueryProperty(nameof(Code), "Code")]
public partial class UserConnectionJoinerViewModel(INavigationService navigationService,
    IUseRefreshTokenUseCase useRefreshTokenUseCase,
    IUserConnectionByCodeClient userConnectionByCodeClient) : ViewModelBase(navigationService)
{
    private readonly HubConnection _connection = userConnectionByCodeClient.CreateClient();

    public string Code { get; set; } = string.Empty;

    [ObservableProperty]
    public new partial ConnectionByCodeStatusPage StatusPage { get; set; }

    [ObservableProperty]
    public partial string GeneratedBy { get; set; } = string.Empty;


    [RelayCommand]
    public async Task Initialize()
    {
        StatusPage = ConnectionByCodeStatusPage.WaitingForJoiner;

        //Codigo temporário
        await useRefreshTokenUseCase.Execute();

        await _connection.StartAsync();

        var result = await _connection.InvokeAsync<HubOperationResult<string>>("JoinWithCode", Code);

        GeneratedBy = result.Response!;

        StatusPage = ConnectionByCodeStatusPage.JoinerConnectedPendingApproval;
    }
}
