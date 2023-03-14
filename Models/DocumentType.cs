using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class DocumentType
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter document title.")]
    public string Title {get; set;}
    public int? PeriodWarning {get; set;}
    public int? PeriodEnd {get; set;} = 7;
    public virtual ICollection<Document>? Documents {get; set;}
  }
}