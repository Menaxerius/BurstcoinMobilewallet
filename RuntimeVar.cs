
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BNWallet
{
    public class RuntimeVar
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CurrentWalletName { get; set; }
        public string CurrentPassphrase { get; set; }
        

        public RuntimeVar()
        {
            id = 1;
            CurrentWalletName = "";
            CurrentPassphrase = "";
        }
    }

    public class RuntimeVarDB
    {
        private SQLiteConnection db;

        public RuntimeVarDB()
        {


            string dbFileName = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "UserDB.db3");
            

            if (System.IO.File.Exists(dbFileName))
            {
                db = new SQLiteConnection(dbFileName);
            }
            else
            {
                //If the file does not exist then create the initial table.
                db = new SQLiteConnection(dbFileName);
                db.CreateTable<UserAccounts>();
                db.CreateTable<RuntimeVar>();

            }
        }

        public RuntimeVar Get()
        {
            var data = db.Table<RuntimeVar>();
            var RT = data.Where(s => s.id == 1).FirstOrDefault();
            return RT;
        }

        public void Save(RuntimeVar RT)
        {
            db.InsertOrReplace(RT);
        }

    }
}
