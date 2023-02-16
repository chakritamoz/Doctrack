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
      var documents = await _context.DocumentDetails
        .Include(dd => dd.Document)
        .Include(dd => dd.Employee)
        .ToListAsync();

      return View(documents);
    }

    //GET: Documents/Create
    public IActionResult Create()
    {
      var viewModel = new DocumentViewModel
      {
        Employee = new Employee(),
        Document = new Document(),
        DocumentDetail = new DocumentDetail()
      };

      ViewBag.JobsTitle = GetJobSelectList();
      ViewBag.RanksTitle = GetRankSelectList();
      ViewBag.DocTypesTitle = GetDocTypeSelectList();
      ViewBag.ReceiptDate = DateTime.Now.ToString("dd/MM/yyyy");
      
      return View(viewModel);
    }

    //POST: Documents/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DocumentViewModel viewModel)
    {


      if (ModelState.IsValid)
      {
        Console.WriteLine($"doc title: {viewModel.Document.DocType_Id}");
        Console.WriteLine($"job title: {viewModel.Employee.Job_Id}");
        Console.WriteLine($"your title: {viewModel.Employee.Rank_Id}");
        Console.WriteLine($"first name: {viewModel.Employee.FirstName}");
        Console.WriteLine($"last name: {viewModel.Employee.LastName}");
        Console.WriteLine($"receipt date: {viewModel.Document.ReceiptDate}");
      }else
      {
        ViewBag.JobsTitle = GetJobSelectList();
        ViewBag.RanksTitle = GetRankSelectList();
        ViewBag.DocTypesTitle = GetDocTypeSelectList();
        ViewBag.ReceiptDate = DateTime.Now.ToString("dd/MM/yyyy");
        foreach (var model in ModelState)
        {
          Console.WriteLine($"key: {model.Key}\nerror: {model.Value.Errors}");
        }
      }
      return View(viewModel);
    }

    public SelectList GetDocTypeSelectList() {
      var docTypeSelectList = new SelectList(_context.DocumentTypes, "Id", "Title");
      
      return docTypeSelectList;
    }
    public SelectList GetJobSelectList() {
      var jobSelectList = new SelectList(_context.DocumentTypes, "Id", "Title");
      
      return jobSelectList;
    }
    public SelectList GetRankSelectList() {
      var rankSelectList = new SelectList(_context.DocumentTypes, "Id", "Title");
      
      return rankSelectList;
    }
  }
}