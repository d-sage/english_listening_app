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

    #region Page Load
    /*
     * 
     * TODO
     * 
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateAllData();
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
        string server = "mysql5018.site4now.net";
        string database = "db_a38d8d_lambe";
        string uid = "a38d8d_lambe";
        string password = "Lambejor000";
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

    #endregion Button Click Events

}//end class