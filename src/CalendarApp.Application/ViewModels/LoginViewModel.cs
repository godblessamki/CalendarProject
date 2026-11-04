namespace CalendarApp.Application.ViewModels;

using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
  [Required(ErrorMessage = "E-mail je povinný.")]
  [EmailAddress(ErrorMessage = "Zadajte platnú e-mailovú adresu.")]
  [Display(Name = "E-mail")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Heslo je povinné.")]
  [DataType(DataType.Password)]
  [Display(Name = "Heslo")]
  public string Password { get; set; } = string.Empty;

  [Display(Name = "Zapamätať prihlásenie")]
  public bool RememberMe { get; set; }

  public string? ReturnUrl { get; set; }
}