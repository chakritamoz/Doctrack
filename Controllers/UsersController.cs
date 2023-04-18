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
    public async Task<IActionResult> Register(string username, string password, string confirmPassword, string email)
    {
      if (!InputFormIsValid(username, password, confirmPassword, email))
      {
        ViewData["username"] = username;
        ViewData["email"] = email;
        return View();
      }

      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);

      var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == username);

      // Add new user to DB
      if (user != null)
      {
        ViewData["ErrorUser"] = "Username is already exists.";
        ViewData["username"] = username;
        ViewData["email"] = email;
        return View();
      }

      user = new User()
      {
        Username = username,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt,
        Email = email,
        Role_Id = 2,
        IsEmailConfirm = false,
        IsApproved = false
      };
      
      // Set password hash and salt
      // user.PasswordHash = passwordHash;
      // user.PasswordSalt = passwordSalt;

      // Console.WriteLine($"Password: {password}");
      // Console.WriteLine($"Password Hash: {Convert.ToBase64String(passwordHash)}");
      // Console.WriteLine($"Password Salt: {Convert.ToBase64String(passwordSalt)}");

      _context.Users.Add(user);
      await _context.SaveChangesAsync();
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

    public bool InputFormIsValid(string username, string password, string confirmPassword, string email)
    {
      bool result = true;
      string patternUser = @"^[\w\d]{7,16}$";
      string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])([^\s]){8,16}$";
      string patternEmail = @"^[\w\.-_]+@[\w]+.[\w]+";

      if (string.IsNullOrEmpty(username))
      {
        ViewData["ErrorUser"] = "Please enter username.";
        result = false;
      }
      else
      {
        bool isUserMatch = Regex.IsMatch(username, $"^{patternUser}");
        if (!isUserMatch)
        {
          ViewData["ErrorUser"] = "Username is invlid and it must be 7 or 16 characters long.";
          result = false;
        }
      } // Verify username

      if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 16)
      {
        ViewData["ErrorPass"] = "8 or 16 characters long and it must be alphanumeric.";
        result = false;
      }
      else
      {
        bool isPassMatch = Regex.IsMatch(password, $"^{patternPass}");
        if (!isPassMatch)
        {
          ViewData["ErrorPass"] = "Password much contain atleast one Uppercase, Numeric and Special character.";
          result = false;
        }
      } // Verify password

      if (string.IsNullOrEmpty(confirmPassword))
      {
        ViewData["ErrorConPass"] = "8 or 16 characters long and it must be alphanumeric.";
        result = false;
      } // Verify confirm password is null

      if (password != confirmPassword)
      {
        ViewData["ErrorConPass"]= "The two passwords don't match.";
        result = false;
      } // Verify pattern confirm password

      if (string.IsNullOrEmpty(email))
      {
        ViewData["ErrorEmail"] = "Please enter your email";
      }
      else {
        bool isEmailMatch = Regex.IsMatch(email, $"^{patternEmail}");
        if (!isEmailMatch)
        {
          ViewData["ErrorEmail"] = "The email is invalid";
        }
      } // Verify email
      
      return result;
    }
  }
}