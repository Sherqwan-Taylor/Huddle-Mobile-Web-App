// Platforms/Android/CustomWebViewHandler.cs
using Android.Webkit;
using Microsoft.Maui.Handlers;
using AWebView = Android.Webkit.WebView;

namespace FirstAppTest
{
    // CHANGE: Inherit from HybridWebViewHandler instead of WebViewHandler
    public class CustomWebViewHandler : HybridWebViewHandler
    {
        protected override void ConnectHandler(AWebView platformView)
        {
            base.ConnectHandler(platformView);

            // Force GPU hardware rendering layer for the WebView
            platformView.SetLayerType(Android.Views.LayerType.Hardware, null);

            // Retain your WebRTC optimizations
            platformView.Settings.JavaScriptEnabled = true;
            platformView.Settings.DomStorageEnabled = true;
            platformView.Settings.MediaPlaybackRequiresUserGesture = false;
            platformView.Settings.AllowFileAccess = true;
            platformView.Settings.AllowContentAccess = true;
            platformView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

            // Securely attach your custom permission engine
            platformView.SetWebChromeClient(new CustomWebChromeClient());
        }
    }
}