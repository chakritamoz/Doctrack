using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
      var documents = await _context.Documents
        .Include(d => d.DocumentType)
        .Include(d => d.DocumentDetails)
        .ToListAsync();

      var documentsDetail = await _context.DocumentDetails
        .Include(dd => dd.Document)
        .Include(dd => dd.Employee)
        .Include(dd => dd.Employee.Job)
        .Include(dd => dd.Employee.Rank)
        .ToListAsync();

      var viewModel = new DocumentViewModel
      {
        Documents = documents,
        DocumentsDetail = documentsDetail
      };

      return View(viewModel);
    }

    //GET: Documents/Create
    public IActionResult Create()
    {
      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.User = GetMachineName();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      
      return View();
    }

    //POST: Documents/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Document document)
    {
      if (document.DocType_Id == 0)
      {
        ModelState.AddModelError("DocType_Id", "Please select document title");
      }

      if (ModelState.IsValid)
      {
        _context.Add(document);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.User = GetMachineName();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      return View(document);
    }

    //GET: Documents/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
      if (id == null || _context.Documents == null)
      {
        return NotFound();
      }
      
      var document = await _context.Documents.FindAsync(id);
      if (document == null)
      {
        return NotFound();
      }

      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      ViewBag.User = GetMachineName();
      return View(document);
    }

    //POST: Documents/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, string? newId, Document document)
    {
      if (_context.Documents == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          if (newId == null)
          {
            _context.Update(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }

          var existsDocument = await _context.Documents
            .Include(doc => doc.DocumentDetails)
            .FirstOrDefaultAsync(doc => doc.Id == id);
          if (existsDocument == null)
          {
            return NotFound();
          }

          var newDocument = new Document()
          {
            Id = newId,
            DocType_Id = document.DocType_Id,
            ReceiptDate = document.ReceiptDate,
            EndDate = document.EndDate,
            Operation = document.Operation,
            OperationDate = document.OperationDate,
            CommandOrder = document.CommandOrder,
            RemarkAll = document.RemarkAll,
            User = document.User,
            DocumentDetails = new List<DocumentDetail>()
          };

          foreach (var docd in existsDocument.DocumentDetails)
          {
            docd.Doc_Id = newId;
            newDocument.DocumentDetails.Add(docd);
          }
            
          _context.Remove(existsDocument);

          _context.Add(newDocument);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch(DbUpdateException)
        {
          if (!DocumentExists(document.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }

      }

      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.User = GetMachineName();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      return View(document);
    }

    //POST: Documents/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
      if (_context.Documents == null)
      {
        return Problem("Enitity set 'DoctrackContext.Documents' is null.");
      }
      
      var document = await _context.Documents.FindAsync(id);
      if (document == null)
      {
        return NotFound();
      }

      _context.Remove(document);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool DocumentExists(string id)
    {
      return (_context.Documents?.Any(doc => doc.Id == id)).GetValueOrDefault();
    }

    public SelectList GetDocTypeSelectList() {
      return (new SelectList(_context.DocumentTypes, "Id", "Title"));
    }

    public SelectList GetJobSelectList() {
      return (new SelectList(_context.DocumentTypes, "Id", "Title"));
    }

    public SelectList GetRankSelectList() {      
      return (new SelectList(_context.DocumentTypes, "Id", "Title"));
    }

    public string GetMachineName() {
      return Environment.MachineName;
    }
  }
}