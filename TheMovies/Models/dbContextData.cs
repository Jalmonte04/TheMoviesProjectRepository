using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TheMovies.Models
{
    public class dbContextData : DbContext
    {
        public dbContextData(): base("dbContextTheMovies") 
        {

        }
       
        public DbSet<Peliculas> Peliculas { get; set; }
       

    }
}