using System;

namespace WebApi.Models
{
    public class Photo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
    }
}