using EmployeeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeApp.App_Start
{
    public class EmployeeSecurity
    {
        public static bool Login(string username,string password)
        {
            using(EmployeeDBEntities db = new EmployeeDBEntities())
            {
                return db.UserLogins.Any(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}