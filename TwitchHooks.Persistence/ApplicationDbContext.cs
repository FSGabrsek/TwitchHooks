using System.Data.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitchHooks.Domain.Configuration;
using TwitchHooks.Domain.Entities;

namespace TwitchHooks.Infrastructure;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Webhook> Webhooks { get; set; }
    
    public DbSet<Client> Clients { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
