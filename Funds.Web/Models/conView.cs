using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funds.Data;

namespace Funds.Web.Models
{
    public class conView
    {
        public List<Contributor> contributors { get; set; }
  
        public string Name { get; set; }
        public int id { get; set; }
    }
}
