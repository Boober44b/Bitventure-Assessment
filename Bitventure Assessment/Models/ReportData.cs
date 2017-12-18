using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bitventure_Assessment.Models
{
    public class ReportData
    {
        public int Id { get; set; }
        public string PaymentId { get; set; }
        public string AccountHolder { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string TransactionDate { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string EffectiveStatusDate { get; set; }

    }
}