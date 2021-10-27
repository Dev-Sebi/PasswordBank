using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SebiPasswordBank
{
    public partial class Form1 : Form
    {
        string Err = "An Error Accured, Please try again!";
        string path = @"c:\SebiPrograms\";
        int forgotpw = 0; // Login tries with wrong password
        int deletion = 5; // Change Key Attempts Left
        int totalentries = 0; //default entrys for view

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(372, 190);
            try
            {
                if (Directory.Exists(path))
                {
                    /*Nothing*/
                }
                else
                {
                    Directory.CreateDirectory(path);
                    File.Create(path + "PassKey.txt");
                    MessageBox.Show("Files Were Generated, Please reopen the Program");
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Err + e);
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string PasswordKey = Convert.ToString(File.ReadAllText(path + "PassKey.txt"));
                int i = 0;
                do
                {
                    if (PasswordKey == "")
                    {
                        this.Text = "Set Master Key";
                        SetMasterKey();
                    }
                    else if (PasswordKey != "")
                    {
                        this.Text = "Login";
                        EnterMasterKey();
                    }
                    else
                    {
                        MessageBox.Show("Couldn't read MasterKey, please contact the Developer");
                    }
                    i++;
                } while (i == 0);
            }
            catch (Exception)
            {
                Application.Exit();
            }
        }

        public void SetMasterKey()
        {
            setkey.Visible = true;
            repeatkey.Visible = true;
            passwordkey.Visible = true;
            passwordkey2.Visible = true;
            setkeybtn.Visible = true;

            if (forgotpw >= 3)
            {
                setkeybtn.Visible = false;
                SecurityQuestion.Visible = true;
            }
        }


        private void passwordkey_TextChanged(object sender, EventArgs e)
        {
            CheckPasswordBoxConfirmed();
        }

        private void passwordkey2_TextChanged(object sender, EventArgs e)
        {
            CheckPasswordBoxConfirmed();
        }

        public void CheckPasswordBoxConfirmed()
        {
            if (Convert.ToString(passwordkey.Text) != Convert.ToString(passwordkey2.Text))
            {
                setkeybtn.Enabled = false;
                SecurityQuestion.Enabled = false;
            }
            else if (Convert.ToString(passwordkey.Text) == Convert.ToString(passwordkey2.Text))
            {
                setkeybtn.Enabled = true;
                SecurityQuestion.Enabled = true;
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void setkeybtn_Click(object sender, EventArgs e)
        {
            string PasswordKey = Convert.ToString(passwordkey.Text);
            File.WriteAllText(path + "PassKey.txt", PasswordKey);
            LoadMainStructure();
        }

        private void loginbtnafterencryption_Click(object sender, EventArgs e)
        {
            /*Build in Security Question later release, this release uses EncryptionKey*/
            string EncryptionKey = "mT6NV6Y9cebKpaRuHY2LNAVfxMeYFsy92yPDCPnP53dSQNxZjDnKTSfArc98wdWZU5mKmTtZSYaXjAmgAu4STB7NeFr67U4ZS3swTmfRPArD4yMY9chQHaSe9buSpxMjpTM9jH8YEj9yRTx2NFHRXWAhMHqmwuzvqF46jTWfWNDMX7hWLVH4eYF5EfbAhUU57QXuzWMjF8w49WkLMwULpjyZnxzDdfJjcC2xTDkb5tPJvyea7zrpMKYGXvsRHkfw";

            if (EncryptionKeytxt.Text == EncryptionKey)
            {
                File.WriteAllText(path + "PassKey.txt", Convert.ToString(passwordkey.Text));
                LoadMainStructure();
            }
            else if (EncryptionKeytxt.Text != EncryptionKey)
            {
                MessageBox.Show("You got **" + deletion + "** tries left before self destruction");
                deletion--;

                if (deletion < 0)
                {
                    Directory.Delete(path, true);
                    Application.Exit();
                }
            }
        }

        private void SecurityQuestion_Click(object sender, EventArgs e)
        {
            setkey.Visible = false;
            repeatkey.Visible = false;
            passwordkey.Visible = false;
            passwordkey2.Visible = false;
            setkeybtn.Visible = false;
            logintxt.Visible = false;
            login.Visible = false;
            loginbtn.Visible = false;
            EncryptionKeytxt.Visible = true;
            EncryptionKeytxt.Text = "";
            EncryptionText.Visible = true;
            SecurityQuestion.Visible = false;
            loginbtnafterencryption.Visible = true;
        }

        public void EnterMasterKey()
        {
            setkey.Visible = false;
            repeatkey.Visible = false;
            passwordkey.Visible = false;
            passwordkey2.Visible = false;
            setkeybtn.Visible = false;
            logintxt.Visible = true;
            login.Visible = true;
            loginbtn.Visible = true;
        }

        private void logintxt_TextChanged(object sender, EventArgs e)
        {
            if (logintxt.Text == "")
            {
                loginbtn.Enabled = false;
            }
            else if (logintxt.Text != "")
            {
                loginbtn.Enabled = true;
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            forgotpw++;
            string RightKey = Convert.ToString(File.ReadAllText(path + "PassKey.txt"));

            if (forgotpw == 5 && RightKey != logintxt.Text)
            {
                ForgotPWLabel.Visible = true;
            }
            else if (forgotpw != 4)
            {
                if (logintxt.Text == RightKey)
                {
                    LoadMainStructure();
                }
                else if (logintxt.Text != RightKey)
                {
                    MessageBox.Show("Wrong Master Key");
                    login.ForeColor = Color.Red;
                }
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void ForgotPWLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Text = "Change Master Key";
            passwordkey.Text = "";
            passwordkey2.Text = "";
            login.Visible = false;
            loginbtn.Visible = false;
            logintxt.Visible = false;
            ForgotPWLabel.Visible = false;
            SetMasterKey();
        }

        private void EncryptionKeytxt_TextChanged(object sender, EventArgs e)
        {
            if (EncryptionKeytxt.Text == "")
            {
                loginbtnafterencryption.Enabled = false;
            }
            else if (EncryptionKeytxt.Text != "")
            {
                loginbtnafterencryption.Enabled = true;
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void logout_Click(object sender, EventArgs e)
        {
            passwordtextbox.Text = "";
            password2textbox.Text = "";
            logintxt.Text = "";
            login.ForeColor = Color.Black;
            UnLoadMainStructure();
            EnterMasterKey();
        }





        /*     MAIN DATABASE     */
        private void UnLoadMainStructure()
        {
            this.Size = new Size(372, 190);
            this.Text = "Login";
            welcomename.Visible = false;
            timelabel.Visible = false;
            logout.Visible = false;
            logintxt.Text = "";
        }

        private void LoadMainStructure()
        {
            LoadEntries();
            deleteall.Visible = true;
            setkey.Visible = false;
            setkeybtn.Visible = false;
            repeatkey.Visible = false;
            passwordkey.Visible = false;
            passwordkey2.Visible = false;
            EncryptionText.Visible = false;
            EncryptionKeytxt.Visible = false;
            login.Visible = false;
            logintxt.Visible = false;
            loginbtn.Visible = false;
            loginbtnafterencryption.Visible = false;
            ForgotPWLabel.Visible = false;
            SecurityQuestion.Visible = false;
            newentry.Visible = true;
            newentry.Enabled = true;
            usagetextbox.Visible = true;
            usagetextbox.Enabled = false;
            passwordtextbox.Visible = true;
            passwordtextbox.Enabled = false;
            password2textbox.Visible = true;
            password2textbox.Enabled = false;
            cancelentry.Visible = true;
            cancelentry.Enabled = false;
            addentry.Visible = true;
            addentry.Enabled = false;
            usagepw.Visible = true;
            pwentry.Visible = true;
            pwentry2.Visible = true;


            this.Size = new Size(1280, 720);
            this.Text = "Password Database";
            //
            logout.Visible = true;
            welcomename.Visible = true;
            timelabel.Visible = true;
            welcomename.Text = "Welcome " + Environment.UserName;
            timelabel.Text = "Last Login: " + Convert.ToString(DateTime.Now);
        }

        private void newentry_Click(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(File.ReadAllText(path + "TotalEntries.txt"));

            if (total >= 12)
            {
                MessageBox.Show("All Spaces are being used, please delete any and try again");
            }

            else if (total <= 11)
            {
                // dont add Add button here, Addbutton is declared if one of the textboxs is not filled in
                newentry.Enabled = false;
                usagetextbox.Enabled = true;
                passwordtextbox.Enabled = true;
                password2textbox.Enabled = true;
                cancelentry.Enabled = true;

                pwentry.ForeColor = Color.Black;
                pwentry2.ForeColor = Color.Black;
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void cancelentry_Click(object sender, EventArgs e)
        {
            newentry.Enabled = true;
            addentry.Enabled = false;
            usagetextbox.Enabled = false;
            passwordtextbox.Enabled = false;
            password2textbox.Enabled = false;
            cancelentry.Enabled = false;

            passwordtextbox.Text = "";
            password2textbox.Text = "";
        }

        private void addentry_Click(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(File.ReadAllText(path + "TotalEntries.txt"));

            if (passwordtextbox.Text == password2textbox.Text)
            {

                if (total >= 12)
                {
                    MessageBox.Show("All Spaces are being used, please delete any and try again");
                }
                else if (total <= 11)
                {
                    totalentries++;

                    File.WriteAllText(path + "TotalEntries.txt", Convert.ToString(totalentries));
                    LoadEntries();
                }
                else
                {
                    MessageBox.Show("Err");
                }


                passwordtextbox.Text = "";
                password2textbox.Text = "";
            }
            else if (passwordtextbox.Text != password2textbox.Text)
            {
                pwentry.ForeColor = Color.Red;
                pwentry2.ForeColor = Color.Red;
                MessageBox.Show("Passwords are not Matching! Please Check!");
            }
            else
            {
                MessageBox.Show(Err);
            }
        }

        private void deleteall_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path + "TotalEntries.txt", "0");

            string path1 = @"c:\SebiPrograms\Case1\";
            string path2 = @"c:\SebiPrograms\Case2\";
            string path3 = @"c:\SebiPrograms\Case3\";
            string path4 = @"c:\SebiPrograms\Case4\";
            string path5 = @"c:\SebiPrograms\Case5\";
            string path6 = @"c:\SebiPrograms\Case6\";
            string path7 = @"c:\SebiPrograms\Case7\";
            string path8 = @"c:\SebiPrograms\Case8\";
            string path9 = @"c:\SebiPrograms\Case9\";
            string path10 = @"c:\SebiPrograms\Case10\";
            string path11 = @"c:\SebiPrograms\Case11\";
            string path12 = @"c:\SebiPrograms\Case12\";

            try
            {
                Directory.Delete(path1, true);
                Directory.Delete(path2, true);
                Directory.Delete(path3, true);
                Directory.Delete(path4, true);
                Directory.Delete(path5, true);
                Directory.Delete(path6, true);
                Directory.Delete(path7, true);
                Directory.Delete(path8, true);
                Directory.Delete(path9, true);
                Directory.Delete(path10, true);
                Directory.Delete(path11, true);
                Directory.Delete(path12, true);
                LoadEntries();
            }

            catch (Exception)
            {
                MessageBox.Show(Err);
            }
        }

        public void LoadEntries()
        {
            try
            {
                File.ReadAllText(path + "TotalEntries.txt");
            }
            catch (Exception)
            {
                Console.WriteLine("No Entries Found - Loading 0 Entries");
                File.WriteAllText(path + "TotalEntries.txt", "0");
            }

            int total = Convert.ToInt32(File.ReadAllText(path + "TotalEntries.txt"));

            if (total == 0)
            {
                MessageBox.Show("No Saved Entries Found");
            }

            else if (total <= 12)
            {
                if (total > 0 && total > 13)
                {
                    deleteall.Enabled = true;
                }

                string path1 = @"c:\SebiPrograms\Case1\";
                string path2 = @"c:\SebiPrograms\Case2\";
                string path3 = @"c:\SebiPrograms\Case3\";
                string path4 = @"c:\SebiPrograms\Case4\";
                string path5 = @"c:\SebiPrograms\Case5\";
                string path6 = @"c:\SebiPrograms\Case6\";
                string path7 = @"c:\SebiPrograms\Case7\";
                string path8 = @"c:\SebiPrograms\Case8\";
                string path9 = @"c:\SebiPrograms\Case9\";
                string path10 = @"c:\SebiPrograms\Case10\";
                string path11 = @"c:\SebiPrograms\Case11\";
                string path12 = @"c:\SebiPrograms\Case12\";


                switch (total)
                {
                    case 1:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path1);
                                File.WriteAllText(path1 + "U1.txt", usage);
                                File.WriteAllText(path1 + "PW1.txt", password);

                                usagetxt1.Text = usage;
                                passwordtext1.Text = password;
                            }
                            else
                            {
                                usagetxt1.Text = "FAILED TO LOAD";
                                passwordtext1.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 2:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path2);
                                File.WriteAllText(path2 + "U2.txt", usage);
                                File.WriteAllText(path2 + "PW2.txt", password);

                                usagetxt2.Text = usage;
                                passwordtext2.Text = password;
                            }
                            else
                            {
                                usagetxt2.Text = "FAILED TO LOAD";
                                passwordtext2.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 3:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path3);
                                File.WriteAllText(path3 + "U3.txt", usage);
                                File.WriteAllText(path3 + "PW3.txt", password);

                                usagetxt3.Text = usage;
                                passwordtext3.Text = password;
                            }
                            else
                            {
                                usagetxt3.Text = "FAILED TO LOAD";
                                passwordtext3.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 4:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path4);
                                File.WriteAllText(path4 + "U4.txt", usage);
                                File.WriteAllText(path4 + "PW4.txt", password);

                                usagetxt4.Text = usage;
                                passwordtext4.Text = password;
                            }
                            else
                            {
                                usagetxt4.Text = "FAILED TO LOAD";
                                passwordtext4.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 5:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path5);
                                File.WriteAllText(path5 + "U5.txt", usage);
                                File.WriteAllText(path5 + "PW5.txt", password);

                                usagetxt5.Text = usage;
                                passwordtext5.Text = password;
                            }
                            else
                            {
                                usagetxt5.Text = "FAILED TO LOAD";
                                passwordtext5.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 6:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path6);
                                File.WriteAllText(path6 + "U6.txt", usage);
                                File.WriteAllText(path6 + "PW6.txt", password);

                                usagetxt6.Text = usage;
                                passwordtext6.Text = password;
                            }
                            else
                            {
                                usagetxt6.Text = "FAILED TO LOAD";
                                passwordtext6.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 7:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path7);
                                File.WriteAllText(path7 + "U7.txt", usage);
                                File.WriteAllText(path7 + "PW7.txt", password);

                                usagetxt7.Text = usage;
                                passwordtext7.Text = password;
                            }
                            else
                            {
                                usagetxt7.Text = "FAILED TO LOAD";
                                passwordtext7.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 8:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        usage8.Visible = true;
                        usagetxt8.Visible = true;
                        password8.Visible = true;
                        passwordtext8.Visible = true;
                        copypw8.Visible = true;
                        viewpw8.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            usagetxt7.Text = "FAILED TO LOAD";
                            passwordtext7.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path8))
                        {
                            string usage = File.ReadAllText(path8 + "U8.txt");
                            string password = File.ReadAllText(path8 + "PW8.txt");

                            usagetxt8.Text = usage;
                            passwordtext8.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path8);
                                File.WriteAllText(path8 + "U8.txt", usage);
                                File.WriteAllText(path8 + "PW8.txt", password);

                                usagetxt8.Text = usage;
                                passwordtext8.Text = password;
                            }
                            else
                            {
                                usagetxt8.Text = "FAILED TO LOAD";
                                passwordtext8.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 9:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        usage8.Visible = true;
                        usagetxt8.Visible = true;
                        password8.Visible = true;
                        passwordtext8.Visible = true;
                        copypw8.Visible = true;
                        viewpw8.Visible = true;
                        usage9.Visible = true;
                        usagetxt9.Visible = true;
                        password9.Visible = true;
                        passwordtext9.Visible = true;
                        copypw9.Visible = true;
                        viewpw9.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            usagetxt7.Text = "FAILED TO LOAD";
                            passwordtext7.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path8))
                        {
                            string usage = File.ReadAllText(path8 + "U8.txt");
                            string password = File.ReadAllText(path8 + "PW8.txt");

                            usagetxt8.Text = usage;
                            passwordtext8.Text = password;
                        }
                        else
                        {
                            usagetxt8.Text = "FAILED TO LOAD";
                            passwordtext8.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path9))
                        {
                            string usage = File.ReadAllText(path9 + "U9.txt");
                            string password = File.ReadAllText(path9 + "PW9.txt");

                            usagetxt9.Text = usage;
                            passwordtext9.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path9);
                                File.WriteAllText(path9 + "U9.txt", usage);
                                File.WriteAllText(path9 + "PW9.txt", password);

                                usagetxt9.Text = usage;
                                passwordtext9.Text = password;
                            }
                            else
                            {
                                usagetxt9.Text = "FAILED TO LOAD";
                                passwordtext9.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 10:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        usage8.Visible = true;
                        usagetxt8.Visible = true;
                        password8.Visible = true;
                        passwordtext8.Visible = true;
                        copypw8.Visible = true;
                        viewpw8.Visible = true;
                        usage9.Visible = true;
                        usagetxt9.Visible = true;
                        password9.Visible = true;
                        passwordtext9.Visible = true;
                        copypw9.Visible = true;
                        viewpw9.Visible = true;
                        usage10.Visible = true;
                        usagetxt10.Visible = true;
                        password10.Visible = true;
                        passwordtext10.Visible = true;
                        copypw10.Visible = true;
                        viewpw10.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            usagetxt7.Text = "FAILED TO LOAD";
                            passwordtext7.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path8))
                        {
                            string usage = File.ReadAllText(path8 + "U8.txt");
                            string password = File.ReadAllText(path8 + "PW8.txt");

                            usagetxt8.Text = usage;
                            passwordtext8.Text = password;
                        }
                        else
                        {
                            usagetxt8.Text = "FAILED TO LOAD";
                            passwordtext8.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path9))
                        {
                            string usage = File.ReadAllText(path9 + "U9.txt");
                            string password = File.ReadAllText(path9 + "PW9.txt");

                            usagetxt9.Text = usage;
                            passwordtext9.Text = password;
                        }
                        else
                        {
                            usagetxt9.Text = "FAILED TO LOAD";
                            passwordtext9.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path10))
                        {
                            string usage = File.ReadAllText(path10 + "U10.txt");
                            string password = File.ReadAllText(path10 + "PW10.txt");

                            usagetxt10.Text = usage;
                            passwordtext10.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path10);
                                File.WriteAllText(path10 + "U10.txt", usage);
                                File.WriteAllText(path10 + "PW10.txt", password);

                                usagetxt10.Text = usage;
                                passwordtext10.Text = password;
                            }
                            else
                            {
                                usagetxt10.Text = "FAILED TO LOAD";
                                passwordtext10.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 11:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        usage8.Visible = true;
                        usagetxt8.Visible = true;
                        password8.Visible = true;
                        passwordtext8.Visible = true;
                        copypw8.Visible = true;
                        viewpw8.Visible = true;
                        usage9.Visible = true;
                        usagetxt9.Visible = true;
                        password9.Visible = true;
                        passwordtext9.Visible = true;
                        copypw9.Visible = true;
                        viewpw9.Visible = true;
                        usage10.Visible = true;
                        usagetxt10.Visible = true;
                        password10.Visible = true;
                        passwordtext10.Visible = true;
                        copypw10.Visible = true;
                        viewpw10.Visible = true;
                        usage11.Visible = true;
                        usagetxt11.Visible = true;
                        password11.Visible = true;
                        passwordtext11.Visible = true;
                        copypw11.Visible = true;
                        viewpw11.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            usagetxt7.Text = "FAILED TO LOAD";
                            passwordtext7.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path8))
                        {
                            string usage = File.ReadAllText(path8 + "U8.txt");
                            string password = File.ReadAllText(path8 + "PW8.txt");

                            usagetxt8.Text = usage;
                            passwordtext8.Text = password;
                        }
                        else
                        {
                            usagetxt8.Text = "FAILED TO LOAD";
                            passwordtext8.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path9))
                        {
                            string usage = File.ReadAllText(path9 + "U9.txt");
                            string password = File.ReadAllText(path9 + "PW9.txt");

                            usagetxt9.Text = usage;
                            passwordtext9.Text = password;
                        }
                        else
                        {
                            usagetxt9.Text = "FAILED TO LOAD";
                            passwordtext9.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path10))
                        {
                            string usage = File.ReadAllText(path10 + "U10.txt");
                            string password = File.ReadAllText(path10 + "PW10.txt");

                            usagetxt10.Text = usage;
                            passwordtext10.Text = password;
                        }
                        else
                        {
                            usagetxt10.Text = "FAILED TO LOAD";
                            passwordtext10.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path11))
                        {
                            string usage = File.ReadAllText(path11 + "U11.txt");
                            string password = File.ReadAllText(path11 + "PW11.txt");

                            usagetxt11.Text = usage;
                            passwordtext11.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path11);
                                File.WriteAllText(path11 + "U11.txt", usage);
                                File.WriteAllText(path11 + "PW11.txt", password);

                                usagetxt11.Text = usage;
                                passwordtext11.Text = password;
                            }
                            else
                            {
                                usagetxt11.Text = "FAILED TO LOAD";
                                passwordtext11.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                    case 12:
                        usage1.Visible = true;
                        usagetxt1.Visible = true;

                        password1.Visible = true;
                        passwordtext1.Visible = true;
                        copypw1.Visible = true;
                        viewpw1.Visible = true;
                        usage2.Visible = true;
                        usagetxt2.Visible = true;
                        password2.Visible = true;
                        passwordtext2.Visible = true;
                        copypw2.Visible = true;
                        viewpw2.Visible = true;
                        usage3.Visible = true;
                        usagetxt3.Visible = true;
                        password3.Visible = true;
                        passwordtext3.Visible = true;
                        copypw3.Visible = true;
                        viewpw3.Visible = true;
                        usage4.Visible = true;
                        usagetxt4.Visible = true;
                        password4.Visible = true;
                        passwordtext4.Visible = true;
                        copypw4.Visible = true;
                        viewpw4.Visible = true;
                        usage5.Visible = true;
                        usagetxt5.Visible = true;
                        password5.Visible = true;
                        passwordtext5.Visible = true;
                        copypw5.Visible = true;
                        viewpw5.Visible = true;
                        usage6.Visible = true;
                        usagetxt6.Visible = true;
                        password6.Visible = true;
                        passwordtext6.Visible = true;
                        copypw6.Visible = true;
                        viewpw6.Visible = true;
                        usage7.Visible = true;
                        usagetxt7.Visible = true;
                        password7.Visible = true;
                        passwordtext7.Visible = true;
                        copypw7.Visible = true;
                        viewpw7.Visible = true;
                        usage8.Visible = true;
                        usagetxt8.Visible = true;
                        password8.Visible = true;
                        passwordtext8.Visible = true;
                        copypw8.Visible = true;
                        viewpw8.Visible = true;
                        usage9.Visible = true;
                        usagetxt9.Visible = true;
                        password9.Visible = true;
                        passwordtext9.Visible = true;
                        copypw9.Visible = true;
                        viewpw9.Visible = true;
                        usage10.Visible = true;
                        usagetxt10.Visible = true;
                        password10.Visible = true;
                        passwordtext10.Visible = true;
                        copypw10.Visible = true;
                        viewpw10.Visible = true;
                        usage11.Visible = true;
                        usagetxt11.Visible = true;
                        password11.Visible = true;
                        passwordtext11.Visible = true;
                        copypw11.Visible = true;
                        viewpw11.Visible = true;
                        usage12.Visible = true;
                        usagetxt12.Visible = true;
                        password12.Visible = true;
                        passwordtext12.Visible = true;
                        copypw12.Visible = true;
                        viewpw12.Visible = true;
                        if (Directory.Exists(path1))
                        {
                            string usage = File.ReadAllText(path1 + "U1.txt");
                            string password = File.ReadAllText(path1 + "PW1.txt");

                            usagetxt1.Text = usage;
                            passwordtext1.Text = password;
                        }
                        else
                        {
                            usagetxt1.Text = "FAILED TO LOAD";
                            passwordtext1.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path2))
                        {
                            string usage = File.ReadAllText(path2 + "U2.txt");
                            string password = File.ReadAllText(path2 + "PW2.txt");

                            usagetxt2.Text = usage;
                            passwordtext2.Text = password;
                        }
                        else
                        {
                            usagetxt2.Text = "FAILED TO LOAD";
                            passwordtext2.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path3))
                        {
                            string usage = File.ReadAllText(path3 + "U3.txt");
                            string password = File.ReadAllText(path3 + "PW3.txt");

                            usagetxt3.Text = usage;
                            passwordtext3.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path4))
                        {
                            string usage = File.ReadAllText(path4 + "U4.txt");
                            string password = File.ReadAllText(path4 + "PW4.txt");

                            usagetxt4.Text = usage;
                            passwordtext4.Text = password;
                        }
                        else
                        {
                            usagetxt3.Text = "FAILED TO LOAD";
                            passwordtext3.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path5))
                        {
                            string usage = File.ReadAllText(path5 + "U5.txt");
                            string password = File.ReadAllText(path5 + "PW5.txt");

                            usagetxt5.Text = usage;
                            passwordtext5.Text = password;
                        }
                        else
                        {
                            usagetxt5.Text = "FAILED TO LOAD";
                            passwordtext5.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path6))
                        {
                            string usage = File.ReadAllText(path6 + "U6.txt");
                            string password = File.ReadAllText(path6 + "PW6.txt");

                            usagetxt6.Text = usage;
                            passwordtext6.Text = password;
                        }
                        {
                            usagetxt6.Text = "FAILED TO LOAD";
                            passwordtext6.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path7))
                        {
                            string usage = File.ReadAllText(path7 + "U7.txt");
                            string password = File.ReadAllText(path7 + "PW7.txt");

                            usagetxt7.Text = usage;
                            passwordtext7.Text = password;
                        }
                        else
                        {
                            usagetxt7.Text = "FAILED TO LOAD";
                            passwordtext7.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path8))
                        {
                            string usage = File.ReadAllText(path8 + "U8.txt");
                            string password = File.ReadAllText(path8 + "PW8.txt");

                            usagetxt8.Text = usage;
                            passwordtext8.Text = password;
                        }
                        else
                        {
                            usagetxt8.Text = "FAILED TO LOAD";
                            passwordtext8.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path9))
                        {
                            string usage = File.ReadAllText(path9 + "U9.txt");
                            string password = File.ReadAllText(path9 + "PW9.txt");

                            usagetxt9.Text = usage;
                            passwordtext9.Text = password;
                        }
                        else
                        {
                            usagetxt9.Text = "FAILED TO LOAD";
                            passwordtext9.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path10))
                        {
                            string usage = File.ReadAllText(path10 + "U10.txt");
                            string password = File.ReadAllText(path10 + "PW10.txt");

                            usagetxt10.Text = usage;
                            passwordtext10.Text = password;
                        }
                        else
                        {
                            usagetxt10.Text = "FAILED TO LOAD";
                            passwordtext10.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path11))
                        {
                            string usage = File.ReadAllText(path11 + "U11.txt");
                            string password = File.ReadAllText(path11 + "PW11.txt");

                            usagetxt11.Text = usage;
                            passwordtext11.Text = password;
                        }
                        else
                        {
                            usagetxt11.Text = "FAILED TO LOAD";
                            passwordtext11.Text = "FAILED TO LOAD";
                        }
                        if (Directory.Exists(path12))
                        {
                            string usage = File.ReadAllText(path12 + "U12.txt");
                            string password = File.ReadAllText(path12 + "PW12.txt");

                            usagetxt12.Text = usage;
                            passwordtext12.Text = password;
                        }
                        else
                        {
                            string usage = usagetextbox.Text;
                            string password = passwordtextbox.Text;

                            if (usage != "" && password != "")
                            {
                                Directory.CreateDirectory(path12);
                                File.WriteAllText(path12 + "U12.txt", usage);
                                File.WriteAllText(path12 + "PW12.txt", password);

                                usagetxt12.Text = usage;
                                passwordtext12.Text = password;
                            }
                            else
                            {
                                usagetxt12.Text = "FAILED TO LOAD";
                                passwordtext12.Text = "FAILED TO LOAD";
                            }
                        }
                        break;
                }


                MessageBox.Show(total + " Entries Loaded");
            }
            else
            {
                MessageBox.Show(Err + " - Failed to Load Database.");
                Application.Exit();
            }
        }

        private void usagetextbox_TextChanged(object sender, EventArgs e)
        {
            CheckIfAllFilled();
        }

        private void passwordtextbox_TextChanged(object sender, EventArgs e)
        {
            CheckIfAllFilled();
        }

        private void password2textbox_TextChanged(object sender, EventArgs e)
        {
            CheckIfAllFilled();
        }

        public void CheckIfAllFilled()
        {
            if (usagetextbox.Text == "" && passwordtextbox.Text == "" && password2textbox.Text == "")
            {
                addentry.Enabled = false;
            }
            else if (usagetextbox.Text != "" && passwordtextbox.Text != "" && password2textbox.Text != "")
            {
                addentry.Enabled = true;
            }
        }


        /*     DATABASE BUTTONS     */

        private void copypw1_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext1.Text);
        }
        private void viewpw1_Click(object sender, EventArgs e)
        {
            if (passwordtext1.UseSystemPasswordChar == true)
            {
                viewpw1.Text = "Hide PW";
                passwordtext1.UseSystemPasswordChar = false;
            }
            else if (passwordtext1.UseSystemPasswordChar == false)
            {
                viewpw1.Text = "View PW";
                passwordtext1.UseSystemPasswordChar = true;
            }
        }

        private void copypw2_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext2.Text);
        }

        private void viewpw2_Click(object sender, EventArgs e)
        {
            if (passwordtext2.UseSystemPasswordChar == true)
            {
                viewpw2.Text = "Hide PW";
                passwordtext2.UseSystemPasswordChar = false;
            }
            else if (passwordtext2.UseSystemPasswordChar == false)
            {
                viewpw2.Text = "View PW";
                passwordtext2.UseSystemPasswordChar = true;
            }
        }

        private void copypw3_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext3.Text);
        }

        private void viewpw3_Click(object sender, EventArgs e)
        {
            if (passwordtext3.UseSystemPasswordChar == true)
            {
                viewpw3.Text = "Hide PW";
                passwordtext3.UseSystemPasswordChar = false;
            }
            else if (passwordtext3.UseSystemPasswordChar == false)
            {
                viewpw3.Text = "View PW";
                passwordtext3.UseSystemPasswordChar = true;
            }
        }

        private void copypw4_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext4.Text);
        }

        private void viewpw4_Click(object sender, EventArgs e)
        {
            if (passwordtext4.UseSystemPasswordChar == true)
            {
                viewpw4.Text = "Hide PW";
                passwordtext4.UseSystemPasswordChar = false;
            }
            else if (passwordtext4.UseSystemPasswordChar == false)
            {
                viewpw4.Text = "View PW";
                passwordtext4.UseSystemPasswordChar = true;
            }
        }

        private void copypw5_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext5.Text);
        }

        private void viewpw5_Click(object sender, EventArgs e)
        {
            if (passwordtext5.UseSystemPasswordChar == true)
            {
                viewpw5.Text = "Hide PW";
                passwordtext5.UseSystemPasswordChar = false;
            }
            else if (passwordtext5.UseSystemPasswordChar == false)
            {
                viewpw5.Text = "View PW";
                passwordtext5.UseSystemPasswordChar = true;
            }
        }

        private void copypw6_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext6.Text);
        }

        private void viewpw6_Click(object sender, EventArgs e)
        {
            if (passwordtext6.UseSystemPasswordChar == true)
            {
                viewpw6.Text = "Hide PW";
                passwordtext6.UseSystemPasswordChar = false;
            }
            else if (passwordtext6.UseSystemPasswordChar == false)
            {
                viewpw6.Text = "View PW";
                passwordtext6.UseSystemPasswordChar = true;
            }
        }

        private void copypw7_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext7.Text);
        }

        private void viewpw7_Click(object sender, EventArgs e)
        {
            if (passwordtext7.UseSystemPasswordChar == true)
            {
                viewpw7.Text = "Hide PW";
                passwordtext7.UseSystemPasswordChar = false;
            }
            else if (passwordtext7.UseSystemPasswordChar == false)
            {
                viewpw7.Text = "View PW";
                passwordtext7.UseSystemPasswordChar = true;
            }
        }

        private void copypw8_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext8.Text);
        }

        private void viewpw8_Click(object sender, EventArgs e)
        {
            if (passwordtext8.UseSystemPasswordChar == true)
            {
                viewpw8.Text = "Hide PW";
                passwordtext8.UseSystemPasswordChar = false;
            }
            else if (passwordtext8.UseSystemPasswordChar == false)
            {
                viewpw8.Text = "View PW";
                passwordtext8.UseSystemPasswordChar = true;
            }
        }

        private void copypw9_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext9.Text);
        }

        private void viewpw9_Click(object sender, EventArgs e)
        {
            if (passwordtext9.UseSystemPasswordChar == true)
            {
                viewpw9.Text = "Hide PW";
                passwordtext9.UseSystemPasswordChar = false;
            }
            else if (passwordtext9.UseSystemPasswordChar == false)
            {
                viewpw9.Text = "View PW";
                passwordtext9.UseSystemPasswordChar = true;
            }
        }

        private void copypw10_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext10.Text);
        }

        private void viewpw10_Click(object sender, EventArgs e)
        {
            if (passwordtext10.UseSystemPasswordChar == true)
            {
                viewpw10.Text = "Hide PW";
                passwordtext10.UseSystemPasswordChar = false;
            }
            else if (passwordtext10.UseSystemPasswordChar == false)
            {
                viewpw10.Text = "View PW";
                passwordtext10.UseSystemPasswordChar = true;
            }
        }

        private void copypw11_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext11.Text);
        }

        private void viewpw11_Click(object sender, EventArgs e)
        {
            if (passwordtext11.UseSystemPasswordChar == true)
            {
                viewpw11.Text = "Hide PW";
                passwordtext11.UseSystemPasswordChar = false;
            }
            else if (passwordtext11.UseSystemPasswordChar == false)
            {
                viewpw11.Text = "View PW";
                passwordtext11.UseSystemPasswordChar = true;
            }
        }

        private void copypw12_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)passwordtext12.Text);
        }

        private void viewpw12_Click(object sender, EventArgs e)
        {
            if (passwordtext12.UseSystemPasswordChar == true)
            {
                viewpw12.Text = "Hide PW";
                passwordtext12.UseSystemPasswordChar = false;
            }
            else if (passwordtext12.UseSystemPasswordChar == false)
            {
                viewpw12.Text = "View PW";
                passwordtext12.UseSystemPasswordChar = true;
            }
        }
    }
}