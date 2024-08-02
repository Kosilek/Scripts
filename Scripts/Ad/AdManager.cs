using DredPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using System;

namespace Kosilek.Ad
{
    public class AdManager : SimpleSingleton<AdManager>
    {
        public bool isNoAd = false;
        [HideInInspector] public Action action;

        #region Start
        private void Start()
        {
            InitAppodeal();
            SomeMethodInterstitial();
            SomeMethodRewardedVideo();
            SomeMethodBanner();
            Appodeal.Show(AppodealShowStyle.BannerBottom);
        }

        private void InitAppodeal()
        {
            int adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner | AppodealAdType.RewardedVideo;
            string appKey = "7b1ba10c69b5838d370c2f86d6228fad962da7016ce89050";
            AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
            Appodeal.Initialize(appKey, adTypes);
        }

        public void OnInitializationFinished(object sender, SdkInitializedEventArgs e) { }

        #region InterstitialAd Callbacks
        private void SomeMethodInterstitial()
        {
            AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
            AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
            AppodealCallbacks.Interstitial.OnShown += OnInterstitialShown;
            AppodealCallbacks.Interstitial.OnShowFailed += OnInterstitialShowFailed;
            AppodealCallbacks.Interstitial.OnClosed += OnInterstitialClosed;
            AppodealCallbacks.Interstitial.OnClicked += OnInterstitialClicked;
            AppodealCallbacks.Interstitial.OnExpired += OnInterstitialExpired;
        }

        // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
        private void OnInterstitialLoaded(object sender, AdLoadedEventArgs e)
        {
            Debug.Log("Interstitial loaded");
        }

        // Called when interstitial failed to load
        private void OnInterstitialFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("Interstitial failed to load");
        }

        // Called when interstitial was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        private void OnInterstitialShowFailed(object sender, EventArgs e)
        {
            Debug.Log("Interstitial show failed");
        }

        // Called when interstitial is shown
        private void OnInterstitialShown(object sender, EventArgs e)
        {
            Debug.Log("Interstitial shown");
        }

        // Called when interstitial is closed
        private void OnInterstitialClosed(object sender, EventArgs e)
        {
            Debug.Log("Interstitial closed");
        }

        // Called when interstitial is clicked
        private void OnInterstitialClicked(object sender, EventArgs e)
        {
            Debug.Log("Interstitial clicked");
        }

        // Called when interstitial is expired and can not be shown
        private void OnInterstitialExpired(object sender, EventArgs e)
        {
            Debug.Log("Interstitial expired");
        }

        #endregion
        #region RewardedVideoAd Callbacks
        private void SomeMethodRewardedVideo()
        {
            AppodealCallbacks.RewardedVideo.OnLoaded += OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad += OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShown += OnRewardedVideoShown;
            AppodealCallbacks.RewardedVideo.OnShowFailed += OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnClicked += OnRewardedVideoClicked;
            AppodealCallbacks.RewardedVideo.OnExpired += OnRewardedVideoExpired;
        }

        //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
        private void OnRewardedVideoLoaded(object sender, AdLoadedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoLoaded(bool isPrecache:{e.IsPrecache})");
        }

        // Called when rewarded video failed to load
        private void OnRewardedVideoFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoFailedToLoad()");
        }

        // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, etc.)
        private void OnRewardedVideoShowFailed(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShowFailed()");
        }

        // Called when rewarded video is shown
        private void OnRewardedVideoShown(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShown()");
        }

        // Called when rewarded video is closed
        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e)
        {
            //Debug.Log($"[APDUnity] [Callback] OnRewardedVideoClosed(bool finished:{e.Finished})");
            action?.Invoke();
            action = null;
        }

        // Called when rewarded video is viewed until the end
        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoFinished(double amount:{e.Amount}, string name:{e.Currency})");
        }

        // Called when rewarded video is clicked
        private void OnRewardedVideoClicked(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoClicked()");
        }

        //Called when rewarded video is expired and can not be shown
        private void OnRewardedVideoExpired(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoExpired()");
        }

        #endregion
        #region BannerAd Callbacks
        private void SomeMethodBanner()
        {
            AppodealCallbacks.Banner.OnLoaded += OnBannerLoaded;
            AppodealCallbacks.Banner.OnFailedToLoad += OnBannerFailedToLoad;
            AppodealCallbacks.Banner.OnShown += OnBannerShown;
            AppodealCallbacks.Banner.OnShowFailed += OnBannerShowFailed;
            AppodealCallbacks.Banner.OnClicked += OnBannerClicked;
            AppodealCallbacks.Banner.OnExpired += OnBannerExpired;
        }

        // Called when a banner is loaded (height arg shows banner's height, precache arg shows if the loaded ad is precache
        private void OnBannerLoaded(object sender, BannerLoadedEventArgs e)
        {
            Debug.Log("Banner loaded");
        }

        // Called when banner failed to load
        private void OnBannerFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("Banner failed to load");
        }

        // Called when banner failed to show
        private void OnBannerShowFailed(object sender, EventArgs e)
        {
            Debug.Log("Banner show failed");
        }

        // Called when banner is shown
        private void OnBannerShown(object sender, EventArgs e)
        {
            Debug.Log("Banner shown");
        }

        // Called when banner is clicked
        private void OnBannerClicked(object sender, EventArgs e)
        {
            Debug.Log("Banner clicked");
        }

        // Called when banner is expired and can not be shown
        private void OnBannerExpired(object sender, EventArgs e)
        {
            Debug.Log("Banner expired");
        }

        #endregion
        #endregion end Start

        #region Interstitial
        private bool IsInterstitialReady()
        {
            return Appodeal.IsLoaded(AppodealAdType.Interstitial);
        }

        public void ShowInterstitial()
        {
            if (isNoAd)
                return;

            if (IsInterstitialReady())
                Appodeal.Show(AppodealShowStyle.Interstitial);
        }
        #endregion end InitializeInterstitial
        #region RewardedVideo
        private bool IsRewardedVideoReady()
        {
            return Appodeal.IsLoaded(AppodealAdType.RewardedVideo);
        }

        public void ShowRewardedVideo()
        {
            if (isNoAd)
                return;

            if (IsRewardedVideoReady())
                Appodeal.Show(AppodealShowStyle.RewardedVideo);
        }

        public void ShowRewardedVideoButton()
        {
            if (IsRewardedVideoReady())
                Appodeal.Show(AppodealShowStyle.RewardedVideo);
        }
        #endregion end RewardedVideo
    }
}