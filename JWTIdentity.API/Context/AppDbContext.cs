using JWTIdentity.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTIdentity.API.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, int>(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
