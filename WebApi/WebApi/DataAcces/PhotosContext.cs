namespace WebApi.DataAcces
{
    using System.Data.Entity;
    using WebApi.Models;

    public class PhotosContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
    }

}