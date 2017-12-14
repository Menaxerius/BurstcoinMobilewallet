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

namespace BNWallet
{
    class UserAccountsViewAdapter : BaseAdapter<UserAccounts>
    {
        private List<UserAccounts> mItems;
        private Context mContext;

        public UserAccountsViewAdapter(Context context,List<UserAccounts> items)
        {
            mContext = context;
            mItems = items;
        }


        public override long GetItemId(int position)
        {
            return position;
        }
        public override UserAccounts this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.WalletSelectorRow, null, false);
            }
            TextView txtWalletName = row.FindViewById<TextView>(Resource.Id.textWalName);
            txtWalletName.Text = mItems[position].AccountName;
            txtWalletName.SetTextColor(Color.White);
            TextView txtBurstAddress = row.FindViewById<TextView>(Resource.Id.textWalAddress);
            txtBurstAddress.Text = mItems[position].BurstAddress;
            txtBurstAddress.SetTextColor(Color.White);
            

            if (position % 2 == 0)

                row.SetBackgroundColor(Color.DarkGray);

            else
                row.SetBackgroundColor(Color.DarkGray);

            return row;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

    }
}