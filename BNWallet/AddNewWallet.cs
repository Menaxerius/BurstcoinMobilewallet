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
    public class AddNewWallet : Activity
    {
        EditText etPassphrase;
       

        BNWalletAPI BNWAPI;
        Toast toast;
        UserAccounts UA;
        UserAccountsDB UADB;
        UserAccountRuntime UAR;
        UserAccountRuntimeDB UARDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddNewWallet);

            etPassphrase = FindViewById<EditText>(Resource.Id.NewWalletPassPhrase);

            
            Button Save = FindViewById<Button>(Resource.Id.btnSaveNewWallet);
            Save.Click += delegate
            {
                
                BNWAPI = new BNWalletAPI();
                GetAccountIDResult gair = BNWAPI.getAccountID(etPassphrase.Text, "");
                if (gair.success)
                {
                    GetAccountResult gar = BNWAPI.getAccount(gair.accountRS);
                    if (gar.success)
                    {
                        UADB = new UserAccountsDB();
                        UA = UADB.Get(gar.name);
                        if(UA != null)
                        {
                            toast = Toast.MakeText(this, "Wallet Already Exists: " + UA.AccountName, ToastLength.Long);
                            toast.Show();
                        }
                        else
                        {
                            UARDB = new UserAccountRuntimeDB();
                            UAR = UARDB.Get();
                            string password = UAR.Password;
                            UA = new UserAccounts();
                            string plaintext = etPassphrase.Text;
                            string encryptedstring = StringCipher.Encrypt(plaintext, password);
                            UA.AccountName = gar.name;
                            UA.BurstAddress = gar.accountRS;
                            UA.PassPhrase = encryptedstring;
                            UADB.Save(UA);
                            Intent intent = new Intent(this, typeof(WalletSelector));
                            intent.SetFlags(ActivityFlags.SingleTop);
                            StartActivity(intent);
                            Finish();
                        }
                    }
                    else
                    {
                        
                        UADB = new UserAccountsDB();
                        UARDB = new UserAccountRuntimeDB();
                        UAR = UARDB.Get();
                        string password = UAR.Password;
                        UA = new UserAccounts();
                        string plaintext = etPassphrase.Text;
                        string encryptedstring = StringCipher.Encrypt(plaintext, password);
                        UA.AccountName = "Unknown Account";
                        UA.BurstAddress = gair.accountRS;
                        UA.PassPhrase = encryptedstring;
                        UADB.Save(UA);
                        Intent intent = new Intent(this, typeof(WalletSelector));
                        intent.SetFlags(ActivityFlags.SingleTop);
                        StartActivity(intent);
                        Finish();
                    }
                }
                else
                {
                    toast = Toast.MakeText(this, "Received API Error: " + gair.errorMsg, ToastLength.Long);
                    toast.Show();
                }
                

            };

            

            // Create your application here
        }
        
    }
}