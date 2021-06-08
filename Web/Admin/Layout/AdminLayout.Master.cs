﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Layout
{
    public partial class AdminLayout : System.Web.UI.MasterPage
    {
        protected string hyplnkOverview;
        protected string hyplnkCategoryList;
        protected string hyplnkCountryList;
        protected string hyplnkLanguageList;
        protected string hyplnkRoleList;
        protected string hyplnkTagList;
        protected string hyplnkDirectorList;


        protected void Page_Load(object sender, EventArgs e)
        {
            hyplnkOverview = GetRouteUrl("Admin_Overview", null);
            hyplnkCategoryList = GetRouteUrl("Admin_CategoryList", null);
            hyplnkCountryList = GetRouteUrl("Admin_CountryList", null);
            hyplnkLanguageList = GetRouteUrl("Admin_LanguageList", null);
            hyplnkRoleList = GetRouteUrl("Admin_RoleList", null);
            hyplnkDirectorList = GetRouteUrl("Admin_DirectorList", null);
            hyplnkTagList = GetRouteUrl("Admin_TagList", null);
        }
    }
}