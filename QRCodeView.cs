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
using Android.Graphics;
using ZXing;
using ZXing.Common;

namespace BNWallet
{
    [Activity(Theme = "@style/MyTheme.BNWallet")]
    public class QRCodeView : Activity
    {
        string burstAddress;
        TextView QRBurstAddress;
        RuntimeVar RT;
        UserAccounts UA;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QRCodeView);

            ImageView view = FindViewById<ImageView>(Resource.Id.qrCodeView);
            

            
            RuntimeVarDB RTDB = new RuntimeVarDB();
            RT = RTDB.Get();           
            UserAccountsDB UADB = new UserAccountsDB();
            UA = UADB.Get(RT.CurrentWalletName);

            QRBurstAddress = FindViewById<TextView>(Resource.Id.txtQRBurstAddress);

            
            QRBurstAddress.Text = UA.BurstAddress;
            view.SetImageBitmap(GetQRCode());

            Button OK = FindViewById<Button>(Resource.Id.btnOK);
            OK.Click += delegate
            {

                Intent intent = new Intent();
                Finish();
            };


        }
        private Bitmap GetQRCode()
        {
            var writer = new ZXing.Mobile.BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 600,
                    Width = 600
                }
            };
            return writer.Write(QRBurstAddress.Text);
            // Create your application here
        }
    }
}