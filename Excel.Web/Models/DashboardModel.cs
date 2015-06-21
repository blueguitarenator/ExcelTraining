using Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Excel.Web.Models
{
    public class DashboardModel
    {
        private List<Session> mySessions;

        public List<Session> MySessions
        {
            get { return mySessions; }
            set { mySessions = value; }
        }
    }
}