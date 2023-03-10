using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
      var employees = await _context.Employees
        .Include(e => e.Job)
        .Include(e => e.Rank)
        .ToListAsync();

      return View(employees);
    }

    //GET: Employees/Create
    public IActionResult Create()
    {
      ViewBag.JobsTitle = new SelectList(_context.Jobs, "Id", "Title");
      ViewBag.RanksTitle = new SelectList(_context.Ranks, "Id", "Title");
      return View();
    }

    //POST: Employees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id, Rank_Id, Job_Id, FirstName, LastName, PhoneNumber")] Employee employee)
    {
      if(employee.Job_Id == 0)
      {
        ModelState.AddModelError("Job_Id", "Please select a job.");
      }
      if(employee.Rank_Id == 0)
      {
        ModelState.AddModelError("Rank_Id", "Please select a rank.");
      }
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

      ViewBag.JobsTitle = new SelectList(_context.Jobs, "Id", "Title", employee.Job_Id);
      ViewBag.RanksTitle = new SelectList(_context.Ranks, "Id", "Title", employee.Rank_Id);

      return View(employee);
    }

    //POST: Employees/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
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
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Employees == null)
      {
        return NotFound();
      }

      var employee = await _context.Employees
        .Include(e => e.Job)
        .Include(e => e.Rank)
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