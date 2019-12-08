namespace WebApi.DataAcces
{
    using System.Data.Entity;
    using WebApi.Models;

    public class UsersContext : DbContext
    {

        public DbSet<User> Users { get; set; }
    }

}