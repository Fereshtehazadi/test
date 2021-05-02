using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HashMaker.Database
{
    // DbContext gör att kopplas till databasen 
    class HashingContext : DbContext
    {
        // addresen till databasen
        public static String connectionString = "Server=.\\SQLExpress;Database=Hash;Trusted_Connection=True;";
        
        // databas tabeller 
        public DbSet<PasswordHash> PasswordHashes { get; set; }
        public DbSet<ProcessingInfo> ProcessingInfo { get; set; }

        // OnConfiguring() använder UseSqlServer() för att ange addresen till databasen.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionString);
        }
    }
}
