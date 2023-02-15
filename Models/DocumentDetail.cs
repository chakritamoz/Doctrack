using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class DocumentDetail
  {
    [Key]
    public int Id {get; set;}
    public string Doc_Id {get; set;}
    [ForeignKey("Doc_Id")]
    public virtual Document? Document {get; set;}
    public int Emp_Id {get; set;}
    [ForeignKey("Emp_Id")]
    public virtual Employee? Employee {get; set;}
    public string? Remark {get; set;}
  }
}