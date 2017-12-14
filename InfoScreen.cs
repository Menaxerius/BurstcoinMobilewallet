using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BNWallet
{
    [Activity(Theme = "@style/MyTheme.BNWallet")]
    public class InfoScreen : Activity
    {

        BNWalletAPI BNWAPI;
        TextView WalletName;
        TextView BurstAddress;
        TextView BurstBalance;
        Button btnSendBurst;
        Button BurstRadio;
        string burstAddress;
        string walletName;
        string balance;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InfoScreen);
            burstAddress = Intent.GetStringExtra("WalletAddress");
            walletName = Intent.GetStringExtra("WalletName");
            balance = Intent.GetStringExtra("WalletBalance");


            WalletName = FindViewById<TextView>(Resource.Id.txtWalletName);
            BurstAddress = FindViewById<TextView>(Resource.Id.txtBurstAddress);
            BurstBalance = FindViewById<TextView>(Resource.Id.txtBalance);


            BurstAddress.Text = burstAddress;
            WalletName.Text = walletName;
            BurstBalance.Text = balance;
            double burstdbl = Convert.ToDouble(BurstBalance.Text);
            burstdbl = burstdbl / 100000000;
            BurstBalance.Text = burstdbl.ToString("#,0.00000000");
            

            btnSendBurst = FindViewById<Button>(Resource.Id.btnSendBurst);
            btnSendBurst.Click += delegate
            {
                Intent intent = new Intent(this, typeof(SendBurstScreen));
                intent.SetFlags(ActivityFlags.SingleTop);
                intent.PutExtra("BurstAddress", BurstAddress.Text);
                StartActivity(intent);
                

            };
            BurstRadio = FindViewById<Button>(Resource.Id.btnBurstRadio);
            BurstRadio.Click += delegate
            {
                
                var uri = Android.Net.Uri.Parse("https://www.burstnation.com/listen.html");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };


            // Create your application here
        }
    }
}