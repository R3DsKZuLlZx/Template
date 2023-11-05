using System.ComponentModel.DataAnnotations;

namespace Template.Infrastructure.FileStorage;

public class FileStorageOptions : IValidatableObject
{
    public const string Azure = "azure";
    public const string Folder = "folder";
    
    public string StorageType { get; set; } = Folder; // Azure, Folder
    
    // Used for folder storage
    public string? FolderPath { get; set; }
    
    // Used for azure blob storage
    public string? AzureStorageConnectionString { get; set; }
    public string? AzureStorageContainerName { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var storageType = StorageType.ToLowerInvariant();

        var allowedStorageTypes = new List<string>
        {
            Azure,
            Folder,
        };

        if (!allowedStorageTypes.Contains(storageType))
        {
            yield return new ValidationResult("StorageType not supported");
        }
        
        if (storageType == Folder && string.IsNullOrWhiteSpace(FolderPath))
        {
            yield return new ValidationResult("FolderPath cant be empty if StorageType is Folder");
        }
        
        if (storageType == Azure && string.IsNullOrWhiteSpace(AzureStorageConnectionString))
        {
            yield return new ValidationResult("AzureStorageConnectionString cant be empty if StorageType is Azure");
        }
        
        if (storageType == Azure && string.IsNullOrWhiteSpace(AzureStorageContainerName))
        {
            yield return new ValidationResult("AzureStorageContainerName cant be empty if StorageType is Azure");
        }
    }
}
