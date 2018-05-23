using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PDFViewer : System.Web.UI.Page
{
    String link = "";
    #region Page Load
    /*
     * 
     * TODO
     * 
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        tblink.Enabled = false;
        btnlink.Enabled = false;
        UpdateAllData();
        hplink.NavigateUrl = link;
        Checkcred();
    }//end method

    #endregion Page Load

    #region GetConnectionString
    /*
     * 
     * Method that contains the variables to construct the connection
     * string required to access the database.
     * 
     */
    private string GetConnectionString()
    {
        string server = "mysql5012.site4now.net";
        string database = "db_a3acac_engdata";
        string uid = "db_a3acac_admin";
        string password = "Reaching4English";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        return connectionString;
    }//end method

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

    #region Update Lessons
    /*
     * 
     * TODO
     * 
     */
    private void UpdateLessonsPDFMP3(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            String sql = "SELECT cid,gid,tid,lid,filename,path FROM lessons  ORDER BY cid,gid,tid,lid ASC";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            DataTable dt = new DataTable();
            MySqlDataAdapter src = new MySqlDataAdapter(cmd);
            src.Fill(dt);
            gridLesson.DataSource = dt;
            gridLesson.DataBind();

        }
        catch (Exception e)
        {
            //TODO: email
            lblerror.Text += Environment.NewLine + "~Error: contact admin";
        }

        connection.Close();
    }



    #endregion Update Lessons

    #region Update Url

    public void UpdateUrl(MySqlConnection connection)
    {
        try
        {
            connection.Open();

            String sql = "SELECT * FROM survey_url";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        link = (String)rdr[0];
                    }//end while
                }//end 
            }//end
            hplink.NavigateUrl = link;
        }//end try
        catch (Exception e)
        {
            //TODO: email
            lblerror.Text += Environment.NewLine + "~Error: contact admin";
        }//end catch

        connection.Close();
    }//end method


    #endregion Update Url

    #region Update All Data
    /*
     * 
     * TODO
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
            //EmailError(text);
            lblerror.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }

        UpdateLessonsPDFMP3(connection);
        UpdateUrl(connection);
    }

    #endregion Update All Data

    #region Button Click Events
    /*
     * 
     * TODO
     * 
     */
    protected void btnpdf_Click(object sender, EventArgs e)
    {
        if ((Session["confirm"]) == null)
            Response.Redirect("Login.aspx");
        else
        {
            bool matching = (bool)Session["confirm"];
            if (matching)
                Response.Redirect("ManageAdd.aspx");
        }//end else
        Response.Redirect("Login.aspx");

    }//end method


    protected void Checkcred()
    {
        if ((Session["confirm"]) == null)
            return;
        else
        {
            bool matching = (bool)Session["confirm"];
            if (matching)
            {
                tblink.Enabled = true;
                btnlink.Enabled = true;
            }//end if
        }//end else
        return;
    }//end method


    protected void btnlink_Click(object sender, EventArgs e)
    {
        String linkdup = tblink.Text;
        tblink.Text = "";
        AddURL(linkdup);
    }//end method

    #endregion Button Click Events

    #region Add URL

    public void AddURL(String urllink)
    {
        MySqlConnection connection = null;
        try
        {
            connection = GetSqlConnection();

            connection.Open();

            String sql = "UPDATE survey_url SET url = (@url) WHERE url = @oldurl ";

            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {

                cmd.Parameters.AddWithValue("@url", urllink);
                cmd.Parameters.AddWithValue("@oldurl", link);

                cmd.ExecuteNonQuery();

            }//end cmd

            connection.Close();

            UpdateUrl(connection);

        }//end try
        catch (Exception e)
        {
            //TODO: email
            lblerror.Text += Environment.NewLine + "~Error: contact admin";
        }//end catch
    }

    #endregion Add URL

    

}//end class