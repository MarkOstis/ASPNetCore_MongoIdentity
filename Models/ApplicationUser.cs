﻿using AspNetCore.Identity.Mongo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCore_MongoIdentity.Models
{
    public class ApplicationUser : MongoUser
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }
    }
}
