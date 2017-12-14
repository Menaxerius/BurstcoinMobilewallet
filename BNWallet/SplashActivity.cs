using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BNWallet
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Icon = "@drawable/BNWalletNewLogo")]
    public class SplashActivity : Activity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {

            await Task.Delay(1000); // Simulate a bit of startup work.

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}