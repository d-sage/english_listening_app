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
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateAllData();
    }//end method


    #region GetConnectionString
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

    #region Lesson DataBound (obsolete)
    protected void Lesson_DataBound(object sender, GridViewRowEventArgs e)
    {

        //nothing

        /*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Attributes.Add("style", "width:600px;word-break:break-all;word-wrap:break-word;");
        }
        */
    }
    #endregion Lesson DataBound

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
            //EmailError(text);
            lblerror.Text += Environment.NewLine + "~Error: could not connect to database | contact admin";
            return;
        }

        UpdateLessonsPDFMP3(connection);

    }

    #endregion Update All Data

    protected void btnpdf_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }//end method

}//end class