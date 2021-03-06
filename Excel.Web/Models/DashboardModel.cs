﻿using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class DashboardModel
    {
        private List<Session> history; 
        private List<Session> mySessions;

        public String Motd { get; set; }

        public int TotalSession { get; set; }

        public List<Session> MySessions
        {
            get { return mySessions; }
            set { mySessions = value; }
        }
        public List<Session> History
        {
            get { return history; }
            set { history = value; }
        }
    }
}