using insuranceRedo3.Models;
using insuranceRedo3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace insuranceRedo3.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (var db = new insuranceUhohEntities())
            {
                var customerTables = db.CustomerTables.ToList();
                var customerTableVms = new List<CustomerTableVm>();
                foreach (var customerTable in customerTables)
                {
                    var customerTableVm = new CustomerTableVm();
                    customerTableVm.Id = customerTable.Id;
                    customerTableVm.FirstName = customerTable.FirstName;
                    customerTableVm.LastName = customerTable.LastName;
                    customerTableVm.EmailAddress = customerTable.EmailAddress;
                    customerTableVm.Quote = customerTable.Quote.ToString();
                    customerTableVms.Add(customerTableVm);

                }
                return View(customerTableVms);
            }
        }
    }
}