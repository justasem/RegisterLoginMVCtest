using BasicRegisterLoginApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BasicRegisterLoginApp.DAL
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}