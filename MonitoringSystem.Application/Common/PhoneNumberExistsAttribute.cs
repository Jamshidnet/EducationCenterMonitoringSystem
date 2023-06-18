using MonitoringSystem.Application.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MonitoringSystem.Application.Common;

public class PhoneNumberExistsAttribute : ValidationAttribute
{
    IApplicationDbContext _dbContext { get; set; }

    public PhoneNumberExistsAttribute(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var phoneNumber = value as string;

        // Check if the phone number is null or empty
        if (string.IsNullOrEmpty(phoneNumber))
        {
            // Return success since the field is optional
            return ValidationResult.Success;
        }

        // Check if the phone number exists in the database
        bool phoneNumberExists = CheckPhoneNumberExists(phoneNumber);

        if (phoneNumberExists)
        {
            // Return an error message
            return new ValidationResult(ErrorMessage ?? "This phoneNumber is busy");
        }

        return ValidationResult.Success;
    }

    private bool CheckPhoneNumberExists(string phoneNumber) =>
      _dbContext.Students.FirstOrDefault(x => x.PhoneNumber == phoneNumber) is not null;

}
