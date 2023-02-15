using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class Document
  {
    [Key]
    [Required(ErrorMessage = "Please enter document ID.")]
    public string Id {get; set;} 
    public int DocType_Id {get; set;}
    [ForeignKey("DocType_Id")]
    public virtual DocumentType? DocumentType {get; set;}
    [Required(ErrorMessage = "Please enter receipt date.")]
    [DataType(DataType.Date)]
    public DateTime ReceiptDate {get; set;} = DateTime.Now;
    [DataType(DataType.Date)]
    public DateTime? EndDate {get; set;}
    public string? Operation {get; set;}
    [DataType(DataType.Date)]
    public DateTime? OperationDate {get; set;}
    public string? CommandOrder {get; set;}
    public string? RemarkAll {get; set;}
    public string User {get; set;}
    public virtual ICollection<DocumentDetail>? DocumentDetails {get; set;}
  }
}