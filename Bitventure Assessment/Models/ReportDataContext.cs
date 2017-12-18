using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bitventure_Assessment.Models
{
    public class ReportDataContext : DbContext
    {
        public DbSet<ReportData> Report { get; set; }
    }
}