using ConsistRestAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsistRestAPI.Models.Users
{
    public class UserRequest
    {         
        [Required]        
        public string UserName { get; set; }

        [Required]        
        public string UserPassword { get; set; }        
    }
}
