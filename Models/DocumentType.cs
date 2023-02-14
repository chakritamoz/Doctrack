using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class DocumentType
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter document title.")]
    public string Title {get; set;}
    [Required(ErrorMessage = "Please enter document period(day).")]
    public int? Period {get; set;} = 7;
    public virtual ICollection<Document> Documents {get; set;}
  }
}