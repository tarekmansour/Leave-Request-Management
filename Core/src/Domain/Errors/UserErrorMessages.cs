namespace Domain.Errors;
public static class UserErrorMessages
{
    public static readonly string UserEmailShouldNotBeNullOrEmpty = "Invalid user email. The user email should not be null or empty.";
    public static readonly string UserPasswordShouldNotBeNullOrEmpty = "Invalid user password. The user password should not be null or empty.";
    public static readonly string UserNotFound = "Invalid user email. The user not found.";
    public static readonly string UserFirstNameShouldNotBeNullOrEmpty = "Invalid user first name. The user first name should not be null or empty.";
    public static readonly string UserLastNameShouldNotBeNullOrEmpty = "Invalid user last name. The user last name should not be null or empty.";
    public static readonly string PasswordLengthShouldHaveMinimumDigits = "Invalid password. The password should have at minimum a length of 8.";
    public static readonly string UserEmailNotUnique = "Invalid user Email. The provided email is not unique.";
}
