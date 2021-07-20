﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class History
    {
        public string filmId { get; set; }
        public string name { get; set; }
        public string thumbnail { get; set; }
        public string url { get; set; }
        public DateTime timestamp { get; set; }
    }
}