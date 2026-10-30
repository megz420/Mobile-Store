
using MobileStore.Domain.Enums;

namespace MobileStore.Domain.Entities{
public class User
{
    public int Id { get; set; }  // more speed than guid in indexing and more simple

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;  // no constrains for choosing the hash method

    public string FirstName { get; set; } = string.Empty; 

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public UserRole Role { get; set; }   // entering number only

    public bool IsActive { get; set; } = true;   // for deactivating without deleting

    public DateTime CreatedAt { get; set; }  

// navigation property: 
    public ICollection<Order> Orders { get; set; } = [];  // bec in order table there is foreign key userid

    public ICollection<Review> Reviews { get; set; } = []; // user has multiple reviews
}
}