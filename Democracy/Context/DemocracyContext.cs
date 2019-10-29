using Democracy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Democracy.Context
{
    public class DemocracyContext : DbContext
    {
        public DemocracyContext()
            : base("DemocracyConnection")
        { }

        public DbSet<State> States { get; set; }
    }
}