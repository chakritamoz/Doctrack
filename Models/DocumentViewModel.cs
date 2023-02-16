using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class DocumentViewModel
  {
    public Employee Employee {get; set;}
    public Document Document {get; set;}
    public DocumentDetail DocumentDetail {get; set;}
  }
}