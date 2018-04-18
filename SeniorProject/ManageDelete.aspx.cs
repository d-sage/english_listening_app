using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Credit for the editing/deleting of gridviews: https://www.aspsnippets.com/Articles/Edit-Update-and-Delete-in-ASPNet-GridView-with-AutoGenerateColumns-True-using-C-and-VBNet.aspx
//Credit for file upload: https://forums.asp.net/t/1840106.aspx?how+to+upload+a+file+and+save+in+server+folder

public partial class ManageDelete : System.Web.UI.Page
{
    private String stringhelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();


    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();

        //makes sure they aren't going around the login
        if (run)
        {

            

            if (!Page.IsPostBack)
            {
                UpdateAllData();
                //TODO
                //temp?
                Session["oldCid"] = "";
                Session["oldTid"] = "";
                Session["oldLid"] = "";
                Session["oldText"] = "";
                //temp?
            }
            
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
            string server = "162.241.244.134";  //162.241.244.134
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_admin";
            string password = "Admin123";
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

        
        string server = "mysql5018.site4now.net";
        string database = "db_a38d8d_lambe";
        string uid = "a38d8d_lambe";
        string password = "Lambejor000";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        

        return connectionString;
    }

    #endregion GetConnectionString

    #region Get SQL Connection

    private MySqlConnection GetSqlConnection()
    {
        string connectionString = GetConnectionString();
        MySqlConnection conn = null;

        try
        {
            conn = new MySqlConnection(connectionString);
        }
        catch (Exception e)
        {
            throw new ArgumentException();
        }

        return conn;
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        String text = "";

        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();
        }
        catch (ArgumentException ae)
        {
            EmailError(text);
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }

        UpdateCountries(connection);

        //UpdateGrades(connection);

        UpdateTopics(connection);

        UpdateCountryGrade(connection);

        UpdateCountryGradeTopic(connection);

