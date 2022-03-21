using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funds.Data;

namespace Funds.Web.Models
{
    public class historyView
    {
        public List<History> history { get; set; }
        public Contributor contributor { get; set; }
    }
}
