using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Domain.Entities
{
    public partial class User : IdentityUser
    {
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
        public int Fee { get; set; }
        public List<UserToken> UserTokens { get; set; }
        
    }
}
