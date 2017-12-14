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
    public class AddNewUser : Activity
    {
        UserAccountRuntime UAR;
        UserAccountRuntimeDB UARDB;
        Toast toast;
        EditText Username;
        EditText etPassword;
        EditText etConfPassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddNewUser);
            Username = FindViewById<EditText>(Resource.Id.NewUserUsername);
            etPassword = FindViewById<EditText>(Resource.Id.NewUsernamePassword);
            etConfPassword = FindViewById<EditText>(Resource.Id.NewUsernameConfirmPassword);
            Button btnSave = FindViewById<Button>(Resource.Id.btnSaveNewUser);
            btnSave.Click += delegate
            {
                // Create your application here

                if (etConfPassword.Text != etPassword.Text)
                {
                    toast = Toast.MakeText(this, "Passwords do not match", ToastLength.Long);
                    etConfPassword.Text = "";
                    toast.Show();
                }
                else
                {
                    UARDB = new UserAccountRuntimeDB();
                    UAR = new UserAccountRuntime();
                    UAR.Username = Username.Text;
                    UAR.Password = HashPassword.Hash(etPassword.Text);
                    UARDB.Save(UAR);
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.SingleTop);
                    StartActivity(intent);
                    Finish();
                }
            };
        }
    }
}