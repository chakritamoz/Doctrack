using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class DocumentViewModel
  {
    public List<Document> Documents {get; set;}
    public List<DocumentDetail> DocumentsDetail {get; set;}
  }
}