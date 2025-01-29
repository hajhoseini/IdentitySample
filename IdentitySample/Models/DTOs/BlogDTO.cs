using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentitySample.Models.DTOs
{
    public class BlogDTO
    {
        [BindNever]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        [BindNever]
        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
