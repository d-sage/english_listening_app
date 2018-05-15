using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

public partial class _Default : System.Web.UI.Page
{

    /*
     * 
     * TODO
     * 
     */
    private RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
    private String dbUsername;
    private String dbSalt;
    private String dbPass;

    /*
     * 
     * TODO
     * 
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Session["oldCid"] = "";
        Session["oldTid"] = "";
        Session["oldLid"] = "";
        Session["oldText"] = "";
        
        this.lblTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        
        string text = "";
        string server = "mysql5018.site4now.net";
        string database = "db_a38d8d_lambe";
        string uid = "a38d8d_lambe";
        string password = "Lambejor000";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            
            String sql = "SELECT * FROM credentials";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                dbUsername = (String)rdr[0];
                dbSalt = (String)rdr[1];
                dbPass = (String)rdr[2];
            }//end while
            rdr.Close();

        }//end try
        catch (MySqlException ex)
        {
            //When handling errors, you can your application's response based 
            //on the error number.
            //The two most common error numbers when connecting are as follows:
            //0: Cannot connect to server.
            //1045: Invalid user name and/or password.
            switch (ex.Number)
            {
                case 0:
                    text = "Cannot connect to server.  Contact administrator";
                    EmailError(text);
                    break;
                case 1045:
                    text = "Invalid username/password, please try again";
                    EmailError(text);
                    break;
                default:
                    text = "number: " + ex.Number;
                    EmailError(text);
                    break;
            }//end switch
            text += "bad";
        }//end catch

        connection.Close();    
        errormsgDB.Text = text;

    }//end method

    /*
     * 
     * TODO
     * 
     */
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
            String adminPassword = pass.Text;

            //use this to gen the hash
            //byte[] salt = GenerateSalt();

            byte[] encryptedpasswordAdmin = GenerateSaltedHash(adminPassword);//,saltDB);
            String fin = "";
            foreach (byte i in encryptedpasswordAdmin)
                fin += i;
            String cred = String.Concat(fin,dbSalt);

            bool matchingpass = ComparePasswords(cred, dbPass);

            if(matchingpass && dbUsername == user.Text)
            {
                Session["confirm"] = matchingpass;
                Response.Redirect("ManageAdd.aspx");
            }//end if

            else
            {
                incorrectmsg.Text = "Wrong Username or Password, Please Try Again";
            }//end else
        }//end else

    }//end method

    /*
     * 
     * TODO
     * 
     */
    static byte[] GenerateSalt()
    {
        byte[] salt = new byte[32];
        System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);
        return salt;
    }//end method

    /*
     * 
     * TODO
     * 
     */
    static byte[] GenerateSaltedHash(String password)//, String salt)
    {      
        // Convert the plain string pwd into bytes
        byte[] passwordbytes = System.Text.UnicodeEncoding.Unicode.GetBytes(password);

        HashAlgorithm algorithm = new SHA512Managed();

        return algorithm.ComputeHash(passwordbytes);
    }//end method

    /*
     * 
     * TODO
     * 
     */
    public static bool ComparePasswords(String str, String str2)//byte[] array1, byte[] array2)
    {
        if (!str.Equals(str2))
            return false;
        return true;
    }//end method  

    /*
     * 
     * TODO
     * 
     */
    private int RandomInteger(int min, int max)
    {
        uint scale = uint.MaxValue;
        while (scale == uint.MaxValue)
        {
            // Get four random bytes.
            byte[] four_bytes = new byte[4];
            Rand.GetBytes(four_bytes);

            // Convert that into an uint.
            scale = BitConverter.ToUInt32(four_bytes, 0);
        }//end while

        // Add min to the scaled difference between max and min.
        return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
    }//end method

    /*
     * 
     * TODO
     * 
     */
    protected void EmailError(String strmess)
    {
        try
        {
            String mypwd = "Nicaragua2017";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("reachingforenglish@gmail.com", mypwd),
                EnableSsl = true
            };
            MailMessage message = new MailMessage("reachingforenglish@gmail.com", "reachingforenglish@gmail.com", "Error Occurred", strmess);
            client.Send(message);
        }//end try
        catch (Exception e)
        {
            errormsgDB.Text = "Email failed to send! " + e.ToString();
        }//end catch
    }//end method

    /*
     * 
     * TODO
     * 
     */
    protected void btnpdf_Click(object sender, EventArgs e)
    {
        Response.Redirect("PDFViewer.aspx");
    }//end method

}//end class