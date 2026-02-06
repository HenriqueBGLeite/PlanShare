using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PlanShare.App.Data.Network.Api;
using PlanShare.App.Models;
using PlanShare.App.Navigation;
using PlanShare.Communication.Responses;

namespace PlanShare.App.ViewModels.Pages.User.Connection;

public partial class UserConnectionGeneratorViewModel(INavigationService navigationService,
    IUserConnectionByCodeClient userConnectionByCodeClient) : ViewModelBase(navigationService)
{
    private readonly HubConnection _connection = userConnectionByCodeClient.CreateClient();

    [ObservableProperty]
    public new partial ConnectionByCodeStatusPage StatusPage { get; set; }

    [ObservableProperty]
    public partial string? ConnectionCode { get; set; }

    [RelayCommand]
    public async Task Initialize()
    {
        StatusPage = ConnectionByCodeStatusPage.GeneratingCode;

        await _connection.StartAsync();

        var result = await _connection.InvokeAsync<HubOperationResult<string>>("GenerateCode");

        ConnectionCode = result.Response!;

        StatusPage = ConnectionByCodeStatusPage.WaitingForJoiner;
    }

    [RelayCommand]
    public async Task Cancel() => await _navigationService.ClosePage();
}
