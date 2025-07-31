namespace Demo.Presentation.Models;

public class ForgetPasswordViewModel
{
    [EmailAddress]
    public string Email { get; set; }
}
