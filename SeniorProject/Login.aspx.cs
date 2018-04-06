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

public partial class _Default : System.Web.UI.Page
{

    private RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
    private String dbUsername;
    private String dbSalt;
    private String dbPass;

    protected void Page_Load(object sender, EventArgs e)
    {

        Session["oldCid"] = "";
        Session["oldTid"] = "";
        Session["oldLid"] = "";
        Session["oldText"] = "";

        this.lblTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        int num = RandomInteger(0, 100000000);
        Session["number"] = num;

        //This is for the credentials
        /*
        string text = "";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_Default";
        string password = "Default1!";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

        /*
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */

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
                    break;

                case 1045:
                    text = "Invalid username/password, please try again";
                    break;
                default:
                    text = "number: " + ex.Number;
                    break;
            }//end switch
            text += " bad";
        }//end catch

        connection.Close();    
        errormsgDB.Text = text;

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
            //String dbPassword = "2442022411122349917696223246343111622219816422031511843150228361812417717097232104806116156106715981331392471161472361447076896854076189109105204170922261282018181126147209236142226824328583818221323126120108971201223861621451535121312919915"; //pull password from DB
            //String adminUsername = "user"; //pull username from DB
            //String saltDB = "81126147209236142226824328583818221323126120108971201223861621451535121312919915";
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

        return algorithm.ComputeHash(passwordbytes);
    }//end method

    public static bool ComparePasswords(String str, String str2)//byte[] array1, byte[] array2)
    {
        if (!str.Equals(str2))
            return false;
        return true;
    }//end method  
 
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


}//end class