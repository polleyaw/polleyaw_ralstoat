﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ralstoat_polleyaw_assignment06
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This is a test.

            Response.Write(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
            //Response.Write(DateTime.Now.ToShortTimeString());
        }
    }
}