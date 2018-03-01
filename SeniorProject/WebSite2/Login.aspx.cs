﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
    }//end method

    protected void Submit_Click(object sender, EventArgs e)
    {
        errormsg.Text = "";
        incorrectmsg.Text = "";
        if (pass.Text == "" || pass.Text == null || user.Text == "" || user.Text == null)
        {
            errormsg.Text = "Error, must have all fields filled out";
        }//end if

        else
        {
            String dbPassword = "2442022411122349917696223246343111622219816422031511843150228361812417717097232104806116156106715981331392471161472361447076896854076189109105204170922261282018181126147209236142226824328583818221323126120108971201223861621451535121312919915"; //pull password from DB
            String adminUsername = "user"; //pull username from DB
            String saltDB = "81126147209236142226824328583818221323126120108971201223861621451535121312919915";
            String adminPassword = pass.Text;

            //byte [] salt = System.Text.UnicodeEncoding.Unicode.GetBytes(saltDB);

            //use this to gen the hash
            //byte[] salt = GenerateSalt();

            //Use this when db is hooked up
            //String cred = String.Concat(dbPassword,saltDB);
            //byte[] encryptedpasswordDB = System.Text.UnicodeEncoding.Unicode.GetBytes(dbPassword);
            //byte[] encryptedpasswordDB2 = Combine(encryptedpasswordDB, salt);
            //string converted = Convert.ToBase64String(encryptedpasswordDB);
            //Label2.Text = encryptedpasswordDB;
            //foreach (byte i in salt)
            //Label2.Text += i;
            //byte[] encryptedpasswordDB = GenerateSaltedHash(dbPassword,salt);
            byte[] encryptedpasswordAdmin = GenerateSaltedHash(adminPassword);//,saltDB);
            String fin = "";
            foreach (byte i in encryptedpasswordAdmin)
                fin += i;
            Label2.Text = fin;
            String cred = String.Concat(fin,saltDB);
            //string converteds = Convert.ToBase64String(encryptedpasswordAdmin);
            //foreach (byte i in encryptedpasswordDB2)
            Label3.Text = cred;

            //use this to display the hash
            /*string converted = Convert.ToBase64String(encryptedpasswordAdmin);
            Label2.Text = converted; */

            bool matchingpass = CompareByteArrays(cred, dbPassword);//encryptedpasswordAdmin, encryptedpasswordAdmin);

            if(matchingpass && adminUsername == user.Text)
            {
                Session["confirm"] = matchingpass;
                Response.Redirect("Manage.aspx");
            }//end if

            else
            {
                incorrectmsg.Text = "Wrong Username or Password, Please Try Again";
            }//end else
        }//end else

    }//end method

    static byte[] GenerateSalt()
    {
        byte[] salt = new byte[32];
        System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);
        return salt;
    }//end method

    static byte[] GenerateSaltedHash(String password)//, String salt)
    {      
        // Convert the plain string pwd into bytes
        byte[] passwordbytes = System.Text.UnicodeEncoding.Unicode.GetBytes(password);

        HashAlgorithm algorithm = new SHA512Managed();

        /*byte[] passwordWithSaltBytes = new byte[passwordbytes.Length + salt.Length];

        for (int i = 0; i < passwordbytes.Length; i++)
        {
            passwordWithSaltBytes[i] = passwordbytes[i];
        }//end for
        for (int i = 0; i < salt.Length; i++)
        {
            passwordWithSaltBytes[passwordbytes.Length + i] = (byte)salt[i];
        }//end for*/

        return algorithm.ComputeHash(passwordbytes);
    }//end method



    public static bool CompareByteArrays(String str, String str2)//byte[] array1, byte[] array2)
    {
        /*if (array1.Length != array2.Length)
        {
            return false;
        }//end if

        for (int i = 0; i < array1.Length; i++)
        {
            if(array1[i] != array2[i])
            {
                return false;
            }//end if
        }//end for*/
        if (!str.Equals(str2))
            return false;

        return true;
    }//end method
    

    public static byte[] Combine(byte [] ara, byte [] salt)
    {
        byte[] passwordWithSaltBytes = new byte[ara.Length + salt.Length];

        for (int i = 0; i < ara.Length; i++)
        {
            passwordWithSaltBytes[i] = ara[i];
        }//end for
        for (int i = 0; i < salt.Length; i++)
        {
            passwordWithSaltBytes[ara.Length + i] = salt[i];
        }//end for

        return passwordWithSaltBytes;
    }//end method


}//end class