using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    internal class MyDBContext : DbContext
    {
        public MyDBContext() : base("Server=DESKTOP-UQGGCKG;Database=abc;User Id=sa;Password=03zdsgfwa;Trusted_Connection=true;")
        {

        }
        public DbSet<ChatUser> Users { get; set; }
    }
}
