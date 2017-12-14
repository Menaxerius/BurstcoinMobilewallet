using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace BNWallet
{
    [Activity(Theme = "@style/MyTheme.BNWallet")]
    public class WalletSelector : Activity
    {
        ListView UserAccountView;
        List<UserAccounts> items;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.WalletSelector);
            UserAccountView = FindViewById<ListView>(Resource.Id.lstUserAccounts);

            UserAccountsDB userAccountDB = new UserAccountsDB();
           
            UserAccounts[] userAccount = userAccountDB.GetAccountList();

            items = userAccount.ToList<UserAccounts>();

            UserAccountsViewAdapter adapter = new UserAccountsViewAdapter(this, items);
            UserAccountView.Adapter = adapter;
            UserAccountView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e)
            {
                RuntimeVarDB RTDB = new RuntimeVarDB();
                RuntimeVar RT = new RuntimeVar();
                RT.CurrentWalletName = items[e.Position].AccountName;
                UserAccounts UA = new UserAccounts();
                UA = userAccountDB.Get(items[e.Position].AccountName);
                RT.CurrentPassphrase = UA.PassPhrase;               
                RTDB.Save(RT);
                Intent intent = new Intent(this, typeof(LoginScreen));
                StartActivity(intent);
                Finish();
            };

            Button AddWallet = FindViewById<Button>(Resource.Id.btnAddNewWallet);
            AddWallet.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AddNewWallet));
                intent.SetFlags(ActivityFlags.SingleTop);
                StartActivity(intent);
                Finish();
            };

            Button btnNewUser = FindViewById<Button>(Resource.Id.btnNewUser);
            btnNewUser.Click += delegate
            {
                var uri = Android.Net.Uri.Parse("https://wallet1.burstnation.com:8125/index.html");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
                Finish();
            };

            // Create your application here
        }
    }
}