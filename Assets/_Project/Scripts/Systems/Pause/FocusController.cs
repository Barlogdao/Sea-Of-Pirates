using Agava.WebUtility;
using Project.Interfaces.SDK;
using Project.Systems.Pause;
using System;
using UnityEngine;

public class FocusController : IDisposable
{
    private readonly PauseService _pauseService;
    private readonly IAdvertismentService _advertisingService;

    public FocusController(PauseService pauseService, IAdvertismentService advertisingService)
    {
        _pauseService = pauseService;
        _advertisingService = advertisingService;

        Application.focusChanged += OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
    }

    public void Dispose()
    {
        Application.focusChanged -= OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
    }

    private void OnInBackgroundChangeApp(bool inApp)
    {
        if (_advertisingService.IsAdsPlaying == false)
            PauseGame(!inApp);
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        if (_advertisingService.IsAdsPlaying == false)
            PauseGame(isBackground);
    }

    private void PauseGame(bool isUnfocused)
    {
        if (isUnfocused)
        {
            _pauseService.Pause();
        }
        else
        {
            _pauseService.Unpause();
        }
    }
}
