using Funds.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Funds.Data;
namespace Funds.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=funds;Integrated Security=true;";
        public IActionResult Index()
        {
            Database db = new Database(_connectionString);
            var simchos = db.GetAllSim();
            int Total = db.TotalCon();
            foreach(var simcha in simchos)
            {
                var givers=db.ThoseWhoGave(simcha.id);
                simcha.giver = givers;
                int totalGiven = db.GetSimTotal(simcha.id);
                simcha.TotalGiven = totalGiven;
            }
          
            simchaView sv = new simchaView
            {
                Total=Total,
                Simchos = simchos,
            };

            return View(sv);
        }
        public IActionResult Contributors()
        {
            Database db = new Database(_connectionString);
            var contributors = db.GetAllCon();
            foreach(var con in contributors)
            {
                var sumcdep=db.GetSumDep(con.id);
                var sumcon = db.GetSumCon(con.id);
                var balance = sumcdep - sumcon;
                con.balance = balance;        
            }
           
            DataView dv = new DataView
            {
                Contributors = contributors
            };
            string message = (string)TempData["Message"];
            if (!String.IsNullOrEmpty(message))
            {
                dv.Message = message;
            };
            
            return View(dv);
        }
        public IActionResult history(int contribid)
        {
            Database db = new Database(_connectionString);
            var deps = db.GetDep(contribid);
            var sims = db.GetSimByid(contribid);
            foreach(var his in sims)
            {
                deps.Add(his);
            }
            //deps.OrderBy(e => e.Date);
            deps.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            var con = db.GetConById(contribid);
            var sumcdep = db.GetSumDep(con.id);
            var sumcon = db.GetSumCon(con.id);
            var balance = sumcdep - sumcon;
            con.balance = balance;
            historyView hv = new historyView
            {
                contributor=con,
               history=deps
            };
            return View(hv);
        }
        [HttpPost]
        public IActionResult deposit(Deposit dep)
        {
            Database db = new Database(_connectionString);
            db.AddDeposit(dep);
            TempData["Message"] = $"Deposited";
            return Redirect("/Home/Contributors");
        }

        [HttpPost]
        public IActionResult newcon(Contributor contributor)
        {
            Database db = new Database(_connectionString);
            var id=db.Add(contributor);
            Deposit dp = new Deposit
            {
                personid = id,
                Amount = contributor.Deposit,
                Date=contributor.Date

            };
            TempData["Message"] = $"New person with an id of {id} was added!";
            db.AddDeposit(dp);
            return Redirect("/Home/Contributors");
        }
        [HttpPost]
        public IActionResult newSimcha(Simcha simcha)
        {
            Database db = new Database(_connectionString);
            db.AddSimcha(simcha);
            return Redirect("/home/index");
        }
        [HttpPost]
        public IActionResult editcon(Contributor contributor)
        {
            Database db = new Database(_connectionString);
            db.Edit(contributor);
            TempData["Message"] = $"Person was edited";
            return Redirect("/Home/Contributors");
        }

        public IActionResult contributions(int simchaid)
        {
            Database db = new Database(_connectionString);
            var people = db.GetAllCon();
            int x = 0;
            foreach (var con in people)
            {
                var sumcdep = db.GetSumDep(con.id);
                var sumcon = db.GetSumCon(con.id);
                var balance = sumcdep - sumcon;
                con.balance = balance;
                con.x = x;
                x++;
                int gave= db.GetIfGaveToSim(simchaid,con.id);
                {
                    if(gave==0)
                    {
                        con.Include = false;
                    }
                    else
                    {
                        con.Include = true;
                    }
                }
            }
            string name = db.GetSim(simchaid);
            conView cv = new conView
            {
                contributors = people,
                Name = name,
                id = simchaid
            };
            return View(cv);
        }
        [HttpPost]
        public IActionResult updatecontributions(List<Contributor> contributors,int simchaId)
        {
            Database db = new Database(_connectionString);
            db.DeleteCon(simchaId);
            foreach(var con in contributors)
            {
                con.SimchaId = simchaId;
                if(con.Include)
                {
                    db.AddContribution(con);
                    
                }
              
               
            }
            return Redirect("/Home/Index");
        }
    }
}
