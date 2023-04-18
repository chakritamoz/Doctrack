using Microsoft.EntityFrameworkCore;
using Doctrack.Models;

namespace Doctrack.Data
{
  public class DoctrackContext : DbContext
  {
    public DoctrackContext(DbContextOptions<DoctrackContext> options)
      : base(options)
      {
      }

    public DbSet<Job> Jobs {get; set;}
    public DbSet<Rank> Ranks {get; set;}
    public DbSet<DocumentType> DocumentTypes {get; set;}
    public DbSet<Document> Documents {get; set;}
    public DbSet<Employee> Employees {get; set;}
    public DbSet<DocumentDetail> DocumentDetails {get; set;}
    public DbSet<JobRankDetail> JobRankDetails {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Role> Roles {get; set;}
  }
}