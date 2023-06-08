using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Models;
using Doctrack.Data;
using Doctrack.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Index()
    {
      var ranks = await _context.Ranks.ToListAsync();
      return View(ranks);
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public ActionResult Create()
    {
      return View();
    }

    //POST: Ranks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
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
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
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

      _context.Ranks.Remove(rank);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> BindJob()
    {
      if (_context.Ranks == null) return NotFound();
      if (_context.Jobs == null) return NotFound();
      if (_context.JobRankDetails == null) return NotFound();

      ViewBag.JobTitle = GetJobsSelectList();
      ViewBag.RankTitle = GetRanksSelectList();

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> BindJob([Bind("Job_Id, Rank_Id")] JobRankDetail jobRankDetail)
    {
      if (_context.Ranks == null) return NotFound();
      if (_context.Jobs == null) return NotFound();
      if (_context.JobRankDetails == null) return NotFound();

      ViewBag.JobTitle = GetJobsSelectList();
      ViewBag.RankTitle = GetRanksSelectList();
      
      var job = await _context.Jobs.FindAsync(jobRankDetail.Job_Id);
      if (job == null)
      {
        ViewBag.JobError = "Job is invalid";
        return View();
      }

      var rank = await _context.Ranks.FindAsync(jobRankDetail.Rank_Id);
      if (rank == null)
      {
        ViewBag.RankError = "Rank is invalid";
        return View();
      }

      var jobRank = await _context.JobRankDetails
        .FirstOrDefaultAsync(jr => 
          jr.Job_Id == jobRankDetail.Job_Id &&
          jr.Rank_Id == jobRankDetail.Rank_Id
        );
      if (jobRank != null)
      {
        ViewBag.ErrBinding = $"Job: {job.Title} and Rank: {rank.Title} already are binding together.";
        return View();
      }

      ViewBag.ComBinding = "Successfully bind between Job and Rank";
      return View();
    }

    public bool rankExists(int id)
    {
      return (_context.Ranks?.Any(r => r.Id == id)).GetValueOrDefault();
    }

    public SelectList GetJobsSelectList() {
      return (new SelectList(_context.Jobs, "Id", "Title"));
    }

    public SelectList GetRanksSelectList() {      
      return (new SelectList(_context.Ranks, "Id", "Title"));
    }
  }
}