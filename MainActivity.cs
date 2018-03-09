using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using static Android.App.ActionBar;

namespace PS4_Tool__Android
{
    [Activity(Label = "PS4 Tool", MainLauncher = true, Icon = "@drawable/ps4logo")]
    public class MainActivity : Activity
    {

        int BufferSize = 10485760;
        string modPath;
        string errorMessage;
        public string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool";
        string currentVersion;
        const int permission_Request = 101;
        bool permissionGranted = false;




        Button inject_btn;
        Spinner payload_dropDown;
        EditText ps4IPAdress;
        EditText port;

        public void SendTCP(string path, string IPAdress, Int32 Port)
            {
                byte[] SendingBuffer = null;
                TcpClient client = null;
                NetworkStream netstream = null;
                try
                {
                    client = new TcpClient(IPAdress, Port);

                    netstream = client.GetStream();
                    FileStream Fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    int NoOfPackets = Convert.ToInt32
                    (Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                    int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
                    for (int i = 0; i < NoOfPackets; i++)
                    {
                        if (TotalLength > BufferSize)
                        {
                            CurrentPacketLength = BufferSize;
                            TotalLength = TotalLength - CurrentPacketLength;
                        }
                        else
                            CurrentPacketLength = TotalLength;
                        SendingBuffer = new byte[CurrentPacketLength];
                        Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                        netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
                    }

                    Fs.Close();
                }
                catch (Exception ex)
                {
                    errorMessage = ex.ToString();
                }
                finally
                {
                    netstream.Close();
                    client.Close();
                }
            }



            protected override void OnCreate(Bundle savedInstanceState)
            {
            RequestPermissions();
            
            base.OnCreate(savedInstanceState);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            SetContentView(Resource.Layout.Main);




            Tab tab1 = ActionBar.NewTab();
            tab1.SetText("1.76");
            tab1.TabSelected += (sender, args) =>
            {
                currentVersion = tab1.Text;
                getFilesInDirectory();
            };
            ActionBar.AddTab(tab1);

            Tab tab2 = ActionBar.NewTab();
            tab2.SetText("4.05");
            tab2.TabSelected += (sender, args) =>
            {
                currentVersion = tab2.Text;
                getFilesInDirectory();
            };
            ActionBar.AddTab(tab2);

            Tab tab3 = ActionBar.NewTab();
            tab3.SetText("4.55");
            tab3.TabSelected += (sender, args) =>
            {
                currentVersion = tab3.Text;
                getFilesInDirectory();
            };
            ActionBar.AddTab(tab3);





            ps4IPAdress = FindViewById<EditText>(Resource.Id.PS4IP);
            Userfiles US = new Userfiles();
            US.Load();
            ps4IPAdress.Text = US.IPAdress;




            getFilesInDownload();
            
            //Move files from Assets to Folder
            //NOT WORKING YET

            //Version 1.76 Payloads
            using (var asset = Assets.Open("dlclose.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/1.76/dlclose.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("WebBrowserPatch.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/1.76/WebBrowserPatch.bin"))
                asset.CopyTo(dest);


            //Version 4.05 Payloads
            using (var asset = Assets.Open("DumpFile405.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.05/DumpFile405.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("EnableWebBrowser.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.05/EnableWebBrowser.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("FullDebugSettings.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.05/FullDebugSettings.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("PS4HEN.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.05/PS4HEN.bin"))
                asset.CopyTo(dest);


            //Version 4.55 Payloads
            using (var asset = Assets.Open("GameDumper.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.05/GameDumper.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("PS4HolyGrail.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.55/PS4HolyGrail.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("PermanentWebbrowser.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.55/PermanentWebbrowser.bin"))
                asset.CopyTo(dest);
            using (var asset = Assets.Open("DebugSettings.bin"))
            using (var dest = File.Create(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/4.55/DebugSettings.bin"))
                asset.CopyTo(dest);



            

            


            inject_btn = FindViewById<Button>(Resource.Id.inject_btn);

            inject_btn.Click += inject_btn_Click;


        }

        
        #region RuntimePermissions

        public void RequestPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted && ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, permission_Request);
                }
                else
                {
                    permissionGranted = true;
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case permission_Request:
                    {
                        // If request is cancelled, the result arrays are empty.
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                        {
                            permissionGranted = true;
                        }
                        else
                        {
                            var msgBox = (new AlertDialog.Builder(this)).Create();
                            msgBox.SetTitle("Warning!");
                            msgBox.SetMessage("You need to accept the Permission, in order to use the App!");
                            msgBox.SetButton("Ok", delegate
                            {
                                RequestPermissions();
                            });
                            msgBox.SetButton2("Cancel", delegate
                            {
                                System.Environment.Exit(0);
                            });
                            msgBox.Show();
                        }
                        return;
                    }

                    // other 'case' lines to check for other
                    // permissions this app might request
            }
        }

        #endregion


        private void getFilesInDownload()
        {
            if (permissionGranted)
            {
                Directory.CreateDirectory(pathToFolder);
                Directory.CreateDirectory(pathToFolder + "/1.76");
                Directory.CreateDirectory(pathToFolder + "/4.05");
                Directory.CreateDirectory(pathToFolder + "/4.55");

                DirectoryInfo di = new DirectoryInfo(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString());
                FileInfo[] files = di.GetFiles("*.bin", SearchOption.TopDirectoryOnly);

                foreach (FileInfo f in files)
                {
                    var msgBox = (new AlertDialog.Builder(this)).Create();
                    msgBox.SetTitle("Version");
                    msgBox.SetMessage("For which Version is the Payload " + f.Name.ToString() + "?");
                    msgBox.SetButton("1.76", delegate
                    {
                        if (File.Exists(pathToFolder + "/1.76/" + f.Name.ToString()))
                        {
                            AlertDialog.Builder msg = new AlertDialog.Builder(this);
                            msg.SetTitle("Warning!");
                            msg.SetMessage("File already existes! Overwrite?");
                            msg.SetPositiveButton("Yes", delegate
                            {
                                File.Replace(f.ToString(), pathToFolder + "/1.76/" + f.Name.ToString(), null);
                            });
                            msg.SetNeutralButton("No", delegate
                            {
                                msg.Dispose();
                            });
                            msg.Show();
                            return;
                        }

                        File.Move(f.ToString(), pathToFolder + "/1.76/" + f.Name.ToString());
                        File.Delete(f.ToString());
                    });
                    msgBox.SetButton2("4.05", delegate
                    {
                        if (File.Exists(pathToFolder + "/4.05/" + f.Name.ToString()))
                        {
                            AlertDialog.Builder msg = new AlertDialog.Builder(this);
                            msg.SetTitle("Warning!");
                            msg.SetMessage("File already exists! Overwrite?");
                            msg.SetPositiveButton("Yes", delegate
                            {
                                File.Replace(f.ToString(), pathToFolder + "/4.05/" + f.Name.ToString(), null);
                                File.Delete(f.ToString());
                            });
                            msg.SetNegativeButton("No", delegate
                            {
                                msg.Dispose();
                            });
                            msg.Show();
                            return;
                        }

                        File.Move(f.ToString(), pathToFolder + "/4.05/" + f.Name.ToString());
                        File.Delete(f.ToString());

                    });
                    msgBox.SetButton3("4.55", delegate
                    {
                        if (File.Exists(pathToFolder + "/4.55/" + f.Name.ToString()))
                        {
                            AlertDialog.Builder msg = new AlertDialog.Builder(this);
                            msg.SetTitle("Warning!");
                            msg.SetMessage("File already existes! Overwrite?");
                            msg.SetPositiveButton("Yes", delegate
                            {
                                File.Replace(f.ToString(), pathToFolder + "/4.55/" + f.Name.ToString(), null);
                            });
                            msg.SetNeutralButton("No", delegate
                            {
                                msg.Dispose();
                            });
                            msg.Show();
                            return;
                        }

                        File.Move(f.ToString(), pathToFolder + "/4.55/" + f.Name.ToString());
                    });
                    msgBox.Show();
                }

                getFilesInDirectory();
            }
        }

        private void getFilesInDirectory()
        {
            if (permissionGranted)
            {
                if (Directory.Exists(pathToFolder) && Directory.Exists(pathToFolder + "/1.76/") && Directory.Exists(pathToFolder + "/4.05/") && Directory.Exists(pathToFolder + "/4.55/"))
                {
                    DirectoryInfo di = new DirectoryInfo(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Tool/" + currentVersion);
                    FileInfo[] files = di.GetFiles("*.bin", SearchOption.TopDirectoryOnly);
                    List<string> finalFiles = new List<string>();
                    foreach (FileInfo f in files)
                    {
                        finalFiles.Add(f.Name.ToString());
                    }

                    payload_dropDown = FindViewById<Spinner>(Resource.Id.payload_DropDown);
                    ArrayAdapter<string> test = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, finalFiles);

                    test.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    payload_dropDown.Adapter = test;
                }
                else
                {
                    Directory.CreateDirectory(pathToFolder);
                    Directory.CreateDirectory(pathToFolder + "/1.76");
                    Directory.CreateDirectory(pathToFolder + "/4.05");
                    Directory.CreateDirectory(pathToFolder + "/4.55");
                }
            }
              
        }

       

        private void inject_btn_Click(object sender, EventArgs e)
            {
                inject_btn = FindViewById<Button>(Resource.Id.inject_btn);
                payload_dropDown = FindViewById<Spinner>(Resource.Id.payload_DropDown);
                port = FindViewById<EditText>(Resource.Id.ps4Port);
                ps4IPAdress = FindViewById<EditText>(Resource.Id.PS4IP);

                Userfiles US = new Userfiles();
                US.IPAdress = ps4IPAdress.Text;
                US.Save();
            
            
                if (ps4IPAdress.Text == null || ps4IPAdress.Text == "Ps4 IP Adress..." || port.Text == null || payload_dropDown.SelectedItem == null)
                {
                    AlertDialog.Builder msgBox = new AlertDialog.Builder(this);
                    msgBox.SetTitle("Error!");
                    msgBox.SetMessage("Enter IP Adress, Port and select a Payload!");
                    msgBox.SetNeutralButton("Ok", delegate
                    {
                        msgBox.Dispose();
                    });
                    msgBox.Show();
                }
                else
                {
                    modPath = pathToFolder + "/" + currentVersion + "/" + payload_dropDown.SelectedItem.ToString();
                    try
                        {
                    if (File.Exists(modPath))
                    {
                        int portText = int.Parse(port.Text);
                        SendTCP(modPath, ps4IPAdress.Text, portText);
                    }
                    else
                    {
                        AlertDialog.Builder msgBox = new AlertDialog.Builder(this);
                        msgBox.SetTitle("Error!");
                        msgBox.SetMessage("Couldn't find the selected Payload!");
                        msgBox.SetNeutralButton("Ok", delegate
                        {
                            msgBox.Dispose();
                        });
                        msgBox.Show();
                    }
                        }
                    catch
                    {
                        var msgBox = (new AlertDialog.Builder(this)).Create();
                        msgBox.SetTitle("Error!");
                        msgBox.SetMessage("Couldn't connect to PS4!");
                        msgBox.SetButton("Ok", delegate
                        {
                            msgBox.Dispose();
                        });
                        msgBox.SetButton2("Details", delegate
                        {
                            AlertDialog.Builder msg = new AlertDialog.Builder(this);
                            msg.SetTitle("Details:");
                            msg.SetMessage(errorMessage);
                            msg.SetNeutralButton("Ok", delegate
                            {
                                msg.Dispose();
                            });
                            msg.Show();
                        });
                        msgBox.Show();
                    }
                }
                
            }
        
    }
}
