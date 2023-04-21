using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Doctrack.Authentication;
using Doctrack.SendGrid;

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
    [AuthenticationFilter]
    public async Task<IActionResult> Index()
    { 
      ViewBag.Username = HttpContext.Session.GetString("Username");
      var documents = await _context.Documents
        .Include(d => d.DocumentType)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Employee)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Job)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Rank)
        .ToListAsync();

      var orderDocument = documents
        .OrderBy(d => d.EndDate)
        .ThenBy(d => d.DocType_Id).ToList();

      return View(orderDocument);
    }

    [AuthenticationFilter]
    public async Task<IActionResult> SearchDocument(string queryDocNo, string queryDocType, string queryDocTitle, string queryEmployee)
    {
      if (_context.Documents == null)
      {
        return NotFound();
      }

      var documents = await _context.Documents
        .Include(d => d.DocumentType)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Employee)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Job)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Rank)
        .Where(d => string.IsNullOrEmpty(queryDocNo) 
          || d.Id.Contains(queryDocNo)
        )
        .Where(d => string.IsNullOrEmpty(queryDocType)
          || d.DocumentType.Title.Contains(queryDocType)
        )
        .Where(d => string.IsNullOrEmpty(queryDocTitle)
          || d.Doc_Title.Contains(queryDocTitle)
        )
        .Where(d => string.IsNullOrEmpty(queryEmployee)
          || d.DocumentDetails
          .Any(dd => (dd.Employee.FirstName + dd.Employee.LastName)
            .Contains(queryEmployee)
          )
        )
        .ToListAsync();

      var orderDocument = documents
        .OrderBy(d => d.EndDate)
        .ThenBy(d => d.DocType_Id).ToList();


      return PartialView("_DocumentTable", documents);
    }

    //GET: Documents/Create
    [AuthenticationFilter]
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
    [AuthenticationFilter]
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
    [AuthenticationFilter]
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
    [AuthenticationFilter]
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

    //POST: Documents/AddEmployee/5
    [AuthenticationFilter]
    public async Task<ActionResult> AddEmployee(string id, int jobId, int rankId, string firstName, string lastName, string? remark)
    {
      if (_context.DocumentDetails == null)
      {
        return NotFound();
      }

      var document = await _context.Documents.FindAsync(id);
      if (document == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees
        .FirstOrDefaultAsync(emp => 
          emp.FirstName == firstName &&
          emp.LastName == lastName
        );
      if (employee == null)
      {
        var newEmployee = new Employee {
          FirstName = firstName,
          LastName = lastName,
          PhoneNumber = null,
          DocumentDetails = new List<DocumentDetail>()
        };
        _context.Employees.Add(newEmployee);
        await _context.SaveChangesAsync();

        employee = await _context.Employees
        .FirstOrDefaultAsync(emp => 
          emp.FirstName == firstName &&
          emp.LastName == lastName
        );
        if (employee == null)
        {
          return NotFound();
        }
      }

      var documentDetail = new DocumentDetail {
        Doc_Id = id,
        Job_Id = jobId,
        Rank_Id = rankId,
        Emp_Id = employee.Id,
        Remark = remark
      };

      _context.DocumentDetails.Add(documentDetail);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
    }

    //GET: Documents/UpdateOP/5
    [AuthenticationFilter]
    public async Task<ActionResult> UpdateOP(string? id)
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

      var viewModel = new Document
      {
        Id = document.Id,
        DocType_Id = document.DocType_Id,
        Doc_Title = document.Doc_Title,
        ReceiptDate = document.ReceiptDate,
        EndDate = document.EndDate,
        Operation = document.Operation,
        OperationDate = document.OperationDate,
        CommandOrder = document.CommandOrder,
        RemarkAll = document.RemarkAll,
        User = document.User,
        DocumentDetails = new List<DocumentDetail>()
      };

      return Json(viewModel);
    }

    //POST: Documents/UpdateOP/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    public async Task<ActionResult> UpdateOP(string id, string Operation, DateTime OperationDate)
    {
      var existsModel = _context.Documents.Find(id);
      if (existsModel == null)
      {
        return NotFound();
      }
      existsModel.Operation = Operation;
      existsModel.OperationDate = OperationDate;
      
      _context.Update(existsModel);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
    }

    //GET: Document/GetAllJobs
    [AuthenticationFilter]
    public async Task<ActionResult> GetAllJobs()
    
    {
      if (_context.Jobs == null)
      {
        return NotFound();
      }
      var jobs = await _context.Jobs.ToListAsync();

      if (jobs == null)
      {
        return NotFound();
      }
      return Json(jobs);
    }

    //GET: Document/GetAllRanks/5
    [AuthenticationFilter]
    public async Task<ActionResult> GetAllRanks(int? id)
    {
      if (id == null || _context.JobRankDetails == null)
      {
        return NotFound();
      }
      var jobRankDet = await _context.JobRankDetails
        .Include(rankd => rankd.Rank)
        .Where(rankd => rankd.Job_Id == id)
        .ToListAsync();

      if (jobRankDet == null)
      {
        return NotFound();
      }
      
      var ranks = jobRankDet.Select(rankd => new {
        id = rankd.Rank?.Id,
        title = rankd.Rank?.Title
      });

      return Json(ranks);
    }

    //POST: Documents/DeleteEmployee/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
      if (_context.DocumentDetails == null)
      {
        return Problem("Enitity set 'DoctrackContext.DocumentDetails' is null.");
      }
      
      var documentDetail =  await _context.DocumentDetails.FindAsync(id);
      if (documentDetail == null)
      {
        return NotFound();
      }

      _context.DocumentDetails.Remove(documentDetail);
      await _context.SaveChangesAsync();
      return Json( new { success = true });
    }

    //GET: Documents/UpdateEmployee/5
    [AuthenticationFilter]
    public async Task<ActionResult> UpdateEmployee(int? id)
    {
      if (id == null || _context.DocumentDetails == null)
      {
        return NotFound();
      }

      var documentDetail = await _context.DocumentDetails
        .Include(docd => docd.Employee)
        .FirstOrDefaultAsync(docd => docd.Id == id);
      if (documentDetail == null)
      {
        return NotFound();
      }

      return Json(
          new {documentDetail},
          new JsonSerializerOptions { ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve}
        );
    }

    //POST: Documents/UpdateEmployee/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    public async Task<ActionResult> UpdateEmployee(int id, int jobId, int rankId, string? remark)
    {
      if (_context.DocumentDetails == null)
      {
        return NotFound();
      }

      var documentDetail = await _context.DocumentDetails.FindAsync(id);
      if (documentDetail == null)
      {
        return NotFound();
      }
      Console.WriteLine("retrieve docd");
      documentDetail.Job_Id = jobId;
      documentDetail.Rank_Id = rankId;
      documentDetail.Remark = remark;

      _context.DocumentDetails.Update(documentDetail);
      await _context.SaveChangesAsync();
      return Json( new { success = true });
    }

    //POST: Documents/UpdateEndDate/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    public async Task<ActionResult> UpdateEndDate(string id, DateTime EndDate)
    {
      var existsModel = _context.Documents.Find(id);
      if (existsModel == null)
      {
        return NotFound();
      }
      existsModel.EndDate = EndDate;
      
      _context.Update(existsModel);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
    }

    //POST: Documents/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    public async Task<IActionResult> Delete(string id)
    {
      if (_context.Documents == null)
      {
        return Problem("Enitity set 'DoctrackContext.Documents' is null.");
      }
      
      var document = await _context.Documents
        .FirstOrDefaultAsync(doc => doc.Id == id);
      if (document == null)
      {
        return NotFound();
      }

      _context.Remove(document);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
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
