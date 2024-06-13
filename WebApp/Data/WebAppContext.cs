using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public WebAppContext()
        {
        }   

        public DbSet<WebApp.Models.Language> Language { get; set; } = default!;
        public DbSet<WebApp.Models.Translation> Translation { get; set; } = default!;

    }
}
