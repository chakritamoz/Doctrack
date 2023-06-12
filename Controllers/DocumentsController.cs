using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using Doctrack.Authentication;
using System.Globalization;
using OfficeOpenXml;


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
    [AuthenticationPrivilege]
    public async Task<IActionResult> Index(string? queryDocNo, string? queryDocType, string? queryDocTitle, string? queryEmployee, string? tabType, bool? isSearch)
    { 
      var currentUser = HttpContext.Session.GetString("Username");

      if (_context.Documents == null) return NotFound();

      // Retrieve data
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
        .Where(d => tabType == "all"
          || d.User == currentUser
        )
        .ToListAsync();

      // Order by data
      documents = documents
        .OrderByDescending(d =>
            d.EndDate == null &&
            (DateTime.Now - d.OperationDate) >= 
            TimeSpan.FromDays(Convert.ToDouble(d.DocumentType.PeriodEnd))
        )
        .ThenByDescending(d =>
            d.EndDate == null &&
            (DateTime.Now - d.OperationDate) >= 
            TimeSpan.FromDays(Convert.ToDouble(d.DocumentType.PeriodWarning))
        )
        .ThenBy(d => d.EndDate)
        .ThenByDescending(d => d.ReceiptDate)
        .ThenBy(d => d.DocumentType.Title)
        .ThenBy(d => d.Id)
        .Take(20)
        .ToList();

      if (isSearch ?? false) {
        return PartialView("_DocumentTable", documents);
      }

      return View(documents);
    }

    //GET: Documents/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public IActionResult Create()
    {
      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      
      return View();
    }

    //POST: Documents/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Create(Document document,string receiptDate)
    {
      if (document.DocType_Id == 0)
      {
        ModelState.AddModelError("DocType_Id", "Please select document title.");
      }

      var doc = await _context.Documents
        .FirstOrDefaultAsync(d => d.Id == document.Id);
      if (doc != null)
      {
        ModelState.AddModelError("Id", "Document No is already exists.");
      }

      if (receiptDate != null)
      {
        var receiptArray = receiptDate.Split("/");
        var conReceiptDate = $"{receiptArray[2]}-{receiptArray[1]}-{receiptArray[0]}";
        if (DateTime.TryParseExact(conReceiptDate, "yyyy-MM-dd", CultureInfo.DefaultThreadCurrentCulture,DateTimeStyles.None,out var parsedDate))
        {
          document.ReceiptDate = parsedDate;
          ModelState.Remove("ReceiptDate");
        }
      } // End validate receipt date

      if (ModelState.IsValid)
      {
        _context.Add(document);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      return View(document);
    }

    //GET: Documents/Edit/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Edit(string? id)
    {
      if (id == null || _context.Documents == null)
      {
        return NotFound();
      }
      id = id.Replace("_","/");
      var document = await _context.Documents.FindAsync(id);
      if (document == null)
      {
        return NotFound();
      }

      if (document.ReceiptDate != null)
      {
        var parsedDate = DateTime.Parse(document.ReceiptDate.ToString());
        ViewBag.ReceiptDate = parsedDate.ToString("dd/MM/yyyy");
      }

      if (document.OperationDate != null)
      {
        var parsedDate = DateTime.Parse(document.OperationDate.ToString());
        ViewBag.OpDate = parsedDate.ToString("dd/MM/yyyy");
      }

      if (document.EndDate != null)
      {
        var parsedDate = DateTime.Parse(document.EndDate.ToString());
        ViewBag.EndDate = parsedDate.ToString("dd/MM/yyyy");
      }

      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      return View(document);
    }

    //POST: Documents/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Edit(string id, string? newId, Document document, string receiptDate, string? opDate, string? endDate)
    {
      if (_context.Documents == null)
      {
        return NotFound();
      }

      if (receiptDate != null)
      {
        var receiptArray = receiptDate.Split("/");
        var conReceiptDate = $"{receiptArray[2]}-{receiptArray[1]}-{receiptArray[0]}";
        if (DateTime.TryParseExact(conReceiptDate, "yyyy-MM-dd", CultureInfo.DefaultThreadCurrentCulture,DateTimeStyles.None,out var parsedDate))
        {
          document.ReceiptDate = parsedDate;
          ModelState.Remove("ReceiptDate");
        }
      } // End validate receipt date

      ModelState.Remove("OperationDate");
      if (opDate != null)
      {
        var opArray = opDate.Split("/");
        var conOPDate = $"{opArray[2]}-{opArray[1]}-{opArray[0]}";
        if (DateTime.TryParseExact(conOPDate, "yyyy-MM-dd", CultureInfo.DefaultThreadCurrentCulture,DateTimeStyles.None,out var parsedDate))
        {
          document.OperationDate = parsedDate;
        }
      } // End validate operation date

      ModelState.Remove("EndDate");
      if (endDate != null)
      {
        var endArray = endDate.Split("/");
        var conEndDate = $"{endArray[2]}-{endArray[1]}-{endArray[0]}";
        if (DateTime.TryParseExact(conEndDate, "yyyy-MM-dd", CultureInfo.DefaultThreadCurrentCulture,DateTimeStyles.None,out var parsedDate))
        {
          document.EndDate = parsedDate;
        }
      } // End validate operation date

      if (ModelState.IsValid)
      {
        try
        {
          if (newId == null)
          {
            document.Id = document.Id.Replace("_","/");
            _context.Update(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }

          id = id.Replace("_","/");
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
      ViewBag.Today = DateTime.Now.ToString("dd/MM/yyyy");
      return View(document);
    }

    //POST: Documents/AddEmployee/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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
    [AuthenticationPrivilege]
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

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<ActionResult>  ExportExcel()
    {
      if (_context.Documents == null)
      {
        return Problem("Enitity set 'DoctrackContext.Documents' is null.");
      }

      // var currentUser = HttpContext.Session.GetString("Username");
      var documents = await _context.Documents
        .Include(d => d.DocumentType)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Employee)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Job)
        .Include(d => d.DocumentDetails)
          .ThenInclude(dd => dd.Rank)
        // .Where(d => d.User == currentUser)
        .ToListAsync();

      // Create a new Excel package
      using (var package = new ExcelPackage())
      {
        // Create a worksheet
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Set the column headers
        worksheet.Cells[1,1].Value = "Receipt Date";
        worksheet.Cells[1,2].Value = "Document No.";
        worksheet.Cells[1,3].Value = "Document Type";
        worksheet.Cells[1,4].Value = "Document Title";
        worksheet.Cells[1,5].Value = "Operation";
        worksheet.Cells[1,6].Value = "Operation Date";
        worksheet.Cells[1,7].Value = "Command Order";
        worksheet.Cells[1,8].Value = "End Date";
        worksheet.Cells[1,9].Value = "Document Remark";
        worksheet.Cells[1,10].Value = "Job";
        worksheet.Cells[1,11].Value = "Title";
        worksheet.Cells[1,12].Value = "First Name";
        worksheet.Cells[1,13].Value = "Last Name";
        worksheet.Cells[1,14].Value = "Remark";
        worksheet.Cells[1,15].Value = "User";

        // Populate the data rows
        var rowCount = 0;
        foreach (var doc in documents)
        {
          if (doc.DocumentDetails.Count > 0)
          {
            foreach (var docd in doc.DocumentDetails)
            {
              worksheet.Cells[rowCount + 2, 1].Value = doc.ReceiptDate;
              worksheet.Cells[rowCount + 2, 1].Style.Numberformat.Format = "dd/MM/yyyy";
              worksheet.Cells[rowCount + 2, 2].Value = doc.Id;
              worksheet.Cells[rowCount + 2, 3].Value = doc.DocumentType?.Title;
              worksheet.Cells[rowCount + 2, 4].Value = doc.Doc_Title;
              worksheet.Cells[rowCount + 2, 5].Value = doc.Operation;
              worksheet.Cells[rowCount + 2, 6].Value = doc.OperationDate;
              worksheet.Cells[rowCount + 2, 6].Style.Numberformat.Format = "dd/MM/yyyy";
              worksheet.Cells[rowCount + 2, 7].Value = doc.CommandOrder;
              worksheet.Cells[rowCount + 2, 8].Value = doc.EndDate;
              worksheet.Cells[rowCount + 2, 8].Style.Numberformat.Format = "dd/MM/yyyy";
              worksheet.Cells[rowCount + 2, 9].Value = doc.RemarkAll;
              worksheet.Cells[rowCount + 2, 10].Value = docd.Job?.Title;
              worksheet.Cells[rowCount + 2, 11].Value = docd.Rank?.Title;
              worksheet.Cells[rowCount + 2, 12].Value = docd.Employee?.FirstName;
              worksheet.Cells[rowCount + 2, 13].Value = docd.Employee?.LastName;
              worksheet.Cells[rowCount + 2, 14].Value = docd.Remark;
              worksheet.Cells[rowCount + 2, 15].Value = doc.User;
              
              rowCount++; // increament row
            } // foreach document detail
          } // if document detail more than 0
          else
          {
            worksheet.Cells[rowCount + 2, 1].Value = doc.ReceiptDate;
            worksheet.Cells[rowCount + 2, 1].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells[rowCount + 2, 2].Value = doc.Id;
            worksheet.Cells[rowCount + 2, 3].Value = doc.DocumentType?.Title;
            worksheet.Cells[rowCount + 2, 4].Value = doc.Doc_Title;
            worksheet.Cells[rowCount + 2, 5].Value = doc.Operation;
            worksheet.Cells[rowCount + 2, 6].Value = doc.OperationDate;
            worksheet.Cells[rowCount + 2, 6].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells[rowCount + 2, 7].Value = doc.CommandOrder;
            worksheet.Cells[rowCount + 2, 8].Value = doc.EndDate;
            worksheet.Cells[rowCount + 2, 8].Style.Numberformat.Format = "dd/MM/yyyy";
            worksheet.Cells[rowCount + 2, 9].Value = doc.RemarkAll;
            worksheet.Cells[rowCount + 2, 10].Value = "";
            worksheet.Cells[rowCount + 2, 11].Value = "";
            worksheet.Cells[rowCount + 2, 12].Value = "";
            worksheet.Cells[rowCount + 2, 13].Value = "";
            worksheet.Cells[rowCount + 2, 14].Value = "";
            worksheet.Cells[rowCount + 2, 15].Value = doc.User;

            rowCount++; // increament row
          }
        }

        // Auto-fit columns for better readability
        worksheet.Cells.AutoFitColumns();

        // Convert the Excel package to a byte array
        var excelBytes = package.GetAsByteArray();

        // Return the Excel file as a download
        // return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
      }
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [HttpPost]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
      if (file != null && file.Length > 0)
      {
        var docTypes = _context.DocumentTypes.ToList();
        var jobs = _context.Jobs.ToList();
        var ranks = _context.Ranks.ToList();
        var emps = _context.Employees.ToList();
        var docds = _context.DocumentDetails.ToList();

        using (var package = new ExcelPackage(file.OpenReadStream()))
        {
          // Read worksheet 1
          var worksheet = package.Workbook.Worksheets[0];
          int rowCount = worksheet.Dimension.Rows;

          for (int row = 2; row <= rowCount; row++)
          {
            // Get document receipt date
            DateTime ReceiptDate = ConvertExcelDate(worksheet.Cells[row, 1].Value?.ToString());

            // Get document Id
            var Id = worksheet.Cells[row, 2].Value?.ToString();

            // Get document type id
            var DocType = docTypes.FirstOrDefault(
              dt => dt.Title == worksheet.Cells[row, 3].Value?.ToString()
            );
            if (DocType == null) continue;

            // Get document title
            var Doc_Title = worksheet.Cells[row, 4].Value?.ToString();

            // Get document operation
            var Operation = worksheet.Cells[row, 5].Value?.ToString();

            DateTime? OperationDate = null;
            if (worksheet.Cells[row, 6].Value?.ToString() != null)
            {
              // Get document operation date
              OperationDate = ConvertExcelDate(worksheet.Cells[row, 6].Value?.ToString());
            }

            // Get document command order
            var CommandOrder = worksheet.Cells[row, 7].Value?.ToString();

            DateTime? EndDate = null;
            if (worksheet.Cells[row, 8].Value?.ToString() != null)
            {
              // Get document end date
              EndDate = ConvertExcelDate(worksheet.Cells[row, 8].Value?.ToString());
            }

            // Get documnet remark all
            var RemarkAll = worksheet.Cells[row, 9].Value?.ToString();

            // Get document user
            var User = HttpContext.Session.GetString("Username");

            var doc = await _context.Documents.FindAsync(Id);
            if (doc == null)
            {
              var newDoc = new Document()
              {
                Id = Id,
                DocType_Id = DocType.Id,
                Doc_Title = Doc_Title,
                ReceiptDate = ReceiptDate,
                EndDate = EndDate,
                Operation = Operation,
                OperationDate = OperationDate,
                CommandOrder = CommandOrder,
                RemarkAll = RemarkAll,
                User = User,
                DocumentDetails = new List<DocumentDetail>()
              };

              _context.Documents.Add(newDoc);
              await _context.SaveChangesAsync();
            }

            var Job = jobs.FirstOrDefault(
              j => j.Title == worksheet.Cells[row, 10].Value?.ToString()
            );
            if (Job == null) continue;

            var Rank = ranks.FirstOrDefault(
              r => r.Title == worksheet.Cells[row, 11].Value?.ToString()
            );
            if (Rank == null) continue;

            // Get first name
            var FirstName = worksheet.Cells[row, 12].Value?.ToString();
            if (FirstName == null) continue;

            // Get last name
            var LastName = worksheet.Cells[row, 13].Value?.ToString();
            if (LastName == null) continue;

            var Remark = worksheet.Cells[row, 14].Value?.ToString();

            // Get employee
            var emp = await _context.Employees
              .FirstOrDefaultAsync(emp => 
                emp.FirstName == FirstName &&
                emp.LastName == LastName
            );
            if (emp == null)
            {
              var newEmp = new Employee {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = null,
                DocumentDetails = new List<DocumentDetail>()
              };

              _context.Employees.Add(newEmp);
              await _context.SaveChangesAsync();

              emp = await _context.Employees
              .FirstOrDefaultAsync(emp => 
                emp.FirstName == FirstName &&
                emp.LastName == LastName
              );
              if (emp == null) continue;
            }

            var docd = docds.FirstOrDefault(
              dd => dd.Emp_Id == emp.Id
            );
            if (docd != null) continue;

            var newDocd = new DocumentDetail {
              Doc_Id = Id,
              Job_Id = Job.Id,
              Rank_Id = Rank.Id,
              Emp_Id = emp.Id,
              Remark = Remark
            };

            _context.DocumentDetails.Add(newDocd);
            await _context.SaveChangesAsync();
          } // For loop
        } // Using
      } // If file not null
      return Json(new { success = true });
    }

    public bool DocumentExists(string id)
    {
      return (_context.Documents?.Any(doc => doc.Id == id)).GetValueOrDefault();
    }

    public SelectList GetDocTypeSelectList() {
      return (new SelectList(_context.DocumentTypes, "Id", "Title"));
    }

    public DateTime ConvertExcelDate(string excelValue) {
      if (double.TryParse(excelValue, out double numVal))
      {
        return DateTime.FromOADate(numVal);
      }
      else if (DateTime.TryParse(excelValue, out DateTime dateVal))
      {
        return dateVal;
      }
      else
      {
        return DateTime.MinValue;
      }
    }
  }
}
