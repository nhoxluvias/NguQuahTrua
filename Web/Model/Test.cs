﻿using MSSQL.Attributes;
using MSSQL.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Model
{
    [Table(Name = "ThuNghiem")]
    public class Test
    {
        [PrimaryKey(Name = "ID", DataType = null, Identity = "(1,1)")]
        public int ID { get; set; }

        [Column(Name = "name", DataType = null, AllowNull = false)]
        public string userName { get; set; }

        public string surName { get; set; }

        public string middleName { get; set; }
        public string name { get; set; }

        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string roleId { get; set; }
        [Column(Name = "dateTime", DataType = null, AllowNull = false)]
        public DateTime dateTime { get; set; }
    }
}