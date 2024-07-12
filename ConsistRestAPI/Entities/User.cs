using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsistRestAPI.Entities
{
    public class User
    {
        
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }        
    }
}
