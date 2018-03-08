using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Content;
using Android.App;

namespace PS4_Tool__Android
{
    [Serializable]
    public class Userfiles
    {
        public string IPAdress { get; set; }

        public void Save()
        {
            var prefs = Application.Context.GetSharedPreferences("PS4 Tool", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("IP Adress", IPAdress);
            prefEditor.Commit();
        }

        public void Load()
        {
            var prefs = Application.Context.GetSharedPreferences("PS4 Tool", FileCreationMode.Private);
            var somePref = prefs.GetString("IP Adress", null);

            IPAdress = somePref;
        }
    }
}
