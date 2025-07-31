namespace Demo.DAL.Models;
public class BaseEntity
{
    public int Id { get; set; } // PK 
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; } // user Id
    public DateTime CreatedOn { get; set; } // 
    public int LastModifiedBy { get; set; } // User Id 
    public DateTime LastModifiedOn { get; set; }
}
