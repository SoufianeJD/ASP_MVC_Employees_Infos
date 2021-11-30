using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web;
using System.Web.Mvc;
using T4EmployeesMVC.Models;

namespace T4EmployeesMVC.Controllers
{
    public class HomeController : Controller
    {

        OdbcConnection con = new OdbcConnection("dsn=Aptrain;uid=sysprogress;pwd=test");
        List<employee> employees = new List<employee>();
        List<pdfclass> pdffiles = new List<pdfclass>();

        public ActionResult Index()
        {         
            return View();
        }
        public ActionResult IndexID()
        {    
            try
            {
                if (Session["id"].ToString() != null)
                    ViewBag.user = Session["id"].ToString().Trim();
                ViewBag.image = "../Profileimg/" + Session["id"].ToString().Trim() + ".jpg";
            }
            catch (Exception e)
            {
                ViewBag.user = "Guest";
                ViewBag.image = "https://cdn1.iconfinder.com/data/icons/flat-character-color-1/60/flat-design-character_6-512.png";
            }

            return View();
        }

        public ActionResult SignIn()
        {
            try
            {
                if (Session["id"].ToString() != null)
                    return RedirectToAction("id");
            }
            catch (Exception ex)
            {
            }

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(user e)
        {
            con.Open();
            OdbcCommand DbCommand = con.CreateCommand();
            DbCommand.CommandText = "SELECT count(*) FROM pub.webUsers WHERE UserName = '" + e.UserName + "' AND Passwrd = '" + e.Passwrd + "'";
            string output = DbCommand.ExecuteScalar().ToString();
            if (output == "1")
            {
                Session["id"] = e.UserName;
                con.Close();
                return RedirectToAction("IndexID");
            }
            else
            {
                ViewBag.wronglogin = "You're informations are wrong, plz try again";
                //con.Close();
                return RedirectToAction("SignIn");
            }
       
        }

        public ActionResult Logout()
        {
            Session.Remove("id");
            Session.RemoveAll();
            return View("SignIn");
        }

        public ActionResult Paytab()
        {
            ViewBag.user = Session["id"].ToString().Trim();
            string query = "SELECT * FROM PUB.\"chq-hdr\" AS E , PUB.webUsers AS W where E.\"Empl-no\" = W.EmplNo AND W.UserName = '" + Session["id"] + "'";
            con.Open();
            OdbcCommand sqlcnxx = new OdbcCommand(query, con);
            sqlcnxx.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sdr = sqlcnxx.ExecuteReader();

                if (sdr.Read())
            {
                string en = sdr["Empl-no"].ToString();
                ViewData["Empl-no"] = en;
                string c = sdr["Company"].ToString();
                ViewData["Company"] = c;
                string ena = sdr["Dept"].ToString();
                ViewData["Dept"] = ena;
                string fn = sdr["Week-no"].ToString();
                ViewData["Week-no"] = fn;
                string ln = sdr["Chq-dte"].ToString();
                ViewData["Chq-dte"] = ln;
                string ad2 = sdr["Gross"].ToString();
                ViewData["Gross"] = ad2;
                string tr = sdr["Net"].ToString();
                ViewData["Net"] = tr;
                string fre = sdr["Freq"].ToString();
                ViewData["Freq"] = fre;
                string tp = sdr["Times-print"].ToString();
                ViewData["Times-print"] = tp;
                string pip = sdr["PPIP-$"].ToString();
                ViewData["PPIP-$"] = pip;
                string type = sdr["Type"].ToString();
                ViewData["Type"] = type;
                string wk = sdr["Wk-apply"].ToString();
                ViewData["Wk-apply"] = wk;
                string inn = sdr["Insbl-$"].ToString();
                ViewData["Insbl-$"] = inn;
                string inp = sdr["Pnsbl-$"].ToString();
                ViewData["Pnsbl-$"] = inp;
                string prn = sdr["Prntbl"].ToString();
                ViewData["Prntbl"] = prn;
            }

            con.Close();
            return View();
        }

        public ActionResult Paychequelist()
        {
            ViewBag.user = Session["id"].ToString().Trim();
            string query = "SELECT TOP 20 * FROM PUB.\"chq-hdr\" AS E , PUB.webUsers AS W where E.\"Empl-no\" = W.EmplNo AND W.UserName = '" + Session["id"] + "'";
            con.Open();
            OdbcCommand sqlcn = new OdbcCommand(query, con);
            sqlcn.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sd = sqlcn.ExecuteReader();
        
                while (sd.Read())
            {
                    employees.Add(new employee()
                    {
                        nchq = sd["Chq-no"].ToString()
                    ,
                         
                        emplno = sd["Empl-no"].ToString()
                    ,
                        semaine = sd["Week-no"].ToString()
                    ,
                        dep = sd["Dept"].ToString()
                    ,
                        net = sd["Net"].ToString()
                    ,
                        pnsb = sd["Pnsbl-$"].ToString()

                    });
                }
        


            con.Close();

            return View(employees);
        }
        public ActionResult Cheque(string param_nchq, string param_emplno)
        {
            employee employee = new employee();

            ViewBag.user = Session["id"].ToString().Trim();
            string query = "SELECT \"Vacn-earn-h\",\"Vacn-earned\",\"Pnsbl-$\",\"Txben-prov\",\"Txben-fed\",\"Chq-no\",\"Empl-no\",\"Chq-dte\",Gross,Net FROM PUB.\"chq-hdr\" where \"Chq-no\" = '" + param_nchq + "'";
            string queryy = "SELECT Hrs,\"holi-rate\",\"Addr-1\",\"Addr-2\",\"Postal-code\",\"Empl-name\",\"E-mail-addr\",\"UIC-code\" FROM PUB.employee where \"Empl-no\" = '" + param_emplno + "'";
            string quer = "SELECT PUB.uic.\"E-mail-addr\" FROM PUB.uic AS E, PUB.employee AS W, PUB.webUsers AS X where E.\"UIC-code\" = W.\"UIC-code\" AND W.\"Empl-no\" ='" + param_emplno + "'";           
            con.Open();
            OdbcCommand sqlcnx = new OdbcCommand(query, con);
            OdbcCommand sqlcn = new OdbcCommand(queryy, con);
            OdbcCommand sql = new OdbcCommand(quer, con);
            sqlcnx.Parameters.AddWithValue("UserName", Session["id"].ToString());
            sqlcn.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sdr = sqlcnx.ExecuteReader();
            OdbcDataReader sd = sqlcn.ExecuteReader();
            OdbcDataReader s = sql.ExecuteReader();

            if (s.Read() && sd.Read() && sdr.Read() &&  sdr.GetValue(0) != DBNull.Value && s.GetValue(0) != DBNull.Value && sd.GetValue(0) != DBNull.Value)
            {
                employee.nchq = param_nchq;
                employee.emplno = param_emplno;
                employee.nchq = sdr["Chq-no"].ToString();
                ViewData["Chq-no"] = employee.nchq;
                employee.emplno = sdr["Empl-no"].ToString();
                ViewData["Empl-no"] = employee.emplno;
                employee.chqdte = sdr["Chq-dte"].ToString();
                ViewData["Chq-dte"] = employee.chqdte;
                employee.net = sdr["Net"].ToString();
                ViewData["Net"] = employee.net;
                employee.gross = Convert.ToInt32(sdr["Gross"]);
                var varr=employee.gross/2;
                ViewData["Gross"] = varr;
                employee.addr1 = sd["Addr-1"].ToString();
                ViewData["Addr-1"] = employee.addr1;
                employee.addr2 = sd["Addr-2"].ToString();
                ViewData["Addr-2"] = employee.addr2;
                employee.postalcode = sd["Postal-code"].ToString();
                ViewData["Postal-code"] = employee.postalcode;
                employee.emplname = sd["Empl-name"].ToString();
                ViewData["Empl-name"] = employee.emplname;
                employee.mailaddr = sd["E-mail-addr"].ToString();
                ViewData["E-mail-addr"] = employee.mailaddr;
                employee.mailaddrr = s["E-mail-addr"].ToString();
                ViewData["E-mail-addr2"] = employee.mailaddrr;
                employee.uiccode = sd["UIC-code"].ToString();
                ViewData["UIC-code"] = employee.uiccode;
                employee.vacnearnh = sdr["Vacn-earn-h"].ToString();
                ViewData["Vacn-earn-h"] = employee.vacnearnh;
                employee.vacnearned = sdr["Vacn-earned"].ToString();
                ViewData["Vacn-earned"] = employee.vacnearned;
                employee.pnsbl = sdr["Pnsbl-$"].ToString();
                ViewData["Pnsbl-$"] = employee.pnsbl;
                employee.txbenprov = sdr["Txben-prov"].ToString();
                ViewData["Txben-prov"] = employee.txbenprov;
                employee.txbenfed = sdr["Txben-fed"].ToString();
                ViewData["Txben-fed"] = employee.txbenfed;
                employee.holirate = sd["holi-rate"].ToString();
                ViewData["holi-rate"] = employee.holirate;
                employee.hrs = sd["Hrs"].ToString();
                ViewData["Hrxxxs"] = employee.hrs;

                ViewBag.Message = employee;

                con.Close();
                return View(employee);
            }
            else
            {
                con.Close();
                return RedirectToAction("IndexID");
            }
        } 
        public ActionResult profil()
        {
            string query = " SELECT * FROM PUB.employee AS E , PUB.webUsers AS W where E.\"Empl-no\" = W.EmplNo AND W.UserName = '" + Session["id"] + "'";
            con.Open();
            OdbcCommand sqlcnx = new OdbcCommand(query, con);
            sqlcnx.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sdr = sqlcnx.ExecuteReader();
            if (sdr.Read())
            {
                string en = sdr["Empl-no"].ToString();
                ViewData["Empl-no"] = en;
                string c = sdr["Company"].ToString();
                ViewData["Company"] = c;
                string ena = sdr["Empl-name"].ToString();
                ViewData["Empl-name"] = ena;
                string ad = sdr["Addr-1"].ToString();
                ViewData["Addr-1"] = ad;
                string fn = sdr["First-name"].ToString();
                ViewData["First-name"] = fn;
                string ln = sdr["Last-name"].ToString();
                ViewData["Last-name"] = ln;
                string ad2 = sdr["Addr-2"].ToString();
                ViewData["Addr-2"] = ad2;
                string pc = sdr["Postal-code"].ToString();
                ViewData["Postal-code"] = pc;
                string tr = sdr["Terminated"].ToString();
                ViewData["Terminated"] = tr;
                string lvl = sdr["Level"].ToString();
                ViewData["Level"] = lvl;
                string bs = sdr["business"].ToString();
                ViewData["business"] = bs;
                string ac = sdr["Active"].ToString();
                ViewData["Active"] = ac;
                string mail = sdr["EmailAddress"].ToString();
                ViewData["EmailAddress"] = mail;
                string pw = sdr["Passwrd"].ToString();
                ViewData["Passwrd"] = pw;
                string sa = sdr["Salary"].ToString();
                ViewData["Salary"] = sa;

            }
            con.Close();
            return View();
     }

        [HttpPost]
        public ActionResult Updatepw(user e)
        {
            OdbcCommand cmd = new OdbcCommand("UPDATE pub.webUsers SET Passwrd = '" + e.Passwrd + "'WHERE UserName = '" + Session["id"] + "'", con);       
            cmd.CommandType = CommandType.Text;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("profil");
        }

        public ActionResult Releve1(string param_emplno, string param_username, string param_year)
        {
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1500px\" height=\"1300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute("~/RL1Rep/" + param_username + "_" + param_emplno + "_"+  param_year +".pdf"));
           // TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute("~/RL1Rep/" + param_username + "_" + param_emplno + "_2020.pdf"));
           // TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute("~/RL1Rep/admin_000001_2019"));
            return RedirectToAction("Releve1List");
        }
        public ActionResult Releve1List()
        {
            ViewBag.user = Session["id"].ToString().Trim();
            string query = "SELECT TOP 10 * FROM PUB.\"Document-archiving\" AS E , PUB.webUsers AS W where E.Emplno = W.EmplNo AND W.UserName = '" + Session["id"] + "'";
            con.Open();
            OdbcCommand sqlcn = new OdbcCommand(query, con);
            sqlcn.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sd = sqlcn.ExecuteReader();

            while (sd.Read())
            {
                employees.Add(new employee()
                {
                    emplno = sd["Emplno"].ToString(),
                    username = sd["UserName"].ToString(),
                    years = sd["years"].ToString(),
                });
            }
            con.Close();
            return View(employees);
        }

        public ActionResult T4List()
        {
            ViewBag.user = Session["id"].ToString().Trim();
            string query = "SELECT TOP 10 * FROM PUB.\"Document-archiving\" AS E , PUB.webUsers AS W where E.Emplno = W.EmplNo AND W.UserName = '" + Session["id"] + "'";
            con.Open();
            OdbcCommand sqlcn = new OdbcCommand(query, con);
            sqlcn.Parameters.AddWithValue("UserName", Session["id"].ToString());
            OdbcDataReader sd = sqlcn.ExecuteReader();

            while (sd.Read())
            {
                employees.Add(new employee()
                {
                    emplno = sd["Emplno"].ToString(),
                    username = sd["UserName"].ToString(),
                    years = sd["years"].ToString(),
                });
            }
            con.Close();
            return View(employees);
        }
        public ActionResult T4(string param_emplno, string param_username, string param_year)
        {
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1500px\" height=\"1300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute("~/T4Rep/" + param_username + "_" + param_emplno + "_" + param_year + ".pdf"));
            return RedirectToAction("T4List");
        }
    }
}