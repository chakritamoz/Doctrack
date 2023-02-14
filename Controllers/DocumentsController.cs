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

    //GET: Documents/Index
    public async Task<IActionResult> Index()
    {
      return View(await _context.DocumentTypes.ToListAsync());
    }
  }
}