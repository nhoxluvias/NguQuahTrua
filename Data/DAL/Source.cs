﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class Source
    {
        public string filmId { get; set; }
        public string mainSource { get; set; }
        public string secondarySource1 { get; set; }
        public string secondarySource2 { get; set; }
        public string secondarySource3 { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}