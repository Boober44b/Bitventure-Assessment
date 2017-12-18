using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitventure_Assessment.Models
{
    public class ImportReportData
    {
        public string BranchCode { get; set; }
        public string Amount { get; set; }
        public string AccountType { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }
}