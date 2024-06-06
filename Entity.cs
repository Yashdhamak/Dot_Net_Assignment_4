using Microsoft.AspNetCore.Identity;

namespace VisitorSecurityClearanceSystem 
{
    public class VisitorEntity
    {
        public  string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        // Add more properties as needed
    }

    public class SecurityEntity : IdentityUser
    {
        // Additional properties for security personnel
        public object Id {  get; set; }
        public object Name { get; internal set; }

        public Object Email {  get; internal set; }
        public object Role { get; internal set; }

    }

    public class ManagerEntity : IdentityUser
    {
        // Additional properties for managers
        public object Name { get; internal set; }
        public object Department { get; internal set; }
    }

    public class OfficeEntity
    {
        
        public string Name { get; set; }
        public string Location { get; set; }
        // Add more properties as needed
    }

    public class PassEntity
    {
        public string Id { get; set; }
        public string VisitorId { get; set; }
        public string ManagerId { get; set; }
        public string SecurityId { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsAccepted { get; set; }
        // Add more properties as needed
    }

    // Add more entities as required
}
