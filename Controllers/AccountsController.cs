using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctrack.Data;
using Doctrack.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Doctrack.Authentication;
using Doctrack.SendGrid;
using Doctrack.Method;

namespace Doctrack.Controllers
{
  public class AccountsController : Controller
  {
    private readonly DoctrackContext _context;

    public AccountsController(DoctrackContext context)
    {
      _context = context;
    }

    [AuthenticationPrivilege]
    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Register(string username, string password, string confirmPassword, string email)
    {
      if (!RegisterFormIsValid(username, password, confirmPassword, email))
      {
        ViewData["username"] = username;
        ViewData["email"] = email;
        return View();
      }

      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);

      byte[] keyToken;
      var token = VerificationTokenGenerator.GenerateEmailVerificationToken(out keyToken);

      await EmailService.SendVerificationEmailAsync(email, token);

      var user = new Account()
      {
        Username = username,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt,
        Email = email,
        Role_Id = 2,
        IsEmailConfirm = false,
        IsApproved = false,
        Token = token,
        KeyToken = keyToken
      };

      _context.Accounts.Add(user);
      await _context.SaveChangesAsync();
      return RedirectToAction("Login");
    }

    public async Task<IActionResult> VerifyEmail(string token)
    {
      if (_context.Accounts == null)
      {
        return NotFound();
      }

      var user = await _context.Accounts
        .FirstOrDefaultAsync(u => u.Token == token);

      if (VerificationTokenGenerator.ValidateToken(token, user.KeyToken)){
        user.IsEmailConfirm = true;
        _context.Accounts.Update(user);
        await _context.SaveChangesAsync();
      }
      else
      {
        Console.WriteLine("Error more than 24 hr");
      }
      return RedirectToAction("Login");
    }

    [AuthenticationPrivilege]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [AuthenticationPrivilege]
    public async Task<IActionResult> Login(string? username, string? password)
    {
      bool flagError = false;
      
      if (_context.Accounts == null)
      {
        return RedirectToAction("Register");
      }

      if (username == null)
      {
        flagError = true;
        ViewData["userError"] = "Please enter username.";
      }

      ViewData["username"] = username;
      
      if (password == null)
      {
        flagError = true;
        ViewData["passError"] = "Please enter password.";
      }
      
      if (flagError)
      {
        return View();
      }


      var user =  await _context.Accounts
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.Username == username);
      if (user == null)
      {
        ViewData["userError"] = "Username is incorrect.";
        return View();
      }
      
      byte[] enterPassHash;
      ReCreatePasswordHash(password, user.PasswordSalt, out  enterPassHash);

      if (enterPassHash.SequenceEqual(user.PasswordHash) && user.IsApproved && user.IsEmailConfirm)
      {
        HttpContext.Session.SetString("IsAuthenticated", "true");
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role", user.Role.Title);
        return RedirectToAction("Index", "Documents");
      }
      else
      {
        ViewData["passError"] = "Password is incorrect.";
        return View();
      }
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public IActionResult Logout()
    {
      HttpContext.Session.Clear();
      return RedirectToAction("Index", "Documents");
    }

    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Management(string? queryStr, bool? isSearch)
    {
      if (_context.Accounts == null)
      {
        return NotFound();
      }

      var users = await _context.Accounts
        .Include(acc => acc.Role)
        .Where(acc => string.IsNullOrEmpty(queryStr)
          || acc.Username.Contains(queryStr)
        )
        .ToListAsync();

      if(isSearch ?? false) 
      {
        return PartialView("_ManagementTable", users);
      }

      return View(users);
    }

    public IActionResult ForgetPassword()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(string username)
    {
      ViewData["username"] = username;

      var user = _context.Accounts
          .FirstOrDefault(u => u.Username == username);
      if (user == null)
      {
        ViewData["userError"] = "Username is incorrect.";
        return View();
      }

      byte[] keyToken;
      var token = VerificationTokenGenerator.GenerateEmailVerificationToken(out keyToken);

      await EmailService.SendConfirmResetAsync(user.Username, user.Email, token);
      user.Token = token;
      user.KeyToken = keyToken;

      ViewData["userError"] = "Please confirm reset password in email.";

      _context.Accounts.Update(user);
      await _context.SaveChangesAsync();
      return View();
    }

    public async Task<IActionResult> ResetPassword(string username, string token)
    {
      if (_context.Accounts == null) return NotFound();

      var user = await _context.Accounts
        .FirstOrDefaultAsync(ac => ac.Username == username);

      if (!VerificationTokenGenerator.ValidateToken(token, user.KeyToken))
      {
        return NotFound();
      }

      ViewData["username"] = username;
      ViewData["token"] = token;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string username,string token, string password, string confirmPassword)
    {
      var user = _context.Accounts
          .FirstOrDefault(u => u.Username == username);
      if (user == null)
      {
        ViewData["userError"] = "Username is incorrect.";
        return View();
      }

      if (!ForgetFormIsValid(username, password, confirmPassword))
      {
        return View();
      }

      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);

      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;

      _context.Accounts.Update(user);
      await _context.SaveChangesAsync();
      return RedirectToAction("Login");
    }

    // [HttpPost]
    // [AuthenticationPrivilege]
    // public async Task<IActionResult> ForgetPassword(string username, string newPassword, string confirmPassword)
    // {
    //   ViewData["username"] = username;

    //   var user = _context.Accounts
    //       .FirstOrDefault(u => u.Username == username);
    //   if (user == null)
    //   {
    //     ViewData["userError"] = "Username is incorrect.";
    //     return View();
    //   }

    //   if (!ForgetFormIsValid(username, newPassword, confirmPassword))
    //   {
    //     return View();
    //   }

    //   byte[] passwordHash, passwordSalt;
    //   CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

    //   user.PasswordHash = passwordHash;
    //   user.PasswordSalt = passwordSalt;

    //   _context.Accounts.Update(user);
    //   await _context.SaveChangesAsync();
    //   return RedirectToAction("Login");
    // }

    [HttpPost]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Approval(string id)
    {
      if (_context.Accounts == null)
      {
        return NotFound();
      }

      var users = await _context.Accounts.ToListAsync();
      if (users == null)
      {
        return NotFound();
      }

      var user = users.FirstOrDefault(acc => acc.Username == id);
      if (user == null)
      {
        return NotFound();
      }

      user.IsApproved = true;
      _context.Accounts.Update(user);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthenticationPrivilege]
    [AuthenticationProtect]
    public async Task<IActionResult> Delete(string id)
    {
      if (_context.Accounts == null || id == null)
      {
        return NotFound();
      }

      var users = await _context.Accounts.ToListAsync();
      if (users == null)
      {
        return NotFound();
      }

      var user = users.FirstOrDefault(acc => acc.Username == id);
      if (user == null)
      {
        return NotFound();
      }

      _context.Accounts.Remove(user);
      await _context.SaveChangesAsync();
      return Json(new { success = true });
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

    public bool RegisterFormIsValid(string username, string password, string confirmPassword, string email)
    {
      bool result = true;
      string patternUser = @"^[\w\d]{7,16}$";
      // string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])([^\s]){8,16}$";
      string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)([^\s]){8,16}$";
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

    public bool ForgetFormIsValid(string username, string password, string confirmPassword)
    {
      bool result = true;
      // string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z])([^\s]){8,16}$";
      string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)([^\s]){8,16}$";
      if (string.IsNullOrEmpty(username))
      {
        ViewData["userError"] = "Please enter username.";
        result = false;
      }

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
      
      return result;
    }
  }
}