﻿
using MSSQL_Lite.Access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DBContext:SqlContext
    {
        public DBContext()
            :base()
        {

        }

        public SqlAccess<User> Users { get { return new SqlAccess<User>(); } }
    }
}