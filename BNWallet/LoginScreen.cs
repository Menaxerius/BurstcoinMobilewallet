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
    public class LoginScreen : Activity
    {
        BNWalletAPI BNWAPI;
        EditText SecretPhrase;
        Toast toast;
        UserAccountRuntime UAR;
        UserAccountRuntimeDB UARDB;
        UserAccounts UA;
        UserAccountsDB UADB;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginScreen);

            RuntimeVar RT = new RuntimeVar();
            RuntimeVarDB RTDB = new RuntimeVarDB();
            RT = RTDB.Get();

            
            UARDB = new UserAccountRuntimeDB();
            UAR = UARDB.Get();
            string password = UAR.Password;
            string SecretPhrase = StringCipher.Decrypt(RT.CurrentPassphrase, password); 
            
                

            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += delegate
            {
                BNWAPI = new BNWalletAPI();
                GetAccountIDResult gair = BNWAPI.getAccountID(SecretPhrase,"");
                if(gair.success)
                {
                    GetAccountResult gar = BNWAPI.getAccount(gair.accountRS);
                    if(gar.success)
                    {
                        Intent intent = new Intent(this, typeof(InfoScreen));
                        intent.PutExtra("WalletAddress", gar.accountRS);
                        intent.PutExtra("WalletName", gar.name);
                        intent.PutExtra("WalletBalance", gar.balanceNQT);
                        intent.SetFlags(ActivityFlags.SingleTop);
                        StartActivity(intent);
                        
                    }
                    else
                    {
                        
                        UADB = new UserAccountsDB();
                        UA = UADB.Get(RT.CurrentWalletName);
                        Intent intent = new Intent(this, typeof(InfoScreen));
                        intent.PutExtra("WalletAddress", UA.BurstAddress);
                        intent.PutExtra("WalletName", UA.AccountName);
                        intent.PutExtra("WalletBalance", "0.00000000");
                        intent.SetFlags(ActivityFlags.SingleTop);
                        StartActivity(intent);
                    }
                }
                else
                {
                    UADB = new UserAccountsDB();
                    UA = UADB.Get(RT.CurrentWalletName);
                    Intent intent = new Intent(this, typeof(InfoScreen));
                    intent.PutExtra("WalletAddress", UA.BurstAddress);
                    intent.PutExtra("WalletName", UA.AccountName);
                    intent.PutExtra("WalletBalance", "0.00000000");
                    intent.SetFlags(ActivityFlags.SingleTop);
                    StartActivity(intent);
                }
                
            };

            

            Button btnLoadWallet = FindViewById<Button>(Resource.Id.btnLoadWallet);
            btnLoadWallet.Click += delegate
            {
                Intent intent = new Intent(this, typeof(WalletSelector));
                intent.SetFlags(ActivityFlags.SingleTop);
                StartActivity(intent);
                Finish();
            };



            // Create your application here
        }
    }
}