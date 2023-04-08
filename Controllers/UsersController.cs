using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
      if (!InputFormIsValid(username, password, confirmPassword))
      {
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
      return RedirectToAction("Index", "Documents");
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    public bool InputFormIsValid(string username, string password, string confirmPassword)
    {
      bool result = true;
      string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])([^\s]){8,16}$";
      bool isMatch = Regex.IsMatch(password, $"^{pattern}");

      if (string.IsNullOrEmpty(username))
      {
        ViewData["ErrorUser"] = "Please enter username.";
        result = false;
      }

      if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 16)
      {
        ViewData["ErrorPass"] = "Please enter a password between 8 and 16 characters long and it must be alphanumeric.";
        result = false;
      }else if (!isMatch)
      {
        ViewData["ErrorPass"] = "Please enter a password much be contain Upercase and Lowercase (a, Z), Numeric (0-9), Special character (!, %, @, #, etc.).";
        result = false;
      }

      if (string.IsNullOrEmpty(confirmPassword))
      {
        ViewData["ErrorConPass"] = "Please enter confirm password.";
        result = false;
      }

      if (password != confirmPassword)
      {
        ViewData["ErrorConPass"]= "Passwords don't match!.";
        result = false;
      }
      return result;
    }
  }
}