using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;
using Doctrack.Data;

namespace Doctrack.Controllers
{
  public class RanksController : Controller
  {
    private readonly DoctrackContext _context;

    public RanksController(DoctrackContext context)
    {
      _context = context;
    }

    //GET: Ranks/Index
    public async Task<IActionResult> Index()
    {
      var ranks = await _context.Ranks.ToListAsync();
      return View(ranks);
    }

    public async Task<IActionResult> SearchRank(string queryStr)
    {
      if (_context.Ranks == null)
      {
        return NotFound();
      }

      var ranks = await _context.Ranks.ToListAsync();
      if (!String.IsNullOrEmpty(queryStr))
      {
        ranks = ranks
          .Where(rank => rank.Title.Contains(queryStr))
          .ToList();
      }

      return PartialView("_RanksTable", ranks);
    }

    //GET: Ranks/Create
    public ActionResult Create()
    {
      return View();
    }

    //POST: Ranks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id, Title")] Rank rank)
    {
      if (ModelState.IsValid)
      {
        _context.Add(rank);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      else
      {
        Console.WriteLine("Model State is invalid.");
      }

      return View(rank);
    }

    //GET: Ranks/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Ranks == null)
      {
        return NotFound();
      }

      var rank = await _context.Ranks.FindAsync(id);
      if (rank == null)
      {
        return NotFound();
      }
      return View(rank);
    }

    //POST: Ranks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title")] Rank rank)
    {
      if (_context.Ranks == null)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(rank);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch(DbUpdateException)
        {
          if (!rankExists(rank.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
      }

      var errors = ModelState
        .Where(er => er.Value.Errors.Count > 0)
        .Select(er => new { er.Key, er.Value.Errors })
        .ToArray();
      foreach ( var item in errors )
      {
        Console.WriteLine($"key: {item.Key}\nvalue: {item.Errors}");
      }
      return View(rank);
      
    }

    //GET: Ranks/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Ranks == null)
      {
        return NotFound();
      }

      var rank = await _context.Ranks
        .FirstOrDefaultAsync(r => r.Id == id);
      if (rank == null)
      {
        return NotFound();
      }

      return View(rank);
    }

    //POST: Ranks/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      if (_context.Ranks == null)
      {
        return Problem("Enitity set 'DoctrankContext.Ranks' is null.");
      }

      var rank = await _context.Ranks.FindAsync(id);
      if (rank == null)
      {
        return NotFound();
      }

      _context.Remove(id);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    public bool rankExists(int id)
    {
      return (_context.Ranks?.Any(r => r.Id == id)).GetValueOrDefault();
    }
  }
}