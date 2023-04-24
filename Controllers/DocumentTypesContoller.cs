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
    public async Task<IActionResult> Index()
    {      
      ViewBag.currentUser = GetUsername();
      var documentTypes = await _context.DocumentTypes.ToListAsync();

      return View(documentTypes);
    }

    [AuthenticationFilter]
    public async Task<IActionResult> SearchDocType(string queryStr)
    {
      ViewBag.currentUser = GetUsername();
      if (_context.DocumentTypes == null)
      {
        return NotFound();
      }
      var documentTypes = await _context.DocumentTypes.ToListAsync();

      if (!String.IsNullOrEmpty(queryStr))
      {
        documentTypes = documentTypes
        .Where(docType => docType.Title.Contains(queryStr))
        .ToList();
      }

      return PartialView("_DocumentTypesTable", documentTypes);
    }

    //GET: DocumentTypes/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public ActionResult Craete()
    {
      ViewBag.currentUser = GetUsername();
      return View();
    }

    //POST: DocumentTypes/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Create([Bind("Id, Title, Period")] DocumentType documentType)
    {
      ViewBag.currentUser = GetUsername();
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
    public async Task<IActionResult> Edit(int? id)
    {
      ViewBag.currentUser = GetUsername();
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
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title, PeriodWarning, PeriodEnd")] DocumentType documentType)
    {
      ViewBag.currentUser = GetUsername();
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
    public async Task<IActionResult> Delete(int? id)
    {
      ViewBag.currentUser = GetUsername();
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
    public async Task<IActionResult> Delete(int id)
    {
      ViewBag.currentUser = GetUsername();
      if (_context.DocumentTypes == null)
      {
        return Problem("Enitity set 'DoctrankContext.DocumentTypes' is null.");
      }

      var documentType = await _context.DocumentTypes.FindAsync(id);
      if (documentType == null)
      {
        return NotFound();
      }

      _context.Remove(id);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool DocumentTypeExists(int id)
    {
      return (_context.DocumentTypes?.Any(dt => dt.Id == id)).GetValueOrDefault();
    }

    public string GetUsername() {
      return HttpContext.Session.GetString("Username");;
    }
  }
}