using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            String dbPassword = "pass"; //pull password form DB
            String adminPassword = pass.Text;
            byte[] salt = GenerateSalt();
            byte[] encryptedpasswordDB = GenerateSaltedHash(dbPassword,salt);
            byte[] encryptedpasswordAdmin = GenerateSaltedHash(adminPassword,salt);
            bool matching = CompareByteArrays(encryptedpasswordDB, encryptedpasswordAdmin);

            if(matching)
            {
                Session["confirm"] = matching;
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

    static byte[] GenerateSaltedHash(String password, byte [] salt)
    {      
        // Convert the plain string pwd into bytes
        byte[] passwordbytes = System.Text.UnicodeEncoding.Unicode.GetBytes(password);

        HashAlgorithm algorithm = new SHA256Managed();

        byte[] passwordWithSaltBytes = new byte[passwordbytes.Length + salt.Length];

        for (int i = 0; i < passwordbytes.Length; i++)
        {
            passwordWithSaltBytes[i] = passwordbytes[i];
        }//end for
        for (int i = 0; i < salt.Length; i++)
        {
            passwordWithSaltBytes[passwordbytes.Length + i] = salt[i];
        }//end for

        return algorithm.ComputeHash(passwordWithSaltBytes);
    }//end method



    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }//end if

        for (int i = 0; i < array1.Length; i++)
        {
            if(array1[i] != array2[i])
            {
                return false;
            }//end if
        }//end for

        return true;
    }//end method
    

}//end class