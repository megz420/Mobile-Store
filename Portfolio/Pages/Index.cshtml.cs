using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portfolio.Pages;

public class IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment) : PageModel
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    [BindProperty]
    public ContactForm Form { get; set; } = new();

    public bool FormSubmitted { get; private set; }

    public string? SubmittedName { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostContact()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Placeholder persistence: append submissions to App_Data/contact.json.
            // Swap for an email service (SendGrid/MailKit) or database when ready.
            var dataDir = Path.Combine(environment.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataDir);
            var file = Path.Combine(dataDir, "contact.json");

            var submissions = System.IO.File.Exists(file)
                ? JsonSerializer.Deserialize<List<ContactForm>>(await System.IO.File.ReadAllTextAsync(file), JsonOptions) ?? []
                : [];

            submissions.Add(Form);
            System.IO.File.WriteAllText(file, JsonSerializer.Serialize(submissions, JsonOptions));
        }
        catch (Exception ex)
        {
            // Never block the visitor on a persistence failure.
            logger.LogError(ex, "Failed to persist contact submission from {Email}", Form.Email);
        }

        logger.LogInformation("Contact message from {Name} <{Email}>: {Subject}", Form.Name, Form.Email, Form.Subject);
        SubmittedName = Form.Name;
        FormSubmitted = true;
        ModelState.Clear();
        Form = new ContactForm();
        return Page();
    }

    public class ContactForm
    {
        [Required(ErrorMessage = "Please tell me your name.")]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please add your email so I can reply.")]
        [EmailAddress(ErrorMessage = "That email address doesn't look right.")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please add a subject.")]
        [StringLength(150)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please write a short message.")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "The message must be at least {2} characters.")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty;
    }
}
