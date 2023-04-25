using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;
using Doctrack.Data;
using Doctrack.Authentication;

namespace Doctrack.Controllers
{
  public class JobsController : Controller
  {
    private readonly DoctrackContext _context;
    public JobsController(DoctrackContext context)
    {
      _context = context;
    }

    //GET: Jobs/Index
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Index()
    {
      var jobs = await _context.Jobs.ToListAsync();

      return View(jobs);
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> SearchJob(string queryStr)
    {
      if (_context.Jobs == null)
      {
        return NotFound();
      }

      var jobs = await _context.Jobs.ToListAsync();
      if (!String.IsNullOrEmpty(queryStr))
      {
        jobs = jobs.Where(job => job.Title.Contains(queryStr)).ToList();
      }

      return PartialView("_JobsTable", jobs);
    }

    //GET: Jobs/Create
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public ActionResult Create()
    {
      return View();
    }

    //POST: Jobs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Create([Bind("Id, Title")] Job job)
    {
      if (ModelState.IsValid)
      {
        _context.Add(job);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      else 
      {
        Console.WriteLine("Model State is invalid.");
      }
      return View(job);
    }

    //GET: Jobs/Edit/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Jobs == null)
      {
        return NotFound();
      }

      var job = await _context.Jobs.FindAsync(id);
      if (job == null) {
        return NotFound();
      }
      
      return View(job);
    }

    //POST: Jobs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title")] Job job)
    {
      if (_context.Jobs == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(job);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch(DbUpdateException)
        {
          if (!JobExists(job.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
      }
      return View(job);
    }

    //GET: Jobs/Delete/5
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Jobs == null)
      {
        return NotFound();
      }

      var job = await _context.Jobs
        .FirstOrDefaultAsync(j => j.Id == id);
      if (job == null)
      {
        return NotFound();
      }

      return View(job);
    }

    //POST: Jobs/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Delete(int id)
    {
      if (_context.Jobs == null)
      {
        return Problem("Entity set 'DoctrackContext.Jobs' is null.");
      }

      var job = await _context.Jobs.FindAsync(id);
      if (job == null)
      {
        return NotFound();
      }

      _context.Jobs.Remove(job);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool JobExists(int id)
    {
      return (_context.Jobs?.Any(j => j.Id == id)).GetValueOrDefault();
    }
  }
}