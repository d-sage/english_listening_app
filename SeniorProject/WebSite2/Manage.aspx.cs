﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool run = GetSession();
        //makes sure they aren't going around the login
        if(run)
        {
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
        if(string.IsNullOrEmpty((string)Session["password"]) || string.IsNullOrEmpty((string)Session["username"]))
            return false;
        else
        {
            String password = (String)Session["password"];
            String username = (String)Session["username"];
            if ((username == "user") && password == ("pass"))
                return true;
        }//end else
        return false;
    }//end method

}//end class