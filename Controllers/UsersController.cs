using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using System.Security.Cryptography;
using System.Text;

namespace Doctrack.Controllers
{
  public class UsersController : Controller
  {
    private readonly DoctrackContext _context;
    // private byte[] passwordSalt;

    public UsersController(DoctrackContext context)
    {
      _context = context;
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password, string confirmPassword)
    {
      ViewData.Clear();
      if (string.IsNullOrEmpty(username))
      {
        ViewData["ErrorUser"] = "Please enter username.";
      }

      if (string.IsNullOrEmpty(password))
      {
        ViewData["ErrorPass"] = "Please enter password.";
      }

      if (string.IsNullOrEmpty(confirmPassword))
      {
        ViewData["ErrorConPass"] = "Please enter confirm password.";
      }

      if (password != confirmPassword)
      {
        ViewData["ErrorConPass"]= "Confirm password doesn't match password.";
      }
      Console.WriteLine("real: "+ ViewData == null);
      if (ViewData != null)
      {
        Console.WriteLine("null");
        ViewData["username"] = username;
        return View();
      }

      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);

      // var user = await _context.Users
      //   .FirstOrDefaultAsync(u => u.Username == username);
      
      // if (user == null)
      // {

      // }

      // user.PasswordHash = passwordHash;
      // user.PasswordSalt = passwordSalt;

      Console.WriteLine($"Password: {password}");
Console.WriteLine($"Password Hash: {Convert.ToBase64String(passwordHash)}");
Console.WriteLine($"Password Salt: {Convert.ToBase64String(passwordSalt)}");
      // _context.Users.Add(user);
      // await _context.SaveChangesAsync();
      return RedirectToAction(nameof(DocumentsController.Index));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }
  }
}