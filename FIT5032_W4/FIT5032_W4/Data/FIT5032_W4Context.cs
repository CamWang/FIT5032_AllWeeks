using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FIT5032_W4.Models;

namespace FIT5032_W4.Data
{
    public class FIT5032_W4Context : DbContext
    {
        public FIT5032_W4Context (DbContextOptions<FIT5032_W4Context> options)
            : base(options)
        {
        }

        public DbSet<FIT5032_W4.Models.Student> Student { get; set; } = default!;

        public DbSet<FIT5032_W4.Models.Unit>? Unit { get; set; }
    }
}
