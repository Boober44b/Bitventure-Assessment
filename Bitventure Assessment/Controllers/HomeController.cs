using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using Bitventure_Assessment.Models;

namespace Bitventure_Assessment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Import()
        {
            return PartialView("~/Views/Partials/FileUpload.cshtml");
        }

        [HttpPost]
        public ActionResult FileUpload()
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent == null || fileContent.ContentLength <= 0) continue;
                    // get a stream
                    var stream = fileContent.InputStream;
                    using (var reader = new StreamReader(stream))
                    {
                        var passedFirstline = false;
                        while (!reader.EndOfStream)
                        { 
                            var line = reader.ReadLine();
                            if (line == null) continue;
                            if (passedFirstline)
                            {
                                var values = line.Split(',');
                                using (var db = new ReportDataContext())
                                {
                                    var reportData = new ReportData
                                    {
                                        PaymentId = values[0],
                                        AccountHolder = values[1],
                                        BranchCode = values[2],
                                        AccountNumber = values[3],
                                        AccountType = values[4],
                                        TransactionDate = values[5],
                                        Amount = values[6],
                                        Status = values[7],
                                        EffectiveStatusDate = values[8]

                                    };

                                    db.Report.Add(reportData);
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                passedFirstline = true;
                            }

                            
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");

        }
        public ActionResult ViewImports()
        {

            using (var db = new ReportDataContext())
            {
                var viewModel = new ViewImportData();

                var reportdata = db.Report.ToList();
                var masterGrid = reportdata.GroupBy(x => x.AccountNumber).Select(y => y.First()).ToList();

                viewModel.MasterGrid = masterGrid;

                return PartialView("~/Views/Partials/ViewImports.cshtml", viewModel);
            }
            
        }
        public ActionResult ViewImportDetails(string accountnumber)
        {

            using (var db = new ReportDataContext())
            {

                var reportdata = db.Report.Where(x=>x.AccountNumber == accountnumber).ToList();
                

                return PartialView("~/Views/Partials/Details.cshtml", reportdata);
            }
            
        }
        public ActionResult Report()
        {

            using (var db = new ReportDataContext()) {

                var reportdata = db.Report.ToList();

                var groupedItems = reportdata.GroupBy(x => new {x.BranchCode, x.AccountType, x.Status}).ToList();

                var viewModel = groupedItems.Select(groupedItem => new ImportReportData
                    {
                        AccountType = groupedItem.Key.AccountType,
                        BranchCode = groupedItem.Key.BranchCode,
                        Status = groupedItem.Key.Status,
                        Count = groupedItem.Count(),
                        Amount = groupedItem.Sum(x => decimal.Parse(x.Amount, NumberStyles.Any, new CultureInfo("en-US"))).ToString(CultureInfo.InvariantCulture)
                    })
                    .OrderBy(x=>x.BranchCode).ToList();

                return PartialView("~/Views/Partials/Report.cshtml", viewModel);
            }
        }
    }
}