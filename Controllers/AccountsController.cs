using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Doctrack.Controllers
{
  public class AccountsController : Controller
  {
    private readonly DoctrackContext _context;

    public AccountsController(DoctrackContext context)
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


      var user = new Account()
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

      _context.Accounts.Add(user);
      await _context.SaveChangesAsync();
      return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
      if (_context.Accounts == null)
      {
        return RedirectToAction("Register");
      }

      var user =  await _context.Accounts
        .FirstOrDefaultAsync(u => u.Username == username);
      if (user == null)
      {
        ViewData["username"] = username;
        ViewData["userError"] = "Username is incorrect.";
        return View();
      }
      byte[] enterPassHash;
      ReCreatePasswordHash(password, user.PasswordSalt, out enterPassHash);

      if (enterPassHash.SequenceEqual(user.PasswordHash))
      {
        HttpContext.Session.SetString("IsAuthenticated", "true");
        return RedirectToAction("Index", "Documents");
      }
      else
      {
        ViewData["username"] = username;
        ViewData["passError"] = "Password is incorrect.";
        return View();
      }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    private void ReCreatePasswordHash(string enterPass, byte[] storePassSalt, out byte[] enterPassHash)
    {
      using (var hmac = new HMACSHA512(storePassSalt))
      {
        enterPassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enterPass));
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
        ViewData["userError"] = "Please enter username.";
        result = false;
      }
      else
      {
        bool isUserMatch = Regex.IsMatch(username, $"^{patternUser}");
        if (!isUserMatch)
        {
          ViewData["userError"] = "Username is invlid and it must be 7 or 16 characters long.";
          result = false;
        }
      } // Verify username

      if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 16)
      {
        ViewData["passError"] = "8 or 16 characters long and it must be alphanumeric.";
        result = false;
      }
      else
      {
        bool isPassMatch = Regex.IsMatch(password, $"^{patternPass}");
        if (!isPassMatch)
        {
          ViewData["passError"] = "Password much contain atleast one Uppercase, Numeric and Special character.";
          result = false;
        }
      } // Verify password

      if (string.IsNullOrEmpty(confirmPassword))
      {
        ViewData["conPassError"] = "8 or 16 characters long and it must be alphanumeric.";
        result = false;
      } // Verify confirm password is null

      if (password != confirmPassword)
      {
        ViewData["conPassError"]= "The two passwords don't match.";
        result = false;
      } // Verify pattern confirm password

      if (string.IsNullOrEmpty(email))
      {
        ViewData["emailError"] = "Please enter your email";
        result = false;
      }
      else {
        bool isEmailMatch = Regex.IsMatch(email, $"^{patternEmail}");
        if (!isEmailMatch)
        {
          ViewData["emailError"] = "The email is invalid";
          result = false;
        }
      } // Verify email
      
      var user = _context.Accounts
        .FirstOrDefault(u => u.Username == username);

      if (user != null)
      {
        ViewData["userError"] = "Username is already exists.";
        result = false;
      }

      user = _context.Accounts
        .FirstOrDefault(u => u.Email == email);

      if (user != null)
      {
        ViewData["emailError"] = "Email address is already exists.";
        result = false;
      }

      return result;
    }
  }
}