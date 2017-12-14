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
    public class AddNewPassphrase : Activity
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
            SetContentView(Resource.Layout.AddNewPassphrase);

            etPassphrase = FindViewById<EditText>(Resource.Id.NewGenWalletPassPhrase);

            Button GenNewPassphrase = FindViewById<Button>(Resource.Id.btnNewPassphraseGenerator);
            GenNewPassphrase.Click += delegate
            {
                etPassphrase.Text = CreatePassword(200);

            };


            Button Save = FindViewById<Button>(Resource.Id.btnSaveNewPassphraseWallet);
            Save.Click += delegate
            {

                BNWAPI = new BNWalletAPI();
                GetAccountIDResult gair = BNWAPI.getAccountID(etPassphrase.Text, "");
                if (gair.success)
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
                   



                };



                // Create your application here
            };
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }

    // Create your application here

}