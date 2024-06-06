using DotLiquid.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.OLE.Interop;
using SendGrid;
using SendGrid.Helpers.Mail;
using VisitorSecurityClearanceSystem.Models.VisitorSecurityClearanceSystem.Models;

namespace VisitorSecurityClearanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private object _dbcontext;
        private object updatedVisitor;
        private object updatedSecurity;
        private object office;
        private object _emailService;
        private object updatedPass;
        private object updatedOffice;
        private readonly YourDbContext _context;



        public string URI = "https://localhost:8081";
        public string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public string DatabaseName = "librarydata";
        public string ContainerName = "student";

        public MainController(YourDbContext context)
        {
            _context = context;
            this._context = context;
        }

        public object EntityState { get; private set; }
        public SendGridMessage MSG { get; private set; }

        public MainController(
           UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           IConfiguration configuration,
           YourDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        // Authentication endpoints
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Registration successful");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok("Login successful");
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }

        // Visitor CRUD endpoints
        [Authorize]
        [HttpPost("visitors")]
        public async Task<IActionResult> CreateVisitor([FromBody] VisitorDTO visitor)
        {
            try
            {
                // Convert VisitorDTO to VisitorEntity
                var visitorEntity = new VisitorEntity
                {   
                    // Id  = visitor.id;
                    Name = visitor.Name,
                    Email = visitor.Email,
                    Address = visitor.Address,
                    CompanyName = visitor.CompanyName,
                    Purpose = visitor.Purpose,
                    EntryTime = visitor.EntryTime,
                    ExitTime = visitor.ExitTime
                    // Add more properties as needed
                };

                // Save the visitorEntity to your database
                // Example: dbContext.Visitors.Add(visitorEntity);
                // Example: await dbContext.SaveChangesAsync();

                // Return a success message
                return Ok("Visitor created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while creating a visitor");

                // Return an error message
                return StatusCode(500, "An error occurred while creating the visitor");
            }
            
            
        }

        [Authorize]
        [HttpGet("visitors/{id}")]
        public async Task<IActionResult> GetVisitorById(string id)
        {
            // Your logic to retrieve visitor by id

            var visitor = await _context.visitors.FindAsync(id);

            if (visitor == null)
            {
                return NotFound("Visitor not found");
            }

            return Ok(visitor);
        }



        [Authorize]
        [HttpPut("visitors/{id}")]
        public async Task<IActionResult> UpdateVisitor(string id, [FromBody] VisitorDTO visitor)
        {
            // Your logic to update a visitor
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
            {
                return NotFound("Visitor not found");
            }

            // Update visitor properties with values from updatedVisitor DTO
            visitor.Name = updatedVisitor.Name;
            visitor.Email = updatedVisitor.Email;
            visitor.Address = updatedVisitor.Address;
            visitor.CompanyName = updatedVisitor.CompanyName;
            visitor.Purpose = updatedVisitor.Purpose;
            visitor.EntryTime = updatedVisitor.EntryTime;
            visitor.ExitTime = updatedVisitor.ExitTime;

            // Mark the visitor entity as modified
            _context.Entry(visitor).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok("Visitor updated successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception
                return StatusCode(500, "Concurrency exception occurred while updating visitor");
            }
            return Ok("Visitor updated successfully");
        }

        [Authorize]
        [HttpDelete("visitors/{id}")]
        public async Task<IActionResult> DeleteVisitor(string id)
        {
            // Your logic to delete a visitor
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
            {
                return NotFound("Visitor not found");
            }

            _context.Visitors.Remove(visitor);
            object id = await _context.SaveChangesAsync();
            return Ok("Visitor deleted successfully");
        }

        // Security CRUD endpoints
        [Authorize(Roles = "Manager")]
        [HttpPost("security")]
        public async Task<IActionResult> CreateSecurity([FromBody] SecurityDTO security)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new SecurityEntity instance and map values from the DTO
                var securityEntity = new SecurityEntity
                {
                    // Assign properties from the DTO
                    // Id = security.Id;
                    Name = security.Name,
                    Email = security.Email,
                    Role = security.Role,
                    Id = security.id

                    // Assign other properties as needed
                };

                // Add security personnel to the database
                _context.Security.Add(securityEntity);
                await _context.SaveChangesAsync();

                return Ok("Security personnel created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while creating a security personnel");

                return StatusCode(500, "An error occurred while creating the security personnel");
            }
            // Your logic to create a new security personnel
            return Ok("Security personnel created successfully");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("security/{id}")]
        public async Task<IActionResult> GetSecurityById(string id)
        {
            var security = await _context.Security.FindAsync(id);

            if (security == null)
            {
                return NotFound("Security personnel not found");
            }

            return Ok(security);

            //  logic to retrieve security personnel by id
           
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("security/{id}")]
        public async Task<IActionResult> UpdateSecurity(string id, [FromBody] SecurityDTO security)
        {
            //  logic to update a security personnel
            var security = await _context.Security.FindAsync(id);

            if (security == null)
            {
                return NotFound("Security personnel not found");
            }

            // Update security personnel properties with values from updatedSecurity DTO
            security.Name = updatedSecurity.Name;
            security.Email = updatedSecurity.Email;
             security.Role = updatedSecurity.Role;
            // Update other properties as needed

            // Save changes to the database
            await _context.SaveChangesAsync();
            return Ok("Security personnel updated successfully");
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("security/{id}")]
        public async Task<IActionResult> DeleteSecurity(string id)
        {
            var security = await _context.Security.FindAsync(id);

            if (security == null)
            {
                return NotFound("Security personnel not found");
            }

            _context.Security.Remove(security);
            await _context.SaveChangesAsync();
            //  logic to delete a security personnel
            return Ok("Security personnel deleted successfully");
        }

        // Manager CRUD endpoints
        [Authorize(Roles = "Manager")]
        [HttpPost("manager")]
        public async Task<IActionResult> CreateManager([FromBody] ManagerDTO manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new ManagerEntity instance and map values from the DTO
                var managerEntity = new ManagerEntity
                {
                    // Assign properties from the DTO
                    Name = manager.Name,
                    Email = manager.Email,
                    Department = manager.Department,
                    // Assign other properties as needed
                };

                // Add manager to the database
                _context.Managers.Add(managerEntity);
                await _context.SaveChangesAsync();

                return Ok("Manager created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while creating a manager");

                return StatusCode(500, "An error occurred while creating the manager");
            }
            //  logic to create a new manager
            
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("manager/{id}")]
        public async Task<IActionResult> GetManagerById(string id)
        {
            // Your logic to retrieve manager by id
            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound("Manager not found");
            }

            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("manager/{id}")]
        public async Task<IActionResult> UpdateManager(string id, [FromBody] ManagerDTO manager)
        {
            // Your logic to update a manager
            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound("Manager not found");
            }

            // Update manager properties with values from updatedManager DTO
            // Example: manager.Name = updatedManager.Name;
            // Example: manager.Email = updatedManager.Email;
            // Example: manager.Department = updatedManager.Department;
            // Update other properties as needed

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok("Manager updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while updating a manager");

                return StatusCode(500, "An error occurred while updating the manager");
            }
            return Ok("Manager updated successfully");
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("manager/{id}")]
        public async Task<IActionResult> DeleteManager(string id)
        {
            // Your logic to delete a manager
            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound("Manager not found");
            }

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();

            return Ok("Manager deleted successfully");
        }

        // Office CRUD endpoints
        [Authorize(Roles = "Manager")]
        [HttpPost("office")]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeDTO office)
        {
            // Your logic to create a new office

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new OfficeEntity instance and map values from the DTO
                var officeEntity = new OfficeEntity
                {
                    // Assign properties from the DTO
                    Name = (string)office.Name,
                    Location = office.Location,
                    // Assign other properties as needed
                };

                // Add office to the database
                _context.Offices.Add(officeEntity);
                await _context.SaveChangesAsync();

                return Ok("Office created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while creating an office");

                return StatusCode(500, "An error occurred while creating the office");
            }
            
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("office/{id}")]
        public async Task<IActionResult> GetOfficeById(string id)
        {
            // Your logic to retrieve office by id
            var office = await _context.Offices.FindAsync(id);

            if (office == null)
            {
                return NotFound("Office not found");
            }
            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("office/{id}")]
        public async Task<IActionResult> UpdateOffice(string id, [FromBody] OfficeDTO office)
        {
            // Your logic to update an office
            var office = await _context.Offices.FindAsync(id);

            if (office == null)
            {
                return NotFound("Office not found");
            }

            // Update office properties with values from updatedOffice DTO
             office.Name = updatedOffice.Name;
            office.Location = updatedOffice.Location;
            // Update other properties as needed

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok("Office updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while updating an office");

                return StatusCode(500, "An error occurred while updating the office");
            }
           
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("office/{id}")]
        public async Task<IActionResult> DeleteOffice(string id)
        {
            // Your logic to delete an office
            var office = await _context.Offices.FindAsync(id);

            if (office == null)
            {
                return NotFound("Office not found");
            }

            _context.Offices.Remove(office);
            await _context.SaveChangesAsync();

            return Ok("Office deleted successfully");
        }

        // Pass creation endpoint
        [Authorize]
        [HttpPost("createpass")]
        public async Task<IActionResult> CreatePass([FromBody] PassDTO pass)
        {
            //  logic to create a pass for a visitor

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Logic to create a pass for a visitor
                // Example: Generate PDF pass with visitor details

                // Send pass as PDF via email
                await _emailService.SendPassByEmail(pass);

                return Ok("Pass created successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while creating a pass");

                return StatusCode(500, "An error occurred while creating the pass");
            }



            
        }

        // Pass status update endpoint
        [Authorize(Roles = "Manager")]
        [HttpPut("updatepass/{id}")]
        public async Task<IActionResult> UpdatePassStatus(string id, [FromBody] PassDTO pass)
        {
            //  logic to update pass status
            var pass = await _context.Passes.FindAsync(id);

            if (pass == null)
            {
                return NotFound("Pass not found");
            }

            // Update pass status with values from updatedPass DTO
            pass.Status = updatedPass.Status;
            // Update other properties as needed

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return Ok("Pass status updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while updating pass status");

                return StatusCode(500, "An error occurred while updating the pass status");
            }

            
        }

        // Get visitors by status endpoint
        [Authorize(Roles = "Manager")]
        [HttpGet("visitorsbystatus")]
        public async Task<IActionResult> GetVisitorsByStatus(string status)
        {
            //  logic to retrieve visitors by status
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status is required");
            }

            try
            {
                // Retrieve visitors filtered by status
                var visitors = await _context.Visitors.Where(v => v.Status == status).ToListAsync();

                return Ok(visitors);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error occurred while retrieving visitors by status");

                return StatusCode(500, "An error occurred while retrieving visitors by status");
            }

           
        }

        // Email service for sending notifications
        [Authorize(Roles = "Manager, Security")]
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO email)
        {
            var apiKey = _configuration["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"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("tidkeshubham10@gmail.com");
            var subject = email.Subject;
            var tos = email.RecipientEmails.ConvertAll(email => new EmailAddress(email));
            var plainTextContent = email.Content;
            var htmlContent = email.Content;
           

            await client.SendEmailAsync(MSG);

            return Ok("Email sent successfully");
        }
    }
}
