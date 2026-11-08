// Platforms/Android/MainActivity.cs
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.Activity;
using System;

namespace FirstAppTest
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = global::Android.Content.PM.LaunchMode.SingleTop,
        ConfigurationChanges = global::Android.Content.PM.ConfigChanges.ScreenSize | global::Android.Content.PM.ConfigChanges.Orientation | global::Android.Content.PM.ConfigChanges.UiMode | global::Android.Content.PM.ConfigChanges.ScreenLayout | global::Android.Content.PM.ConfigChanges.SmallestScreenSize | global::Android.Content.PM.ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private const int PermissionRequestCode = 1001;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestMediaPermissions();

            // Register the smart gesture interceptor framework
            OnBackPressedDispatcher.AddCallback(this, new WebBackPressCallback(this, true));
        }

        public Android.Webkit.WebView? FindActiveWebView(Android.Views.View? view)
        {
            if (view == null) return null;
            if (view is Android.Webkit.WebView webView) return webView;
            if (view is ViewGroup group)
            {
                for (int i = 0; i < group.ChildCount; i++)
                {
                    var result = FindActiveWebView(group.GetChildAt(i));
                    if (result != null) return result;
                }
            }
            return null;
        }

        private void RequestMediaPermissions()
        {
            var permissionsNeeded = new System.Collections.Generic.List<string>();
            if (ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.Camera) != Permission.Granted)
                permissionsNeeded.Add(global::Android.Manifest.Permission.Camera);
            if (ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.RecordAudio) != Permission.Granted)
                permissionsNeeded.Add(global::Android.Manifest.Permission.RecordAudio);

            if (permissionsNeeded.Count > 0)
            {
                ActivityCompat.RequestPermissions(this, permissionsNeeded.ToArray(), PermissionRequestCode);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    // ── SMART BACK-PRESS INTERCEPTOR ENGINE ──
    public class WebBackPressCallback : OnBackPressedCallback
    {
        private readonly MainActivity _activity;
        private long _lastBackPressTime = 0;
        private const long DoublePressInterval = 2000; // Time window in milliseconds (2 seconds)

        public WebBackPressCallback(MainActivity activity, bool enabled) : base(enabled)
        {
            _activity = activity;
        }

        public override void HandleOnBackPressed()
        {
            var webView = _activity.FindActiveWebView(_activity.Window?.DecorView);
            if (webView == null)
            {
                RunDefaultSystemExit();
                return;
            }

            // Step 1: Call window.handlePageBack() and pass the return value through
            // directly so we can distinguish three states:
            //   'sheet_closed' — a sheet was open and has been closed, stop here
            //   'at_home'      — page is the root screen, skip canGoBack(), double-tap to exit
            //   'fallback'     — page has no handler, use standard back navigation
            webView.EvaluateJavascript(@"
                (function() {
                    if (window.handlePageBack) {
                        return String(window.handlePageBack());
                    }
                    return 'fallback';
                })()", new JavaScriptResultCallback(result =>
            {
                // Android wraps JS string results in quotes — Contains() handles this safely.

                if (result != null && result.Contains("sheet_closed"))
                {
                    // A sheet was open and has been closed. Back navigation fully handled.
                    return;
                }

                if (result != null && result.Contains("at_home"))
                {
                    // Page signals it is the root/home screen.
                    // Skip canGoBack() entirely — go straight to double-tap exit logic.
                    HandleDoubleTapExit();
                    return;
                }

                // Step 2: 'fallback' — standard WebView back navigation if possible.
                _activity.RunOnUiThread(() =>
                {
                    if (webView.CanGoBack())
                    {
                        webView.GoBack();
                    }
                    else
                    {
                        // Step 3: Root of back stack — double-tap to exit.
                        HandleDoubleTapExit();
                    }
                });
            }));
        }

        private void HandleDoubleTapExit()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (currentTime - _lastBackPressTime < DoublePressInterval)
            {
                // Second tap within 2 seconds: bypass tracking and minimize app
                RunDefaultSystemExit();
            }
            else
            {
                // First tap: Update threshold timestamp and flash toast notification window
                _lastBackPressTime = currentTime;
                Android.Widget.Toast.MakeText(_activity, "Press back again to exit", Android.Widget.ToastLength.Short)?.Show();
            }
        }

        private void RunDefaultSystemExit()
        {
            Enabled = false;
            _activity.OnBackPressedDispatcher.OnBackPressed();
            Enabled = true;
        }
    }

    // Simple callback tracker to receive evaluated values back from native JavaScript execution loops
    public class JavaScriptResultCallback : Java.Lang.Object, IValueCallback
    {
        private readonly Action<string> _action;
        public JavaScriptResultCallback(Action<string> action) => _action = action;
        public void OnReceiveValue(Java.Lang.Object? value) => _action?.Invoke(value?.ToString() ?? "");
    }
}