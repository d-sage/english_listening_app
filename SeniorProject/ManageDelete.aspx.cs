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
    /*
     * 
     * TODO
     * 
     */
    private String stringhelper;
    private List<string> listcountry = new List<string>();
    private List<string> listgrade = new List<string>();
    private List<string> listlesson = new List<string>();
    private List<string> listtopic = new List<string>();


    #region Page_Load
    /*
     * 
     * TODO
     * 
     */
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
    /*
     * 
     * Method that contains the variables to construct the connection
     * string required to access the database.
     * 
     */
    private string GetConnectionString()
    {
        
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
    /*
     * 
     * Method that attempts to get a database connection object
     * from the connection string and return it for database
     * access
     * 
     */
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
    /*
     * 
     * Method that puts the ManageDelete page back to a 'blank slate' state
     * in which all data from the database is re-initialized
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            return;
        }

        UpdateCountries(connection);
        
        UpdateTopics(connection);

        UpdateCountryGrade(connection);

        UpdateCountryGradeTopic(connection);

        UpdateLessons(connection);

    }

    #endregion Update All Data

    #region Update Countries
    /*
     * 
     * Method for updating data related to 'countries'. Re-populating the country table
     * with the current 'country data' from the database.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }

        connection.Close();
        
    }

    #endregion Update Countries

    #region Update Topics
    /*
     * 
     * Method for updating data related to 'topics'. Re-populating the topic table
     * with the current 'topic data' from the database.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }

        connection.Close();
    }

    #endregion Update Topics

    #region Update Country_Grade
    /*
     * 
     * Method for updating data related to 'country grade'. Re-populating the country grade table
     * with the current 'country grade data' from the database.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }

        connection.Close();
    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic
    /*
     * 
     * Method for updating data related to 'country grade topic'. Re-populating the country grade topic table
     * with the current 'country grade topic data' from the database.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }

        connection.Close();
    }

    #endregion Update Country_Grade_Topic

    #region Update Lessons
    /*
     * 
     * Method for updating data related to 'lessons'. Re-populating the lesson table
     * with the current 'lesson data' from the database.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;
        }
        catch (Exception e)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }

        connection.Close();
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Table Events

    #region Country

    #region Country RowEditing
    /*
     * 
     * Method for when the 'edit' button is clicked on a 'country' row. It sets the row
     * up for editing in an editing state. Also stores the variables that can be edited
     * so if edit is confirmed it can be used for the database edit string.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Country RowEditing

    #region Country RowUpdating
    /*
     * 
     * Method used for when the 'update' button is clicked on a 'country' row in the edit state.
     * Uses the stored variables taken when the row was put in to an edit state and attempts
     * to edit the data of the row being edited.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
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
    /*
     * 
     * Method used for when the 'cancel' button is clicked on a 'country' row. Takes the row out
     * of an editing state.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Country RowCancelEdit

    #region Country RowDelete
    /*
     * 
     * Method used for when the 'delete' button is clicked on a 'country' row.
     * It checks to make sure that there are no rows in edit mode.
     * Then checks to see if there is any data associated with the given 'country'.
     * If there is not, then the 'country' can be removed.
     * If there is, then it cannot be removed.
     * 
     */
    protected void Country_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        if (!Session["oldCid"].Equals(""))
        {
            tblog.Text += Environment.NewLine + "Country: in editing mode, cannot delete";
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
                    tblog.Text += Environment.NewLine + "~Country: content associateed with Country, cannot remove";
                    good = false;
                }
            }//end cmd

        }
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
            good = false;
        }

        connection.Close();

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Country: Successfully Deleted";
        }

    }

    #endregion Country RowDelete
    
    #endregion Country

    #region Topic

    #region Topic RowEditing
    /*
     * 
     * Method for when the 'edit' button is clicked on a 'topic' row. It sets the row
     * up for editing in an editing state. Also stores the variables that can be edited
     * so if edit is confirmed it can be used for the database edit string.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Topic RowEditing

    #region Topic RowUpdating
    /*
     * 
     * Method used for when the 'update' button is clicked on a 'topic' row in the edit state.
     * Uses the stored variables taken when the row was put in to an edit state and attempts
     * to edit the data of the row being edited.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
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
    /*
     * 
     * Method used for when the 'cancel' button is clicked on a 'topic' row. Takes the row out
     * of an editing state.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Topic RowCancelEdit

    #region Topic RowDelete
    /*
     * 
     * Method used for when the 'delete' button is clicked on a 'topic' row.
     * It checks to make sure that there are no rows in edit mode.
     * Then checks to see if there is any data associated with the given 'topic'.
     * If there is not, then the 'topic' can be removed.
     * If there is, then it cannot be removed.
     * 
     */
    protected void Topic_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Session["oldTid"].Equals(""))
        {
            tblog.Text += Environment.NewLine + "Topic: in editing mode, cannot delete";
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
                    tblog.Text += Environment.NewLine + "~Topic: content associated with Topic, cannot remove";
                    good = false;
                }
            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
            good = false;
        }

        connection.Close();

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Topic: Successfully Deleted";
        }

    }

    #endregion Topic RowDelete

    #endregion Topic

    #region Country_Grade

    #region Country_Grade RowDelete
    /*
     * 
     * Method used for when the 'delete' button is clicked on a 'country grade' row.
     * It checks to see if there is any data associated with the given 'country grade'.
     * If there is not, then the 'country grade' can be removed.
     * If there is, then it cannot be removed.
     * 
     */
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
                    tblog.Text += Environment.NewLine + "~Country_grade: content associateed with Country_Grade, cannot remove";
                    good = false;
                }
            }//end cmd

        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
            good = false;
        }

        connection.Close();
        
        if(good)
        {
            tblog.Text += Environment.NewLine + "~Country Grade: Successfully Deleted";
        }

    }

    #endregion Country_Grade RowDelete

    #endregion Country_Grade

    #region Country_Grade_Topic

    #region Country_Grade_Topic RowDelete
    /*
     * 
     * Method used for when the 'delete' button is clicked on a 'country grade topic' row.
     * It delegates to a helper method called 'RemoveTopic' to do work with removing data
     * associated with the chosen 'country grade topic'.
     * See 'RemoveTopic' method
     * If that succeeds, then the record for the 'country grade topic' can be removed from
     * the database.
     * 
     */
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
                tblog.Text += Environment.NewLine + text;

                //TODO: email
                good = false;
            }
            catch (ArgumentException ae)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
                good = false;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact help";
                good = false;
            }

            connection.Close();

        }
        else
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: topic removal failed, contact help";
            good = false;
        }

        if(good)
        {
            tblog.Text += Environment.NewLine + "~Country_Grade_Topic: Successfully Deleted";
        }
        
    }

    #endregion Country_Grade_Topic RowDelete

    #region Country_Grade_Topic RemoveTopic
    /*
     * 
     * Method used as a helper to remove data associated with a 'country grade topic'.
     * Gets a list of all 'lid' assocaited with the given 'country grade topic'.
     * Then delegates to another helper method 'LessonDelete' that does work to
     * remove any data associated with a lesson.
     * See 'LessonDelete' method.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            connection.Close();
            return false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            connection.Close();
            return false; ;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
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
    
    #endregion Country_Grade_Topic

    #region Lesson

    #region Lesson DataBound
    /*
     * 
     * Method used for when the databound event fires on the lesson table so that
     * some custom styles can be added to the table.
     * 
     */
    protected void Lesson_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Taken out: word-break:break-all;word-wrap:break-word;
            e.Row.Cells[4].Attributes.Add("style", "width:600px;");
        }
    }

    #endregion Lesson DataBound

    #region Lesson RowEditing
    /*
     * 
     * Method for when the 'edit' button is clicked on a 'lesson' row. It sets the row
     * up for editing in an editing state. Also stores the variables that can be edited
     * so if edit is confirmed it can be used for the database edit string.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Lesson RowEditing

    #region Lesson RowUpdating
    /*
     * 
     * Method used for when the 'update' button is clicked on a 'lesson' row in the edit state.
     * Uses the stored variables taken when the row was put in to an edit state and attempts
     * to edit the data of the row being edited.
     * 
     */
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
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            good = false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            good = false;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
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
    /*
     * 
     * Method used for when the 'cancel' button is clicked on a 'lesson' row. Takes the row out
     * of an editing state.
     * 
     */
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
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
        }
    }

    #endregion Lesson RowCancelEdit

    #region Lesson RowDelete
    /*
     * 
     * Method used for when the 'delete' button is clicked on a 'lesson' row.
     * It delegates to a helper method called 'LessonDelete' to do work with removing data
     * associated with the chosen 'lesson'.
     * See 'LessonDelete' method
     * If that succeeds, then the 'lesson' data is updated.
     * 
     */
    protected void Lesson_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        if (!Session["oldLid"].Equals(""))
        {
            tblog.Text += Environment.NewLine + "Lesson: in editing mode, cannot delete";
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
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
                good = false;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact help";
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
    /*
     * 
     * Method used for removing data associated with a given 'lesson'.
     * This involves delegating to a helper method 'RemoveFile' that deals with
     * removing the file assocaied with the given 'lesson'.
     * See 'RemoveFile' method.
     * If that succeeds, then the actual record for the given 'lesson' can be removed.
     * If failure occurs, then help will need to be contacted.
     * 
     */
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
                tblog.Text += Environment.NewLine + text;

                //TODO: email
                connection.Close();
                return false;
            }
            catch (ArgumentException ae)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
                connection.Close();
                return false; ;
            }
            catch (Exception ex)
            {
                //TODO: email
                tblog.Text += Environment.NewLine + "~Error: contact help";
                connection.Close();
                return false;
            }

            connection.Close();
            
        }
        else
        {
            tblog.Text += Environment.NewLine + "~Lesson: file removal failed, contact help";
            return false;
        }

        return true;
    }

    #endregion LessonDelete()

    #region Lesson RemoveFile
    /*
     * 
     * Method used for removing a file that is associated with the given 'lesson'.
     * It checks to see if there is more than one path associated with the file
     * that is associated with the given 'lesson'.
     * If there is only 1 path (itself) then it will attempt to remove the file.
     * If there is more than 1 path (another lesson uses it), then it will not remove the file.
     * 
     * 
     */
    private bool RemoveFile(string country, string grade, string topic, string lid)
    {

        string filename = "";
        string ext = "";
        int pathCount = 1;

        bool good = true;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(cid) as count, filename, ext FROM lessons WHERE path = (" +
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

                    ext = (string)rdr[2];
                }

            }//end rdr


        }//end try
        catch (MySqlException mse)
        {
            string text = MySqlExceptionNumberHandler(mse.Number);
            tblog.Text += Environment.NewLine + text;

            //TODO: email
            connection.Close();
            return false;
        }
        catch (ArgumentException ae)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: could not connect to database | contact help";
            connection.Close();
            return false; ;
        }
        catch (Exception ex)
        {
            //TODO: email
            tblog.Text += Environment.NewLine + "~Error: contact help";
            connection.Close();
            return false;
        }

        connection.Close();
        
        //if there is only one path count (the given lesson itseld)
        //then remove the file
        if (pathCount == 1)
        {
            try
            {
                string physicalFolderPath = ext == "mp3" ? Server.MapPath(".//Audio//") : Server.MapPath(".//PDF//");
                string physicalFilePath = physicalFolderPath + filename;

                if (System.IO.File.Exists(physicalFilePath))
                {
                    try
                    {
                        System.IO.File.Delete(physicalFilePath);
                    }
                    catch (Exception ex)
                    {
                        good = false;
                        tblog.Text += Environment.NewLine + "~Lesson: file removal error, contact help";
                    }
                }
                else
                {
                    good = false;
                    tblog.Text += Environment.NewLine + "~Lesson: file did not exist, contact help";
                }
            }
            catch (Exception e)
            {
                good = false;
                tblog.Text += Environment.NewLine + "~Error: contact help";
            }
        }

        return good;
    }

    #endregion Lesson RemoveFile
    
    #endregion Lesson

    #endregion Table Events

    #region Helpers

    #region Clear Log
    /*
     * 
     * Event assocatied with clicking the 'clear log' button
     * to revert the log box back to a clean slate
     * 
     */
    protected void ClearLog_Click(object sender, EventArgs e)
    {
        tblog.Text = "";
    }

    #endregion Clear Log

    #region MySqlExceptionHandler
    /*
     * 
     * Method used for determining what the sql error number corresponds to.
     * It will return the text associated with that error
     * 
     */
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
                return "~Error: Cannot connect to server | Contact help";
            case 1042:
                return "~Error: Cannot resolve server name  | Contact help";
            case 1045:
                return "~Error: Invalid username/password | Contact help";
            case 1062:
                return "~Duplicate Entry (certain fields cannot be the same), please try again";
            default:
                return "~Error: number: " + exceptionNum + " | Contact help";
        }//end switch
    }

    #endregion MySqlExceptionHandler

    #region GetSession()
    /*
     * 
     * TODO
     * 
     */
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

    #region Email Method
    /*
     * 
     * TODO
     * 
     */
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
            tblog.Text += Environment.NewLine + "Email failed to send! " + e.ToString();
        }//end catch
    }//end method

    #endregion Email Method

    #endregion Helpers

    #region Button Click Events

    /*
     * 
     * TODO
     * 
     */
    protected void Btnaddlink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAdd.aspx");
    }//end method


    /*
     * 
     * TODO
     * 
     */
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("PDFViewer.aspx");
    }//end method

    #endregion Button Click Events

}//end class