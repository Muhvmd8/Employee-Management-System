namespace Demo.BLL.Services.AttachmentService;
public class AttachmentService : IAttachmentService
{
    // Allowed Extensions 
    private List<string> _allowedExtensions = [".png", ".jpeg", ""];
    // Max Size 
    private const int MaxSize = 2_097_152;
    public async Task<string?> UploadAsync(IFormFile file, string folderName) // =>Images
    {
        // 1. Check Extension
        var extension = Path.GetExtension(file.FileName);
        if (!_allowedExtensions.Contains(extension)) return null;
        //2. Check Size 
        if (file.Length > MaxSize) return null;
        //3. Get Located Folder Path
        //Directory.GetCurrentDirectory() + "\wwwroot\Files\" + "folderName" 
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);
        //4. Make the Attachment Name Unique  => GUID 
        var fileName = $"{Guid.NewGuid()}{extension}";
        //5. Combine the FilePath 
        var filePath = Path.Combine(folderPath, fileName);
        //6. Create File Stream to be used for Copy {Unmanaged}
        using var stream = new FileStream(filePath, FileMode.Create);
        //7. use Stream to copy the file 
        await file.CopyToAsync(stream);
        //8. Return the fileName ==> to be stored in DB 
        return fileName;

    }

    public bool Delete(string filePath)
    {
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}
//FolderPath                                                               // File Name 
//D:\Route\Route\C 43\G03\07 Asp.Net Mvc\Demo\DemoMvc\Demo.Presentation\Views\_ViewImports.cshtml
