using insuranceRedo3.Models;
using insuranceRedo3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace insuranceRedo3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestVm()
        {
            return View();
        }


      

        private insuranceUhohEntities db = new insuranceUhohEntities();

        //CUSTOMER INTAKE TO DB
        [HttpPost]
        public ActionResult CustomerTable(string firstName, string lastName, string emailAddress, string dateOfBirth, int carYear,
                                           string carMake, string carModel, bool dui, string speedingTicket, bool fullCoverage)
        {
            //ERROR CHECK FOR BLANKS
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(dateOfBirth.ToString()) ||
                string.IsNullOrEmpty(carYear.ToString()) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(speedingTicket.ToString()))
            {
                return View("~/Views/Shared/Error.cshtml");

            }
            else
            {
                //DB INSTANSIATION 
                using (insuranceUhohEntities db = new insuranceUhohEntities())
                {
                    var customerTable = new CustomerTable();
                    
                    customerTable.FirstName = firstName;
                    customerTable.LastName = lastName;
                    customerTable.EmailAddress = emailAddress;
                    customerTable.DateOfBirth = DateTime.Parse(dateOfBirth);
                    customerTable.CarYear = carYear;
                    customerTable.CarMake = carMake;
                    carMake = carMake.ToLower();
                    customerTable.CarModel = carModel;
                    carModel = carModel.ToLower();
                    customerTable.DUI = dui;
                    customerTable.SpeedingTickets = Convert.ToInt32(speedingTicket);
                    customerTable.FullCoverage = fullCoverage;
                  

                    //LOGIC OPPERATIONS
                    DateTime grab = DateTime.Parse(dateOfBirth);
                    int year = grab.Year;
                    int age = DateTime.Now.Year - year;

                    double quote;
                    quote = 50.00;

                    //AGE CHECK
                    if(age < 18)
                    {
                        quote = quote + 100.00;
                    }
                    else if(age < 25 || age > 100)
                    {
                        quote = quote + 25.00;
                    }


                    //CAR YEAR CHECK
                    if(carYear == 1999 || carYear > 2015)
                    {
                        quote = quote + 25.00;
                    }
                  
                    //PORCHE CHECK 
                    if(carMake == "porche")
                    {
                        quote = quote + 25.00;
                        if(carModel == "911 carrera")
                        {
                            quote = quote + 25.00;
                        }
                    }

                    //SPEEDING TICKET CHECK
                    int ticketCount = Convert.ToInt32(speedingTicket);
                    if (ticketCount < 0)
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    for(int i = 0; i < ticketCount; i++ )
                    {
                        quote = quote + 10.00; 
                    }

                    //DUI CHECK
                    if (dui == true)
                    {
                        quote = quote + (quote * .25);
                    }

                    //FULL COVERAGE CHECK 
                    if(fullCoverage == true)
                    {
                        quote = quote + (quote * .50);
                    }

                    customerTable.Quote = quote; 

                    db.CustomerTables.Add(customerTable);
                    db.SaveChanges();

                    int userId = customerTable.Id;

                    //   return View(Quoter(userId));

                    return RedirectToAction("Quote", customerTable.Id);
                }
                
            }
        }


        //public ActionResult Quoter(int Id)
        //{
        //    if (Id == null)
        //    {
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    CustomerTable customer = db.CustomerTables.Find(Id);
        //    if (customer == null)
        //    {
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return View(customer);
        //}


        public ActionResult Quote(int, Id)
        {
            quoteVm returner = new quoteVm();
            returner.Id = Id; 
            return View();
        }



        //public ActionResult Quote(int? id)
        //{

        //    CustomerTable customer = db.CustomerTables.Find(id);

        //    return View(customer);
        //}


        //[HttpPost]
        //public ActionResult Quote(int Id)
        //{
        //    using (var db = new insuranceUhohEntities())
        //    {
        //        var finder = db.CustomerTables.Find(Id);
        //        string quote = finder.Quote.ToString();
        //        return View(quote);

        //    }

        //}


    }
}