using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace BNWallet
{
    [Activity(Theme = "@style/MyTheme.BNWallet")]
    public class MainActivity : Activity
    {

        Button Login;
        Button Register;
        EditText Username;
        EditText Password;
        UserAccountRuntime UAR;
        UserAccountRuntimeDB UARDB;
        UserAccounts UA;
        UserAccountsDB UADB;
        Toast toast;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Login = FindViewById<Button>(Resource.Id.btnNULogin);
            Register = FindViewById<Button>(Resource.Id.btnNURegister);
            Username = FindViewById<EditText>(Resource.Id.NewUserUsername);
            Password = FindViewById<EditText>(Resource.Id.NewUserPassword);

            UARDB = new UserAccountRuntimeDB();
            UAR = UARDB.Get();
            UADB = new UserAccountsDB();

            Register.Click += delegate
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
                alertDialog.SetTitle("Confirmation");
                alertDialog.SetMessage("Registering a new user clears the database. \nAre you sure?");
                alertDialog.SetPositiveButton("Yes", delegate
                {
                    UADB.ClearDB();

                    Intent Intent = new Intent(this, typeof(AddNewUser));
                    Intent.SetFlags(ActivityFlags.SingleTop);
                    StartActivity(Intent);
                    Finish();
                });
                alertDialog.SetNegativeButton("No", delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            };

            Login.Click += delegate
            {
                if (UAR != null)
                {
                    if (UAR.Username == Username.Text)
                    {
                        if (HashPassword.VerifyHashedPassword(UAR.Password,Password.Text))
                        {
                            Intent intent = new Intent(this, typeof(WalletSelector));
                            intent.SetFlags(ActivityFlags.SingleTop);
                            StartActivity(intent);
                            Finish();
                        }
                        else
                        {
                            toast = Toast.MakeText(this, "Password is incorrect", ToastLength.Long);
                            toast.Show();
                            Password.Text = "";
                        }
                    }
                    else
                    {
                        toast = Toast.MakeText(this, "Username doesn't exist, please register", ToastLength.Long);
                        toast.Show();
                    }
                }
                else
                {
                    toast = Toast.MakeText(this, "User doesn't exist, please register", ToastLength.Long);
                    toast.Show();
                }
            };
            
            


            // Set our view from the "main" layout resource
             
        }
    }
}