        UpdateLessons(connection);

    }

    #endregion Update All Data

    #region Update Countries

    private void UpdateCountries(MySqlConnection connection)
    {

        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid FROM countries ORDER BY cid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridCountry.DataSource = dt;
            gridCountry.DataBind();
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
        
    }

    #endregion Update Countries

    #region Update Grades

    /*private void UpdateGrades(MySqlConnection connection)
    {
        connection.Open();
        //TODO
        //BlanksOnDropList(dlCGcountry);

        String sql = "SELECT * FROM grades";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = rdr[0].ToString();
            listgrade.Add(stringhelper);
        }//end while
        rdr.Close();
        connection.Close();

        blgrades.DataSource = listgrade;
        blgrades.DataBind();
    }*/

    #endregion Update Grades

    #region Update Topics

    private void UpdateTopics(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            String sql = "SELECT tid FROM topics ORDER BY tid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            
            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridTopic.DataSource = dt;
            gridTopic.DataBind();

        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
    }

    #endregion Update Topics

    #region Update Country_Grade

    private void UpdateCountryGrade(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid,gid FROM country_grade_relationship ORDER BY cid,gid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            
            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridCountryGrade.DataSource = dt;
            gridCountryGrade.DataBind();
        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic

    private void UpdateCountryGradeTopic(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            //display the countries
            String sql = "SELECT cid,gid,tid FROM country_grade_topic_relation ORDER BY cid,gid,tid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);
            
            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridCountryGradeTopic.DataSource = dt;
            gridCountryGradeTopic.DataBind();

        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
    }

    #endregion Update Country_Grade_Topic

    #region Update Lessons

    private void UpdateLessons(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            String sql = "SELECT cid,gid,tid,lid,text,filename FROM lessons  ORDER BY cid,gid,tid,lid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridLesson.DataSource = dt;
            gridLesson.DataBind();

        }
        catch (MySqlException mse)
        {
            //TODO: email
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();

        //bllessons.DataSource = listlesson;
        //bllessons.DataBind();
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Table Events

    #region Country

    #region Country RowEditing

    protected void Country_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["oldCid"] = gridCountry.Rows[e.NewEditIndex].Cells[0].Text;
        
        gridCountry.EditIndex = e.NewEditIndex;

        try
        {
            UpdateCountries(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Country RowEditing

    #region Country RowUpdating

    protected void Country_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridCountry.Rows[e.RowIndex];
        string newCid = ((TextBox)(row.Cells[0].Controls[0])).Text;
        string oldCid = (string)Session["oldCid"];

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "UPDATE countries SET cid = (@newCid) WHERE cid = (@oldCid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@newCid", newCid);
                cmd.Parameters.AddWithValue("@oldCid", oldCid);

                cmd.ExecuteNonQuery();

            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();

        Session["oldCid"] = "";
        gridCountry.EditIndex = -1;

        if (good)
        {
            tblog.Text += Environment.NewLine + "~Country: Successfully Edited";
            UpdateAllData();
        }
    }

    #endregion Country RowUpdating
    
    #region Country RowCancelEdit

    protected void Country_OnRowCancelingEdit(object sender, EventArgs e)
    {
        Session["oldCid"] = "";
        gridCountry.EditIndex = -1;

        try
        {
            UpdateCountries(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Country RowCancelEdit

    #region Country RowDelete

    protected void Country_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        if (!Session["oldCid"].Equals(""))
        {
            tblog.Text += "Country: in editing mode, cannot delete";
            return;
        }

        GridViewRow row = gridCountry.Rows[e.RowIndex];
        string countryToDelete = row.Cells[0].Text;
        
        bool canRemove = false;
        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "SELECT COUNT(cid) as count FROM country_grade_relationship WHERE cid = (@cid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@cid", countryToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM countries WHERE cid = (@cid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    UpdateCountries(connection);
                }
                else
                {
                    tblog.Text += "~Country: content associateed with Country, cannot remove";
                    good = false;
                }
            }//end cmd

        }
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Country: Successfully Deleted";
        }

    }

    #endregion Country RowDelete


    #region obsolete code
    #region Country Command (obsolete)

    /*
     *  Cannot delete a country if there is any data that is associated with this given country
     *  Then will update country data
     */
    protected void Country_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country: NOT IMPLEMENTED, switched to different approach');", true);

        return;
        int rowIndex = int.Parse(e.CommandArgument.ToString());

        bool canChange = false;

        if(e.CommandName.Equals("e"))
        {
            canChange = CountryEdit(rowIndex);
        }
        else if(e.CommandName.Equals("d"))
        {
            canChange = CountryDelete(rowIndex);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country: unrecognized command, contact admin');", true);
            return;
        }

        
        if (canChange)
        {
            UpdateCountries(GetSqlConnection());
        }

    }

    #endregion Country Command (obsolete)

    #region Country Delete (obsolete)

    private bool CountryDelete(int rowIndex)
    {

        GridViewRow row = gridCountry.Rows[rowIndex];
        string countryToDelete = row.Cells[0].Text;

        string text = "";
        bool canRemove = false;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(cid) as count FROM country_grade_relationship WHERE cid = (@cid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@cid", countryToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM countries WHERE cid = (@cid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country: content associateed with Country, cannot remove');", true);
                }
            }//end cmd

        }//end try
        catch (MySqlException ex)
        {

            canRemove = false;

            text += MySqlExceptionNumberHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Country Delete (obsolete)

    #region Country Edit (obsolete)

    private bool CountryEdit(int rowIndex)
    {

        GridViewRow row = gridCountry.Rows[rowIndex];
        string countryToDelete = row.Cells[0].Text;

        //gridCountry.EditIndex = rowIndex;

        //TODO




        //ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;
    }

    #endregion Country Edit (obsolete)
    #endregion obsolete code

    #endregion Country

    #region Topic
    
    #region Topic RowEditing

    protected void Topic_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["oldTid"] = gridTopic.Rows[e.NewEditIndex].Cells[0].Text;
        gridTopic.EditIndex = e.NewEditIndex;

        try
        {
            UpdateTopics(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Topic RowEditing

    #region Topic RowUpdating

    protected void Topic_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridTopic.Rows[e.RowIndex];
        string newTid = ((TextBox)(row.Cells[0].Controls[0])).Text;
        string oldTid = (string)Session["oldTid"];

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "UPDATE topics SET tid = (@newTid) WHERE tid = (@oldTid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@newTid", newTid);
                cmd.Parameters.AddWithValue("@oldTid", oldTid);

                cmd.ExecuteNonQuery();

            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();
        
        Session["oldTid"] = "";
        gridTopic.EditIndex = -1;

        if (good)
        {
            tblog.Text += Environment.NewLine + "~Topic: Successfully Edited";
            UpdateAllData();
        }
    }

    #endregion Topic RowUpdating

    #region Topic RowCancelEdit

    protected void Topic_OnRowCancelingEdit(object sender, EventArgs e)
    {
        Session["oldTid"] = "";
        gridTopic.EditIndex = -1;

        try
        {
            UpdateTopics(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Topic RowCancelEdit

    #region Topic RowDelete

    protected void Topic_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Session["oldTid"].Equals(""))
        {
            tblog.Text += "Topic: in editing mode, cannot delete";
            return;
        }
        
        GridViewRow row = gridTopic.Rows[e.RowIndex];
        string topicToDelete = row.Cells[0].Text;
        
        bool canRemove = false;
        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "SELECT COUNT(tid) as count FROM country_grade_topic_relation WHERE tid = (@tid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@tid", topicToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM topics WHERE tid = (@tid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    UpdateTopics(connection);
                }
                else
                {
                    tblog.Text += "~Topic: content associated with Topic, cannot remove";
                    good = false;
                }
            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Topic: Successfully Deleted";
        }

    }

    #endregion Topic RowDelete


    #region (obsolete)
    #region Topic Command (obsolete)

    /*
     *  Cannot delete a topic if there is any data that is associated with this given topic
     *  Then will update topic data
     */
    protected void Topic_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        return;

        int rowIndex = int.Parse(e.CommandArgument.ToString());

        bool canChange = false;

        if (e.CommandName.Equals("e"))
        {
            canChange = TopicEdit(rowIndex);
        }
        else if (e.CommandName.Equals("d"))
        {
            canChange = TopicDelete(rowIndex);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Topic: unrecognized command, contact admin');", true);
            return;
        }


        if (canChange)
        {
            UpdateTopics(GetSqlConnection());
        }

    }

    #endregion Topic Command

    #region Topic Delete (obsolete)

    private bool TopicDelete(int rowIndex)
    {

        GridViewRow row = gridTopic.Rows[rowIndex];
        string topicToDelete = row.Cells[0].Text;

        string text = "";
        bool canRemove = false;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(tid) as count FROM country_grade_topic_relation WHERE tid = (@tid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@tid", topicToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM topics WHERE tid = (@tid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Topic: content associateed with Topic, cannot remove');", true);
                }
            }//end cmd

        }//end try
        catch (MySqlException ex)
        {
            canRemove = false;

            text += MySqlExceptionNumberHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Topic Delete

    #region Topic Edit (obsolete)

    private bool TopicEdit(int rowIndex)
    {
        //TODO

        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;

    }

    #endregion Topic Edit
    #endregion (obsolete)

    #endregion Topic
    
    #region Country_Grade
    
    #region Country_Grade RowDelete

    protected void CountryGrade_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        GridViewRow row = gridCountryGrade.Rows[e.RowIndex];
        string countryToDelete = row.Cells[0].Text;
        string gradeToDelete = row.Cells[1].Text;
        
        bool canRemove = false;
        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "SELECT COUNT(cid) as count FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@cid", countryToDelete);
                cmd.Parameters.AddWithValue("@gid", gradeToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM country_grade_relationship WHERE cid = (@cid) AND gid = (@gid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    UpdateCountryGrade(connection);
                }
                else
                {
                    tblog.Text += "~Country_grade: content associateed with Country_Grade, cannot remove";
                    good = false;
                }
            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();
        
        if(good)
        {
            tblog.Text += Environment.NewLine + "~Country Grade: Successfully Deleted";
        }

    }

    #endregion Country_Grade RowDelete


    #region (obsolete)
    #region Country_Grade Command (obsolete)

    /*
     *  Cannot delete a country_grade if there is any data that is associated with this given country_grade
     *  Then will update country_grade data
     */
    protected void CountryGrade_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        return;

        int rowIndex = int.Parse(e.CommandArgument.ToString());

        bool canChange = false;

        if (e.CommandName.Equals("d"))
        {
            canChange = Country_GradeDelete(rowIndex);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country_Grade: unrecognized command, contact admin');", true);
            return;
        }


        if (canChange)
        {
            UpdateCountryGrade(GetSqlConnection());
        }

    }

    #endregion Country_Grade Command

    #region Country_Grade Delete (obsolete)

    private bool Country_GradeDelete(int rowIndex)
    {

        GridViewRow row = gridCountryGrade.Rows[rowIndex];
        string countryToDelete = row.Cells[0].Text;
        string gradeToDelete = row.Cells[1].Text;

        string text = "";
        bool canRemove = false;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(cid) as count FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@cid", countryToDelete);
                cmd.Parameters.AddWithValue("@gid", gradeToDelete);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {

                    //if count is zero (0) then can remove
                    if (rdr.Read())
                        canRemove = Convert.ToInt32(rdr[0]) == 0 ? true : false;

                }//end rdr

                if (canRemove)
                {
                    sql = "DELETE FROM country_grade_relationship WHERE cid = (@cid) AND gid = (@gid)";

                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country_grade: content associateed with Country_Grade, cannot remove');", true);
                }
            }//end cmd

        }//end try
        catch (MySqlException ex)
        {
            canRemove = false;

            text += MySqlExceptionNumberHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Country_Grade Delete
    #endregion (obsolete)

    #endregion Country_Grade

    #region Country_Grade_Topic

    #region Country_Grade_Topic RowDelete

    protected void CountryGradeTopic_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        GridViewRow row = gridCountryGradeTopic.Rows[e.RowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;

        bool good = true;

        if (RemoveTopic(country, grade, topic))
        {
            MySqlConnection connection = null;

            try
            {
                connection = GetSqlConnection();

                connection.Open();

                String sql = "DELETE FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {

                    cmd.Parameters.AddWithValue("@cid", country);
                    cmd.Parameters.AddWithValue("@gid", grade);
                    cmd.Parameters.AddWithValue("@tid", topic);

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    UpdateCountryGradeTopic(connection);
                    UpdateLessons(connection);

                }//end cmd

            }//end try
            catch (MySqlException mse)
            {
                string text = MySqlExceptionNumberHandler(mse.Number);
                tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

                //TODO: email
                good = false;
            }
            catch (ArgumentException ae)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
                good = false;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact admin";
                good = false;
            }

            connection.Close();

        }
        else
        {
            tblog.Text += "~Country_Grade_Topic: topic removal failed, contact admin";
            good = false;
        }

        if(good)
        {
            tblog.Text += "~Country_Grade_Topic: Successfully Deleted";
        }
        
    }

    #endregion Country_Grade_Topic RowDelete

    #region Country_Grade_Topic RemoveTopic
    private bool RemoveTopic(string country, string grade, string topic)
    {

        List<String> lids = new List<string>();
        
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "SELECT lid FROM lessons WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid);";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@cid", country);
                cmd.Parameters.AddWithValue("@gid", grade);
                cmd.Parameters.AddWithValue("@tid", topic);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lids.Add((string)rdr[0]);
                    }
                }//end rdr
            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            connection.Close();
            return false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            connection.Close();
            return false; ;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            connection.Close();
            return false;
        }

        connection.Close();
        
        foreach (string lid in lids)
        {
            if (!LessonDelete(country, grade, topic, lid))
            {
                return false;
            }
        }

        return true;
    }
    #endregion Country_Grade_Topic RemoveTopic


    #region (obsolete)
    #region Country_Grade_Topic Command (obsolete)

    protected void CountryGradeTopic_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        return;

        int rowIndex = int.Parse(e.CommandArgument.ToString());

        bool canChange = false;

        if (e.CommandName.Equals("d"))
        {
            canChange = Country_Grade_TopicDelete(rowIndex);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country_Grade_Topic: unrecognized command, contact admin');", true);
            return;
        }


        if (canChange)
        {
            UpdateCountryGradeTopic(GetSqlConnection());
            UpdateLessons(GetSqlConnection());
        }

    }

    #endregion Country_Grade_Topic Command

    #region Country_Grade_Topic Delete (obsolete)

    private bool Country_Grade_TopicDelete(int rowIndex)
    {
        
        string text = "";
        bool good = true;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        GridViewRow row = gridCountryGradeTopic.Rows[rowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;

        if(true)//RemoveTopic(country, grade, topic))
        {
            try
            {
                connection.Open();

                String sql = "DELETE FROM country_grade_topic_relation WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {

                    cmd.Parameters.AddWithValue("@cid", country);
                    cmd.Parameters.AddWithValue("@gid", grade);
                    cmd.Parameters.AddWithValue("@tid", topic);
                    
                    cmd.ExecuteNonQuery();

                }//end cmd

            }//end try
            catch (MySqlException ex)
            {
                good = false;

                text += MySqlExceptionNumberHandler(ex.Number);

                text += " bad";

            }//end catch

            connection.Close();

            errormsgDB.Text = text;
            
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Country_Grade_Topic: topic removal failed, contact admin');", true);
            good = false;
        }

        return good;
        
    }
    
    #endregion Country_Grade_Topic Delete
    #endregion (obsolete)

    #endregion Country_Grade_Topic

    #region Lesson

    #region Lesson DataBound
    protected void Lesson_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Attributes.Add("style", "width:500px;word-break:break-all;word-wrap:break-word;");
        }
    }
    #endregion Lesson DataBound

    #region Lesson RowEditing

    protected void Lesson_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["oldLid"] = gridLesson.Rows[e.NewEditIndex].Cells[3].Text;
        Session["oldText"] = gridLesson.Rows[e.NewEditIndex].Cells[4].Text;

        gridLesson.EditIndex = e.NewEditIndex;

        try
        {
            UpdateLessons(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Lesson RowEditing

    #region Lesson RowUpdating

    protected void Lesson_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridLesson.Rows[e.RowIndex];
        string newLid = ((TextBox)(row.Cells[3].Controls[0])).Text;
        string oldLid = (string)Session["oldLid"];
        string newText = ((TextBox)(row.Cells[4].Controls[0])).Text;
        string cid = row.Cells[0].Text;
        string gid = row.Cells[1].Text;
        string tid = row.Cells[2].Text;

        bool good = true;
        MySqlConnection connection = null;

        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "UPDATE lessons SET lid = (@newLid), text = (@newText) WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid) AND lid = (@oldLid)";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@newLid", newLid);
                cmd.Parameters.AddWithValue("@newText", newText);
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@gid", gid);
                cmd.Parameters.AddWithValue("@tid", tid);
                cmd.Parameters.AddWithValue("@oldLid", oldLid);

                cmd.ExecuteNonQuery();

                connection.Close();

                Session["oldLid"] = "";
                gridLesson.EditIndex = -1;
                UpdateLessons(connection);

            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            good = false;
        }

        connection.Close();

        if (good)
        {
            tblog.Text += Environment.NewLine + "~Lesson: Successfully Edited";
        }

    }

    #endregion Lesson RowUpdating

    #region Lesson RowCancelEdit

    protected void Lesson_OnRowCancelingEdit(object sender, EventArgs e)
    {
        Session["oldLid"] = "";
        gridLesson.EditIndex = -1;

        try
        {
            UpdateLessons(GetSqlConnection());
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
        }
    }

    #endregion Lesson RowCancelEdit

    #region Lesson RowDelete

    protected void Lesson_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        if (!Session["oldLid"].Equals(""))
        {
            tblog.Text += "Lesson: in editing mode, cannot delete";
            return;
        }
        
        GridViewRow row = gridLesson.Rows[e.RowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;
        string lid = row.Cells[3].Text;

        bool good = true;

        if(LessonDelete(country, grade, topic, lid))
        {
            try
            {
                UpdateLessons(GetSqlConnection());
            }
            catch (ArgumentException ae)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
                good = false;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact admin";
                good = false;
            }
        }
        else
        {
            tblog.Text += Environment.NewLine + "~Lesson: Failed to Delete";
            good = false;
        }

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Lesson: Successfully Deleted";
        }

    }

    #endregion Lesson RowDelete

    #region LessonDelete()

    private bool LessonDelete(string country, string grade, string topic, string lid)
    {
        
        if (RemoveFile(country, grade, topic, lid))
        {
            MySqlConnection connection = null;

            try
            {
                connection = GetSqlConnection();

                connection.Open();

                String sql = "DELETE FROM lessons WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid) AND lid = (@lid)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {

                    cmd.Parameters.AddWithValue("@cid", country);
                    cmd.Parameters.AddWithValue("@gid", grade);
                    cmd.Parameters.AddWithValue("@tid", topic);
                    cmd.Parameters.AddWithValue("@lid", lid);

                    cmd.ExecuteNonQuery();

                }//end cmd

            }//end try
            catch (MySqlException mse)
            {
                string text = MySqlExceptionNumberHandler(mse.Number);
                tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

                //TODO: email
                connection.Close();
                return false;
            }
            catch (ArgumentException ae)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
                connection.Close();
                return false; ;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact admin";
                connection.Close();
                return false;
            }

            connection.Close();
            
        }
        else
        {
            tblog.Text += Environment.NewLine + "~Lesson: file removal failed, contact admin";
            return false;
        }

        return true;
    }

    #endregion LessonDelete()

    #region Lesson RemoveFile
    private bool RemoveFile(string country, string grade, string topic, string lid)
    {

        string filename = "";
        int pathCount = 1;

        bool good = true;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(cid) as count, filename FROM lessons WHERE path = (" +
                                    "SELECT path FROM lessons WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid) AND lid = (@lid));";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);
            cmd.Parameters.AddWithValue("@tid", topic);
            cmd.Parameters.AddWithValue("@lid", lid);

            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {

                //if count is zero (0) then can remove file
                if (rdr.Read())
                {
                    pathCount = Convert.ToInt32(rdr[0]);

                    filename = (string)rdr[1];
                }

            }//end rdr


        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + "~Error: " + text + " | contact admin";

            //TODO: email
            connection.Close();
            return false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            connection.Close();
            return false; ;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact admin";
            connection.Close();
            return false;
        }

        connection.Close();
        
        string physicalAudioPath = Server.MapPath(".//Audio//");
        string physicalFilePath = physicalAudioPath + filename;

        if (pathCount == 1)
        {
            try
            {
                if (System.IO.File.Exists(physicalFilePath))
                {
                    try
                    {
                        System.IO.File.Delete(physicalFilePath);
                    }
                    catch (Exception ex)
                    {
                        good = false;
                        tblog.Text += "~Lesson: file removal error, contact admin";
                    }
                }
                else
                {
                    tblog.Text += "~Lesson: file did not exist, contact admin";
                }
            }
            catch (Exception e)
            {
                good = false;
                tblog.Text += "~Error: contact admin";
            }
        }

        return good;
    }
    #endregion Lesson RemoveFile


    #region (obsolete)
    #region Lesson Command (obsolete)

    /*
     *  Cannot delete a lesson
     */
    protected void Lesson_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        return;

        int rowIndex = int.Parse(e.CommandArgument.ToString());

        bool canChange = false;

        if (e.CommandName.Equals("e"))
        {
            canChange = LessonEdit(rowIndex);
        }
        else if (e.CommandName.Equals("d"))
        {
            canChange = LessonDelete(rowIndex);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Lesson: unrecognized command, contact admin');", true);
            return;
        }


        if (canChange)
        {
            UpdateLessons(GetSqlConnection());
        }

    }

    #endregion Lesson Command

    #region Lesson Delete (obsolete)

    private bool LessonDelete(int rowIndex)
    {
        return false;
        GridViewRow row = gridLesson.Rows[rowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;
        string lid = row.Cells[3].Text;

        return LessonDelete(country, grade, topic, lid);
        
    }
    
    #endregion Lesson Delete

    #region Lesson Edit (obsolete)

    private bool LessonEdit(int rowIndex)
    {
        //TODO

        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;

    }

    #endregion Lesson Edit
    #endregion (obsolete)

    #endregion Lesson

    #endregion Table Events

    #region Helpers

    #region Clear Log

    protected void ClearLog_Click(object sender, EventArgs e)
    {
        tblog.Text = "";
    }

    #endregion Clear Log

    #region MySqlExceptionHandler

    private string MySqlExceptionNumberHandler(int exceptionNum)
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
            case 1042:
                return "Cannot resolve server name";
            case 1045:
                return "Invalid username/password, please try again";
            case 1062:
                return "Duplicate Entry";
            default:
                return "number: " + exceptionNum;
        }//end switch
    }

    #endregion MySqlExceptionHandler

    #region GetSession()

    private bool GetSession()
    {
        if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            if (matching)
                return true;
        }//end else
        return true;
    }//end method

    #endregion GetSession()

    protected void Btnaddlink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAdd.aspx");
    }//end method

    protected void EmailError(String strmess)
    {
        try
        {
            String mypwd = "";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("yourEmail@gmail.com", mypwd),
                EnableSsl = true
            };
            MailMessage message = new MailMessage("yourEmail@gmail.com", "EmailToSendTo", "Error Occurred", strmess);
            client.Send(message);
        }//end try
        catch (Exception e)
        {
            tblog.Text += "Email failed to send! " + e.ToString();
        }//end catch
    }//end method

    #endregion Helpers

}//end class