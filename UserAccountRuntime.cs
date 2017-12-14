
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
    public class UserAccountRuntime
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public UserAccountRuntime()
        {
            id = 1;
            Username = "";
            Password = "";
        }
    }

    public class UserAccountRuntimeDB
    {
        private SQLiteConnection db;

        public UserAccountRuntimeDB()
        {

            string dbFileName = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "UserDB.db3");
            

            if (System.IO.File.Exists(dbFileName))
            {
                db = new SQLiteConnection(dbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            }
            else
            {
                //If the file does not exist then create the initial table.
                db = new SQLiteConnection(dbFileName);
            }
                db.CreateTable<UserAccounts>();
                db.CreateTable<RuntimeVar>();
                db.CreateTable<UserAccountRuntime>();

            
        }

        public UserAccountRuntime Get()
        {
            var data = db.Table<UserAccountRuntime>();
            var RT = data.Where(s => s.id == 1).FirstOrDefault();
            return RT;
        }

        public void Save(UserAccountRuntime RT)
        {
            db.InsertOrReplace(RT);
        }

    }
}
   
