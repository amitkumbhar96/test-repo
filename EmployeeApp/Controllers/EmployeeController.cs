using EmployeeApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace EmployeeApp.Controllers
{
    //[EnableCorsAttribute{"*","*","*"}]
    public class EmployeeController : ApiController
    {
        private EmployeeDBEntities db = new EmployeeDBEntities();
        //private EmployeeDBNewEntities db = new EmployeeDBNewEntities();

        //Get: Get all employee records
        [HttpGet]
        [basicAuthenticationAttributes]
        public IEnumerable<Employee> GetAllEmployee()
        {
            //int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            
            List<Employee> employees = new List<Employee>();
            //if(UserId==0)
            //{
            //    em.Id = 0;
            //    em.FirstName = "";
            //    em.LastName = "";
            //    em.Gender = "";
            //    em.Salary = 0;
            //    employees.Add(em);
            //    return employees;
            //}
            string username = Thread.CurrentPrincipal.Identity.Name;
            var emp = db.Employees.ToList();
            switch(username.ToLower())
            {
                case "akumbhar@adaptivesourcing.com":
                     emp= db.Employees.Where(e => e.Gender == "Male").ToList();
                    break;
                case "Other":
                     emp = db.Employees.Where(e => e.Gender == "Female").ToList();
                    break;
            }
           
           // var emp = db.sp_InsUpdDelEmployees(0, "", "", "", 0, "All").ToList();
            //var emp=db.EmployeeInsertUpdateSelectDelete(0, "", "", "", 0, "Select").ToList();
            for (int ii = 0; ii < emp.Count(); ii++)
            {
                Employee em = new Employee();
                em.ID = emp[ii].ID;
                em.FirstName = emp[ii].FirstName;
                em.LastName = emp[ii].LastName;
                em.Gender = emp[ii].Gender;
                em.Salary = emp[ii].Salary;
                employees.Add(em);
            }
            return employees;
           // return db.Employees;
        }

        //Get: Get Perticular employee details
        [HttpGet]
        public IHttpActionResult GetEmployee(int id)
        {

            List<Employee> employees = new List<Employee>();
            var emp = db.sp_InsUpdDelEmployees(id, "", "", "", 0, "GetById").ToList();
            //var emp = db.EmployeeInsertUpdateSelectDelete(0, "", "", "", 0, "Select").ToList();
            for (int ii = 0; ii < emp.Count(); ii++)
            {
                if (emp[ii].ID == id)
                {
                    Employee em = new Employee();
                    em.ID = emp[ii].ID;
                    em.FirstName = emp[ii].FirstName;
                    em.LastName = emp[ii].LastName;
                    em.Gender = emp[ii].Gender;
                    em.Salary = emp[ii].Salary;
                    employees.Add(em);
                }
            }
            return Ok(employees);
            //var emp = db.Employees.FirstOrDefault(e => e.ID == id);
            //if (emp == null)
            //{
            //    return NotFound();
            //}
            //return Ok(emp);
        }  
        //POST: Add new Employee records
        [HttpPost]
        //[Route("InsertEmployee")]
        public IHttpActionResult AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee Details Null!!!");
            }
            
            //var emplist = db.sp_InsUpdDelEmployees(0, employee.FirstName.ToString(), employee.LastName.ToString(), employee.Gender.ToString(), employee.Salary.ToString(), "Ins").ToList();
            //db.Employees.Add(employee);
            //db.SaveChanges();
            try
            {
                var emp = db.sp_InsUpdDelEmployees(0, employee.FirstName, employee.LastName, employee.Gender, employee.Salary, "Ins");
                //var emp = db.EmployeeInsertUpdateSelectDelete(0, employee.FirstName.ToString(), employee.LastName.ToString(), employee.Gender.ToString(), employee.Salary, "Insert");

            }
            catch(Exception EX)
            {

            }
            return Ok("success");
        }

        //PUT: Update Employee details
        [HttpPut]
        public IHttpActionResult EditEmployeeDetails(int id, Employee employee)
        {
            if (id <= 0 && employee == null)
            {
                return BadRequest();
            }
            if (id != employee.ID)
            {
                return BadRequest();
            }
           
            try
            {
                //var emp = db.Employees.FirstOrDefault(e => e.ID == id);
                var emp = db.sp_InsUpdDelEmployees(id, employee.FirstName, employee.LastName, employee.Gender, employee.Salary, "Upd");
                //var emp = db.EmployeeInsertUpdateSelectDelete(id, employee.FirstName, employee.LastName, employee.Gender, employee.Salary, "Update").ToString();
            }
            catch(Exception ex)
            {

            }

            return Ok("success");
        }
        //DETELE: Delete particular employee details from daatabase.
        [HttpDelete]
        public IHttpActionResult DeleteEmployeeDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            try
            {
                // Employee emp = db.Employees.Where(e => e.Id == id).FirstOrDefault();
                var emp = db.sp_InsUpdDelEmployees(id, "", "", "", 0, "Del");
                //var emp = db.EmployeeInsertUpdateSelectDelete(id, "", "", "", 0, "Delete").ToString();
                //if (emp == null)
                //{
                //    return NotFound();
                //}
            }
            catch(Exception ex)
            {
                
            }
           
           // try
            //{
            //   // db.Employees.Remove(emp);
            //   // db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    return NotFound();
            //}
           
          
            return Ok("success");
        }

        // This is like finalise method
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool EmployeeExists(int id)
        //{
        //    return db.Employees.Count(e => e.Id == id) > 0;
        //}
    }
}
