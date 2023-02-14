using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;
using Doctrack.Data;

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
    public async Task<IActionResult> Index()
    {
      return View(await _context.Jobs.ToListAsync());
    }

    //GET: Jobs/Create
    public ActionResult Create()
    {
      return View();
    }

    //POST: Jobs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
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