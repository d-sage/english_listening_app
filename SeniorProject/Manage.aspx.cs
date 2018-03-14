﻿using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Runtime.Serialization.Json;

public partial class Manage : System.Web.UI.Page
{
    private int num;
    private String stringhelper;
    private int inthelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();


    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        //makes sure they aren't going around the login
        if(run)
        {
            ValueHiddenField.Value = num.ToString();

            string text = "";
            string server = "162.241.244.134";
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_Default";
            string password = "Default1!";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                //display the countries
                String sql = "SELECT * FROM countries";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    stringhelper = (String)rdr[0];
                    listcountry.Add(stringhelper);
                }//end while
                rdr.Close();

                //display the grades
                sql = "SELECT * FROM grades";

                cmd = new MySqlCommand(sql, connection);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    stringhelper = rdr[0].ToString();
                    listgrade.Add(stringhelper);
                }//end while
                rdr.Close();

                //display the topics
                sql = "SELECT * FROM topics";

                cmd = new MySqlCommand(sql, connection);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    stringhelper = (String)rdr[0];
                    listtopic.Add(stringhelper);
                }//end while
                rdr.Close();

                //display the lessons
                sql = "SELECT * FROM lessons";

                cmd = new MySqlCommand(sql, connection);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    stringhelper = rdr[0] + " | " + rdr[1].ToString() + " | " + rdr[2] + " | " + rdr[3] + " | " + rdr[4] + " | " + rdr[5];
                    listlesson.Add(stringhelper);
                }//end while
                rdr.Close();

            }//end try
            catch (MySqlException ex)
            {

                text += MySqlExceptionHandler(ex.Number);

                text += " bad";

            }//end catch

            connection.Close();
            errormsgDB.Text = text;

            blcountry.DataSource = listcountry;
            blcountry.DataBind();

            blgrades.DataSource = listgrade;
            blgrades.DataBind();

            bltopics.DataSource = listtopic;
            bltopics.DataBind();

            bllessons.DataSource = listlesson;
            bllessons.DataBind();
            

            //*all code goes in here*


        }//end if
        else
            Response.Redirect("Login.aspx");

    }//end main method

    #endregion Page_Load

    #region GetConnectionString

    private string GetConnectionString()
    {

        /*
         string text = "";
            string server = "162.241.244.134";
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_Default";
            string password = "Default1!";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */
        
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        return connectionString;
    }

    #endregion GetConnectionString

    #region AddCountry_Click

    protected void AddCountry_Click(object sender, EventArgs e)
    {
        
        string text = "Good";
        
        string connectionString = GetConnectionString();
        
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO countries (cid) VALUES (@cid)";
            
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", txtcountryAdd.Text);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

    }//end method

    #endregion AddCountry_Click

    #region AddTopic_Click

    protected void AddTopic_Click(object sender, EventArgs e)
    {
        
        string text = "Good";

        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "INSERT INTO topics (tid) VALUES (@tid)";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@tid", txttopicAdd.Text);

            cmd.ExecuteNonQuery();

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

    }//end method

    #endregion AddTopic_Click

    #region AddCountryGrade_Click

    #endregion AddCountryGrade_Click

    #region AddCountryGradeTopic_Click

    #endregion AddCountryGradeTopic_Click

    #region AddLesson_Click

    #endregion AddLesson_Click

    #region GetSession

    private bool GetSession()
    {
        /*if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            num = (int)Session["number"];
            if (matching)
                return true;
        }//end else*/
        return true;
    }//end method

    #endregion GetSession

    #region MySqlExceptionHandler

    private string MySqlExceptionHandler(int exceptionNum)
    {
        //When handling errors, you can your application's response based 
        //on the error number.
        //The two most common error numbers when connecting are as follows:
        //0: Cannot connect to server.
        //1045: Invalid user name and/or password.
        switch (exceptionNum)
        {
            case 0:
                return "Cannot connect to server.  Contact administrator";
            case 1045:
                return "Invalid username/password, please try again";
            default:
                return "number: " + exceptionNum;
        }//end switch
    }

    #endregion MySqlExceptionHandler


    #region Unused Ajax

    private bool SetAjaxSession()
    {

        bool isGood = true;

        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;

        /*
        string text = "Good";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_Default";
        string password = "Default1!";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        */
        string text = "Good";
        string server = "localhost";
        string database = "daricsag_ela";
        string uid = "daricsag_ela";
        string password = "english";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";


        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "DELETE FROM session";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            
            cmd.CommandText = "INSERT INTO session (id,time) VALUES(@id, @time)";
            cmd.Parameters.AddWithValue("@id", num);
            cmd.Parameters.AddWithValue("@time", secondsSinceEpoch);
            cmd.ExecuteNonQuery();

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
            isGood = false;
        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return isGood;
    }

    [WebMethod]
    public static string AjaxSendTest(object data)
    {

        /*
        Dictionary<string, object> d = (Dictionary<string, object>)data;

        List<string> keyList = new List<string>(d.Keys);
        
        object tt = d["Data"];

        object[] oa = (object[])tt;

        object one = oa[1];
        */

        return "here";

    }

    #endregion Unused Ajax


}//end class