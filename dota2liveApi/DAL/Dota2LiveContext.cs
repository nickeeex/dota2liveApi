using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace dota2liveApi.DAL
{
    public class Dota2LiveContext : DbContext
    {
        public Dota2LiveContext() : base("Dota2LiveContext")
        {
        }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
