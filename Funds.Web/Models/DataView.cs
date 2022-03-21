using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funds.Data;

namespace Funds.Web.Models
{
    public class DataView
    { 
        public List<Contributor> Contributors { get; set; }
        public string Message { get; set; }
    }
}
