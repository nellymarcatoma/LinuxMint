namespace NorthWind.Membership.Entities.Resources;

public class UserRegistrationMessages
{
    public static string RequiredFirstNameErrorMessage => "El nombre es requerido.";
    public static string RequiredLastNameErrorMessage => "El apellido es requerido.";
    public static string RequiredEmailErrorMessage => "El correo es requerido.";
    public static string InvalidEmailErrorMessage => "El formato de correo no es válido.";
    public static string RequiredPasswordErrorMessage => "La contraseña es requerida.";
    public static string PasswordTooShortErrorMessage => "Se requiere al menos 6 caracteres en la contraseña.";
    public static string PasswordRequiresLowerErrorMessage => "Se requiere al menos un caracter minúscula en la contraseña.";
    public static string PasswordRequiresUpperErrorMessage => "Se requiere al menos un caracter mayúscula en la contraseña.";
    public static string PasswordRequiresDigitErrorMessage => "Se requiere al menos un dígito en la contraseña.";
    public static string PasswordRequiresNonAlphanumericErrorMessage => "Se requiere al menos un caracter no alfanumérico en la contraseña.";
}
