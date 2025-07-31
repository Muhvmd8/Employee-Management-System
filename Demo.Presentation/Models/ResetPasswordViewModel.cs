namespace Demo.Presentation.Models;

public class ResetPasswordViewModel
{
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password & Confirm Password Doesn't Match")]
    public string ConfirmPassword { get; set; }
    [HiddenInput]
    public string Email { get; set; }
    [HiddenInput]
    public string Token { get; set; }
}
