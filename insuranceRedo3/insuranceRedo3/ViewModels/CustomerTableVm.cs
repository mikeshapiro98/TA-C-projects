﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace insuranceRedo3.ViewModels
{
    public class CustomerTableVm
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Quote { get; set; }
    }
}