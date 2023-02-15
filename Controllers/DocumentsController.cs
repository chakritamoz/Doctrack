using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;

namespace Doctrack.Controllers
{
  public class DocumentsController : Controller
  {
    private readonly DoctrackContext _context;
    public DocumentsController(DoctrackContext context)
    {
      _context = context;
    }

    public async Task<IActionResult> Index()
    {
      var documents = await _context.DocumentDetails
        .Include(dd => dd.Document)
        .Include(dd => dd.Employee)
        .ToListAsync();
        
      return View(documents);
    }
  }
}