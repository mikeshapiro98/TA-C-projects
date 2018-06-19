using insuranceRedo.Models;
using insuranceRedo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace insuranceRedo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {


            using (var db = new InsuranceEntities())
            {
                var customerInfoes = db.CustomerInfoes.ToList();
                var customerInfoVms = new List<CustomerInfoVm>();
                foreach (var customerinfo in customerInfoes)
                {
                    var customerInfoVm = new CustomerInfoVm();
                    customerInfoVm.Id = customerinfo.Id;
                    customerInfoVm.FirstName = customerinfo.FirstName;
                    customerInfoVm.LastName = customerinfo.LastName;
                    customerInfoVm.EmailAddress = customerinfo.EmailAddress;
                    customerInfoVms.Add(customerInfoVm);

                }
                return View(customerInfoVms);

            }

        }
    }
}