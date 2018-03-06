using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manage : System.Web.UI.Page
{
    private int num;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        //makes sure they aren't going around the login
        if(run)
        {
            ValueHiddenField.Value = num.ToString();
            //all code goes in here
        }//end if
        else
            Response.Redirect("Login.aspx");

    }//end main method

    protected void Submit_Click(object sender, EventArgs e)
    {

    }//end method

    private bool GetSession()
    {
        if((Session["confirm"]) == null)
            return false;
        else
        {
            bool matching = (bool)Session["confirm"];
            num = (int)Session["number"];
            if (matching)
                return true;
        }//end else
        return false;
    }//end method




}//end class