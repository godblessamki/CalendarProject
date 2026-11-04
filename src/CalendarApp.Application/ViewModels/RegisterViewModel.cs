namespace CalendarApp.Application.ViewModels;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
  [Required]
  [StringLength(25, ErrorMessage = "Meno môže mať maximálne 25 znakov.")]
  [Display(Name = "Meno")]
  public string FirstName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Priezvisko je povinné.")]
  [StringLength(25, ErrorMessage = "Priezvisko môže mať maximálne 25 znakov.")]
  [Display(Name = "Priezvisko")]
  public string LastName { get; set; } = string.Empty;

  [Required(ErrorMessage = "E-mail je povinný.")]
  [EmailAddress(ErrorMessage = "Zadajte platnú e-mailovú adresu.")]
  [Display(Name = "E-mail")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Heslo je povinné.")]
  [StringLength(50, MinimumLength = 8, ErrorMessage = "Heslo musí mať aspoň {2} znakov.")]
  [DataType(DataType.Password)]
  [Display(Name = "Heslo")]
  public string Password { get; set; } = string.Empty;

  [DataType(DataType.Password)]
  [Display(Name = "Potvrdenie hesla")]
  [Compare("Password", ErrorMessage = "Heslá sa nezhodujú.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}