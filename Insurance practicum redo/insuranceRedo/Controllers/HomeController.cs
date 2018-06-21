﻿using insuranceRedo.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace insuranceRedo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult CustomerInfo(string firstName, string lastName, string emailAddress, string dateOfBirth, int carYear,
                                            string carMake, string carModel, bool dui, string speedingTicket, bool fullCoverage)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(dateOfBirth.ToString()) ||
                string.IsNullOrEmpty(carYear.ToString()) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(speedingTicket.ToString()))
            {
                return View("Error");

            }
            else
            {
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    var customrInfo = new CustomerInfo();
                    customrInfo.FirstName = firstName;
                    customrInfo.LastName = lastName;
                    customrInfo.EmailAddress = emailAddress;
                    customrInfo.DateOfBirth = DateTime.Parse(dateOfBirth);
                    customrInfo.CarYear = carYear;
                    customrInfo.CarMake = carMake;
                    customrInfo.CarModel = carModel;
                    customrInfo.DUI = dui;
                    customrInfo.SpeedingTickets = Convert.ToInt32(speedingTicket);
                    customrInfo.FullCoverage = fullCoverage;

                    db.CustomerInfoes.Add(customrInfo);
                    db.SaveChanges();

                }
            return View("Quote");
            }

        }



    }
}