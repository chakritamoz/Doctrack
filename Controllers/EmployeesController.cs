using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Doctrack.Authentication;

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
    [AuthenticationFilter]
    public async Task<IActionResult> Index()
    {
      var employees = await _context.Employees
        .ToListAsync();

      return View(employees);
    }

    [AuthenticationFilter]
    public async Task<IActionResult> SearchEmployee(string queryStr)
    {
      if (_context.Employees == null)
      {
        return NotFound();
      }

      var employees = await _context.Employees.ToListAsync();
      if (!String.IsNullOrEmpty(queryStr))
      {
        employees = employees
          .Where(emp => $"{emp.FirstName} {emp.LastName}".Contains(queryStr))
          .ToList();
      }
      
      return PartialView("_EmployeeTable", employees);
    }

    //GET: Employees/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public IActionResult Create()
    {
      ViewBag.JobsTitle = new SelectList(_context.Jobs, "Id", "Title");
      ViewBag.RanksTitle = new SelectList(_context.Ranks, "Id", "Title");
      return View();
    }

    //POST: Employees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Create([Bind("Id, Rank_Id, Job_Id, FirstName, LastName, PhoneNumber")] Employee employee)
    {
      if (ModelState.IsValid)
      {
        _context.Add(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      ViewBag.JobsTitle = new SelectList(_context.Jobs, "Id", "Title");
      ViewBag.RanksTitle = new SelectList(_context.Ranks, "Id", "Title");

      return View(employee);
    }

    //GET: Employees/Edit/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Employees == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees.FindAsync(id);
      if (employee == null)
      {
        return NotFound();
      }

      return View(employee);
    }

    //POST: Employees/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Rank_Id, Job_Id, FirstName, LastName", "PhoneNumber")] Employee employee)
    {
      if (_context.Employees == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(employee);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch(DbUpdateException)
        {
          if (!EmployeeExists(employee.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
      }
      return View(employee);
    }

    //GET: Employees/Delete/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Employees == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.Id == id);
      if (employee == null)
      {
        return NotFound();
      }

      return View(employee);
    }

    //POST: Employees/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Delete(int id)
    {
      if (_context.Employees == null)
      {
        return Problem("Enitity set 'DoctrackContext.Employees' is null.");
      }

      var employee = await _context.Employees.FindAsync(id);
      if (employee == null)
      {
        return NotFound();
      }

      _context.Remove(employee);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool EmployeeExists(int id)
    {
      return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}