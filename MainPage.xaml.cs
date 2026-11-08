namespace FirstAppTest
{
    public partial class MainPage : ContentPage
    {

        // Handle incoming messages from JS
        void OnMessageReceived(object sender,
            HybridWebViewRawMessageReceivedEventArgs e)
        {
            Console.WriteLine("Got: " + e.Message);
        }


        public MainPage()
        {
            InitializeComponent();

            // Registers this C# class as a target so JavaScript can call native methods
            hwv.SetInvokeJavaScriptTarget(this);
        }

        protected override bool OnBackButtonPressed()
        {
            // Always dispatch web view updates to the main UI thread
            Dispatcher.Dispatch(async () =>
            {
                try
                {
                    // 1. Evaluate the current URL path directly (completely bypassing the wrapper)
                    string rawPath = await hwv.EvaluateJavaScriptAsync("window.location.pathname");

                    // Clean up the string (platforms often wrap the evaluated result in extra quotes)
                    string currentPath = rawPath?.Trim('"', '\'');

                    // 2. Decide if we are at a root screen or a nested screen
                    if (string.IsNullOrEmpty(currentPath) ||
                        currentPath.EndsWith("index.html") ||
                        currentPath.EndsWith("dashboard.html") ||
                        currentPath == "/")
                    {
                        // If the user is on the main dashboard/login index, exit the application
                        CloseApplication();
                    }
                    else
                    {
                        // If they are on a subpage (like auth.html), tell the browser history to step backward
                        await hwv.EvaluateJavaScriptAsync("window.history.back();");
                    }
                }
                catch
                {
                    // Fallback safety: if script valuation breaks down, gracefully exit the app
                    CloseApplication();
                }
            });

            // Return true to tell Android that we have completely intercepted and handled the button press
            return true;
        }

        // This method must be public so the HybridWebView JavaScript bridge can invoke it
        public void CloseApplication()
        {
#if ANDROID
            // Cleanly minimizes the app or finishes the active native Android activity
            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Finish();
#else
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
#endif
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {

        }
    }
}
