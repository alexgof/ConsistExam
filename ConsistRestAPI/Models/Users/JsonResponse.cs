using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ConsistRestAPI.Models.Users
{            

    [DataContract]
    public class JsonResponse
    {
        [DataMember(EmitDefaultValue = true, Order = 0)]
        public string Status { get; set; }
                        
        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string Message { get; set; }

        public void SetError(string errorMessage, string status = "Error")
        {
            Status = status;
            Message = errorMessage;
        }
    }
}
