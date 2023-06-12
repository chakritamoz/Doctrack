using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;
using Doctrack.Data;
using Doctrack.Authentication;

namespace Doctrack.Controllers
{
  public class DocumentTypesController : Controller
  {
    private readonly DoctrackContext _context;

    public DocumentTypesController(DoctrackContext context)
    {
      _context = context;
    }

    //GET: DocumentTypes/Index
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Index(string? queryStr, bool? isSearch)
    {      
      var documentTypes = await _context.DocumentTypes
        .Where(dt => string.IsNullOrEmpty(queryStr)
          || dt.Title.Contains(queryStr)
        )
        .ToListAsync();

      if (isSearch ?? false)
      {
        return PartialView("_DocumentTypesTable", documentTypes);
      }

      return View(documentTypes);
    }

    //GET: DocumentTypes/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public ActionResult Create()
    {
      return View();
    }

    //POST: DocumentTypes/Create
    [HttpPost]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Create([Bind("Id, Title, PeriodWarning, PeriodEnd")] DocumentType documentType)
    {
      if (ModelState.IsValid)
      {
       _context.Add(documentType);
       await _context.SaveChangesAsync();
       return RedirectToAction(nameof(Index));
      }
      else
      {
        Console.WriteLine("Model State is invalid.");
      }

      return View(documentType);
    }

    //GET: DocumentTypes/Edit/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.DocumentTypes == null)
      {
        return NotFound();
      }

      var documentType = await _context.DocumentTypes.FindAsync(id);
      if (documentType == null)
      {
        return NotFound();
      }

      return View(documentType);
    }

    //POST: DocumentTypes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title, PeriodWarning, PeriodEnd")] DocumentType documentType)
    {
      if (_context.DocumentTypes == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.DocumentTypes.Update(documentType);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch(DbUpdateException)
        {
          if (!DocumentTypeExists(documentType.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
      }
      return View(documentType);
    }

    //GET: DocumentTypes/Delete/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.DocumentTypes == null)
      {
        return NotFound();
      }

      var documentType = await _context.DocumentTypes
        .FirstOrDefaultAsync(dt => dt.Id == id);
      if (documentType == null)
      {
        return NotFound();
      }

      return View(documentType);
    }

    //POST: DocumentTypes/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Delete(int id)
    {
      if (_context.DocumentTypes == null)
      {
        return Problem("Enitity set 'DoctrankContext.DocumentTypes' is null.");
      }

      var documentType = await _context.DocumentTypes.FindAsync(id);
      if (documentType == null)
      {
        return NotFound();
      }

      _context.DocumentTypes.Remove(documentType);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool DocumentTypeExists(int id)
    {
      return (_context.DocumentTypes?.Any(dt => dt.Id == id)).GetValueOrDefault();
    }
  }
}