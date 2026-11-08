// Platforms/Android/CustomWebChromeClient.cs
//
// Grants permission requests (camera, microphone) made by web content
// inside the Android WebView — e.g. when a page calls
// navigator.mediaDevices.getUserMedia({ video: true, audio: true })
//
// This only grants the request AT THE WEBVIEW LEVEL. The app must
// ALSO hold the equivalent Android runtime permissions
// (Camera, Microphone) — see MainActivity.cs.

using Android.Webkit;
using Android.Util;

namespace FirstAppTest
{
    public class CustomWebChromeClient : WebChromeClient
    {
        private const string Tag = "HuddleWebView";

        public override void OnPermissionRequest(PermissionRequest? request)
        {
            if (request == null) return;

            Log.Debug(Tag, $"Permission request from origin: {request.Origin}");

            // FIX: Force the permission grant operation onto the native Android UI thread
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                request.Grant(request.GetResources());
                Log.Debug(Tag, "Permission successfully granted at WebView level.");
            });
        }

        public override void OnShowCustomView(Android.Views.View? view, ICustomViewCallback? callback)
        {
            base.OnShowCustomView(view, callback);
        }

        public override void OnHideCustomView()
        {
            base.OnHideCustomView();
        }
    }
}
