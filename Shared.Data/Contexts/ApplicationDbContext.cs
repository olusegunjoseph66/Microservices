using Microsoft.EntityFrameworkCore;
using Shared.Data.Models;

namespace Shared.Data.Contexts
{
    public class ApplicationDbContext : DmsportaldbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<DmsportaldbContext> options)
            : base(options)
        {
        }
    }
}
