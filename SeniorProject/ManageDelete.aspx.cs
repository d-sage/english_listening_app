using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            }



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
            string server = "localhost";  //162.241.244.134
            string database = "jordape8_EnglishApp";
            string uid = "jordape8_admin";
            string password = "Admin123";
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

    #region Get SQL Connection

    private MySqlConnection GetSqlConnection()
    {
        
        string text = "";
        string server = "162.241.244.134";
        string database = "jordape8_EnglishApp";
        string uid = "jordape8_admin";
        string password = "Admin123";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        

        //local testing >>>
        //string connectionString = GetConnectionString();
        //local testing <<<

        return new MySqlConnection(connectionString);
    }

    #endregion Get SQL Connection

    #region UpdateData

    #region Update All Data

    private void UpdateAllData()
    {

        String text = "Good";

        MySqlConnection connection = GetSqlConnection();

        try
        {

            UpdateCountries(connection);

            //UpdateGrades(connection);

            UpdateTopics(connection);

            UpdateCountryGrade(connection);

            UpdateCountryGradeTopic(connection);

            UpdateLessons(connection);

        }//end try
        catch (MySqlException ex)
        {

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        errormsgDB.Text = text;

    }

    #endregion Update All Data

    #region Update Countries

    private void UpdateCountries(MySqlConnection connection)
    {

        connection.Open();

        //display the countries
        String sql = "SELECT cid FROM countries";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            //add to bulletlist display
            stringhelper = (String)rdr[0];
            listcountry.Add(stringhelper);

            //add to countries to table
            //dlCGcountry.Items.Add(new ListItem((String)rdr[0], (String)rdr[0]));

        }//end while*/



        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridCountry.DataSource = dt;
        gridCountry.DataBind();
        //test




        //rdr.Close();
        connection.Close();

        //databind countries to bullet lists
        //blcountry.DataSource = listcountry;
        //blcountry.DataBind();

        //databind countries to table
        //gridCountry.DataSource = listcountry;
        //gridCountry.DataBind();
        
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
        connection.Open();
        //TODO
        //BlanksOnDropList(dlCGcountry);

        String sql = "SELECT * FROM topics";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = (String)rdr[0];
            listtopic.Add(stringhelper);
        }//end while*/


        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridTopic.DataSource = dt;
        gridTopic.DataBind();
        //test


        //rdr.Close();
        connection.Close();

        //bltopics.DataSource = dt;
        //bltopics.DataBind();
    }

    #endregion Update Topics

    #region Update Country_Grade

    private void UpdateCountryGrade(MySqlConnection connection)
    {

        connection.Open();

        //display the countries
        String sql = "SELECT * FROM country_grade_relationship";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {

            //add to countrygrade droplist
            //dlCGTcountrygrade.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString(), (String)rdr[0] + " " + rdr[1].ToString()));

        }//end while
        rdr.Close();*/

        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridCountryGrade.DataSource = dt;
        gridCountryGrade.DataBind();
        //test

        connection.Close();

    }

    #endregion Update Country_Grade

    #region Update Country_Grade_Topic

    private void UpdateCountryGradeTopic(MySqlConnection connection)
    {

        connection.Open();

        //display the countries
        String sql = "SELECT * FROM country_grade_topic_relation";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {

            //add to countrygrade droplist
            //dlLesson.Items.Add(new ListItem((String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2], (String)rdr[0] + " " + rdr[1].ToString() + " " + (String)rdr[2]));

        }//end while
        rdr.Close();*/


        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridCountryGradeTopic.DataSource = dt;
        gridCountryGradeTopic.DataBind();
        //test


        connection.Close();

    }

    #endregion Update Country_Grade_Topic

    #region Update Lessons

    private void UpdateLessons(MySqlConnection connection)
    {
        connection.Open();

        String sql = "SELECT cid,gid,tid,lid,filename FROM lessons";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        /*MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            stringhelper = rdr[0] + " | " +
                            rdr[1].ToString() + " | " +
                            rdr[2] + " | " +
                            rdr[3] + " | " +
                            rdr[4] + " | " +
                            rdr[5] + " | " +
                            rdr[6];
            listlesson.Add(stringhelper);
        }//end while
        rdr.Close();*/


        //test
        DataTable dt = new DataTable();
        MySqlDataAdapter src = new MySqlDataAdapter(cmd);
        src.Fill(dt);
        gridLesson.DataSource = dt;
        gridLesson.DataBind();
        //test


        connection.Close();

        bllessons.DataSource = listlesson;
        bllessons.DataBind();
    }

    #endregion Update Lessons

    #endregion UpdateData

    #region Table Events

    #region Country

    #region Country Command

    /*
     *  Cannot delete a country if there is any data that is associated with this given country
     *  Then will update country data
     */
    protected void Country_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

    #endregion Country Command

    #region Country Delete

    private bool CountryDelete(int rowIndex)
    {

        GridViewRow row = gridCountry.Rows[rowIndex];
        string countryToDelete = row.Cells[0].Text;

        string text = "Good";
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

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Country Delete

    #region Country Edit

    private bool CountryEdit(int rowIndex)
    {
        //TODO

        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;
    }

    #endregion Country Edit

    #endregion Country

    #region Topic

    #region Topic Command

    /*
     *  Cannot delete a topic if there is any data that is associated with this given topic
     *  Then will update topic data
     */
    protected void Topic_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

    #region Topic Delete

    private bool TopicDelete(int rowIndex)
    {

        GridViewRow row = gridTopic.Rows[rowIndex];
        string topicToDelete = row.Cells[0].Text;

        string text = "Good";
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

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Topic Delete

    #region Topic Edit

    private bool TopicEdit(int rowIndex)
    {
        //TODO

        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;

    }

    #endregion Topic Edit

    #endregion Topic

    #region Country_Grade

    #region Country_Grade Command

    /*
     *  Cannot delete a country_grade if there is any data that is associated with this given country_grade
     *  Then will update country_grade data
     */
    protected void CountryGrade_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

    #region Country_Grade Delete

    private bool Country_GradeDelete(int rowIndex)
    {

        GridViewRow row = gridCountryGrade.Rows[rowIndex];
        string countryToDelete = row.Cells[0].Text;
        string gradeToDelete = row.Cells[1].Text;

        string text = "Good";
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

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        return canRemove;

    }

    #endregion Country_Grade Delete

    #endregion Country_Grade

    #region Country_Grade_Topic

    #region Country_Grade_Topic Command
    
    protected void CountryGradeTopic_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

    #region Country_Grade_Topic Delete

    private bool Country_Grade_TopicDelete(int rowIndex)
    {
        
        string text = "Good";
        bool good = true;
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        GridViewRow row = gridCountryGradeTopic.Rows[rowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;

        if(RemoveTopic(country, grade, topic))
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

                text += MySqlExceptionHandler(ex.Number);

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

    private bool RemoveTopic(string country, string grade, string topic)
    {

        List<String> lids = new List<string>();

        bool good = true;
        string text = "Good";
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT lid FROM lessons WHERE cid = (@cid) AND gid = (@gid) AND tid = (@tid);";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@cid", country);
            cmd.Parameters.AddWithValue("@gid", grade);
            cmd.Parameters.AddWithValue("@tid", topic);

            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while(rdr.Read())
                {
                    lids.Add((string)rdr[0]);
                }
            }//end rdr
            
        }//end try
        catch (MySqlException ex)
        {

            good = false;

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        foreach (string lid in lids)
        {
            /*if (!RemoveFile(country, grade, topic, lid))
            {
                return false;
            }*/
            if(!LessonDelete(country, grade, topic, lid))
            {
                return false;
            }
        }

        return good;
    }

    #endregion Country_Grade_Topic Delete

    #endregion Country_Grade_Topic

    #region Lesson

    #region Lesson Command

    /*
     *  Cannot delete a lesson
     */
    protected void Lesson_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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

    #region Lesson Delete

    private bool LessonDelete(int rowIndex)
    {

        GridViewRow row = gridLesson.Rows[rowIndex];
        string country = row.Cells[0].Text;
        string grade = row.Cells[1].Text;
        string topic = row.Cells[2].Text;
        string lid = row.Cells[3].Text;

        return LessonDelete(country, grade, topic, lid);
        
    }

    private bool LessonDelete(string country, string grade, string topic, string lid)
    {
        bool good = true;
        string text = "Good";
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        if (RemoveFile(country, grade, topic, lid))
        {

            try
            {
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
            catch (MySqlException ex)
            {

                good = false;

                text += MySqlExceptionHandler(ex.Number);

                text += " bad";

            }//end catch

            connection.Close();

            errormsgDB.Text = text;

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Lesson: file removal failed, contact admin');", true);
            good = false;
        }

        return good;
    }

    private bool RemoveFile(string country, string grade, string topic, string lid)
    {

        string path = "";
        int pathCount = 1;

        bool good = true;
        string text = "Good";
        string connectionString = GetConnectionString();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            String sql = "SELECT COUNT(cid) as count, path FROM lessons WHERE path = (" +
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

                    path = (string)rdr[1];
                }
                
            }//end rdr
                

        }//end try
        catch (MySqlException ex)
        {

            good = false;

            text += MySqlExceptionHandler(ex.Number);

            text += " bad";

        }//end catch

        connection.Close();

        errormsgDB.Text = text;

        if(!good)
        {
            return false;
        }


        if(pathCount == 1)
        {
            try
            {
                path = Server.MapPath(path);

                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        good = false;
                    }
                }
                else
                {
                    //TODO: file did not exist
                    ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('Lesson: file did not exist, contact admin');", true);
                }
            }
            catch(Exception e)
            {
                good = false;
            }
        }

        return good;
    }

    #endregion Lesson Delete

    #region Lesson Edit

    private bool LessonEdit(int rowIndex)
    {
        //TODO

        ClientScript.RegisterStartupScript(this.GetType(), "Invalid Request", "alert('NOT IMPLEMENTED');", true);

        return false;

    }

    #endregion Lesson Edit

    #endregion Lesson

    #endregion Table Events

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

    #region GetSession()

    private bool GetSession()
    {
        /*if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            //num = (int)Session["number"];
            if (matching)
                return true;
        }//end else*/
        return true;
    }//end method

    #endregion GetSession()

    protected void Btnaddlink_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAdd.aspx");
    }//end method


}//end class