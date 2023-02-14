using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;

namespace Doctrack.Controllers
{
  public class EmployeesController : Controller
  {
    private readonly DoctrackContext _context;
    public EmployeesController(DoctrackContext context)
    {
      _context = context;
    }

    //GET: Employees/Index
    public async Task<IActionResult> Index()
    {
      return View(await _context.Employees.ToListAsync());
    }

    //GET: Employees/Create
    public IActionResult Create()
    {
      return View();
    }

    public async Task<IActionResult> Create([Bind("Id, Rank_Id, Job_Id, FirstName, LastName, PhoneNumber")] Employee employee)
    {
      if (ModelState.IsValid)
      {
        _context.
      }

      return View(employee);
    }
  }
}