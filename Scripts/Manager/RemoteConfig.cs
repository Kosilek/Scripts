using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using System;

public class RemoteConfigq : MonoBehaviour
{
    public string version;
    public string realVersion;
    public CanvasUpdatePliz canvasUpdatePliz;
    public struct userAttributes { }

    public struct appAttribute { }

    struct EmptyStruct { }

    private void Awake()
    {
        FetchRemoteConfiguration();
    }

    async public void FetchRemoteConfiguration()
    {
        RuntimeConfig res;
        try
        {
            // Initialize 
            await UnityServices.InitializeAsync();

            // Sign in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //Fetch configuration settings from the remote service:
            res = await RemoteConfigService.Instance.FetchConfigsAsync(new EmptyStruct(), new EmptyStruct());

            Debug.Log(res.config);

            version = Application.version;
            realVersion = res.GetString("version");
            if (version != realVersion)
                canvasUpdatePliz.windwo.Open();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("Failed to fetch configs, is your project linked and configuration deployed?");
        }
    }

    private void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                break;

            case ConfigOrigin.Cached:
                break;

            case ConfigOrigin.Remote:
              
                break;
        }
    }

    void Start()
    {

      //  LoadVersion();
        //RemoteSettings.Completed += HandleRemoteSettings;
    }

  /*  private void HandleRemoteSettings(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
    {
       
    }*/

    private void LoadVersion()
    {
        realVersion
           = RemoteSettings.GetString("version");
    }
}
