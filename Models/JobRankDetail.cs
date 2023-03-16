using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class JobRankDetail
  {
    [Key]
    public int Id {get; set;}
    public int Job_Id {get; set;}
    [ForeignKey("Job_Id")]
    public virtual Job? Job {get; set;}
    public int Rank_Id {get; set;}
    [ForeignKey("Rank_Id")]
    public virtual Rank? Rank {get; set;}
  }
}