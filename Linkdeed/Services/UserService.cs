using Linkdeed.Data;
using Linkdeed.Helper;
using Linkdeed.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Linkdeed.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);

        User AdminCreate(User user, string password);

        void AddUserDesc(int id);

        void AddEmpDesc(int id);
        void Update(User user, string currentPassword, string password, string confirmPassword);
        void Delete(int id);

        string ForgotPassword(string username);
    }
    public class UserService : IUserService
    {
        private Context _context;
        private readonly IEmailService _emailService;

        public UserService(Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.User.FirstOrDefault(x => x.Username == username) ?? null;

            // check if username exists
            if (user == null)
            {
                return null;
            }

            // Granting access if the hashed password in the database matches with the password(hashed in computeHash method) entered by user.
            if (computeHash(password) != user.PasswordHash)
            {
                return null;
            }
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.User.Any(x => x.Username == user.Username))
            {
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            if(user.AccesLevel != "User" && user.AccesLevel != "Employer")
                throw new AppException("Invalid AccesLevel, please try Employer or User");

            //Saving hashed password into Database table
            user.PasswordHash = computeHash(password);

             _context.User.Add(user);
             _context.SaveChanges();

          

            if (user.AccesLevel == "Employer")
            {
                AddEmpDesc(user.Id);
            }
            else if (user.AccesLevel == "User")
            {
                AddUserDesc(user.Id);
            }

            return user;
        }

        public User AdminCreate(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.User.Any(x => x.Username == user.Username))
            {
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            //Saving hashed password into Database table
            user.PasswordHash = computeHash(password);

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void AddUserDesc(int id)
        {
            var desc = new UserDescription
            {
                Description = "",
                UserId = id
            };
             _context.UserDescription.Add(desc);
             _context.SaveChanges();
        }

        public void AddEmpDesc(int id)
        {
            var desc = new EmployerDescription
            {
                Description = "",
                UserId = id,
                IsPremium = 0
            };
             _context.EmployerDescription.Add(desc);
             _context.SaveChanges();
        }

        public void Update(User userParam, string currentPassword = null, string password = null, string confirmPassword = null)
        {
            //Find the user by Id
            var user = _context.User.Find(userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException("Username " + userParam.Username + " is already taken");
                }
                else
                {
                    user.Username = userParam.Username;
                }
            }
            // Add new field of this project !
            //if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            //{
            //    user.FirstName = userParam.FirstName;
            //}
            //if (!string.IsNullOrWhiteSpace(userParam.LastName))
            //{
            //    user.LastName = userParam.LastName;
            //}
            if (!string.IsNullOrWhiteSpace(currentPassword))
            {
                if (computeHash(currentPassword) != user.PasswordHash)
                {
                    throw new AppException("Invalid Current password!");
                }

                if (currentPassword == password)
                {
                    throw new AppException("Please choose another password!");
                }

                if (password != confirmPassword)
                {
                    throw new AppException("Password doesn't match!");
                }

                //Updating hashed password into Database table
                user.PasswordHash = computeHash(password);
            }

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        public string ForgotPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new AppException("Valid Username is required");
            }
            else
            {
                var user = _context.User.SingleOrDefault(x => x.Username == username);
                if (user != null)
                {
                    string password = GenerateRandomCryptographicKey(5);
                    user.PasswordHash = computeHash(password);
                    _context.SaveChanges();

                    var emailAddress = new List<string>() { user.AddressMail};
                    var emailSubject = "Password Recovery";
                    var messageBody = password;

                    var response = _emailService.SendEmailAsync(emailAddress, emailSubject, messageBody);

                    if (response.IsCompletedSuccessfully)
                    {
                        return new string("If your account exists, your new password will be emailed to you shortly (success)");
                    }
                }
                return new string("If your account exists, your new password will be emailed to you shortly");
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach (var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2");
            }
            return hashstring;
        }

        

        // helper method to generate random password
        private static string GenerateRandomCryptographicKey(int keyLength)
        {
            char[] SPECIAL_CHARACTERS = @"!#$%&*@\".ToCharArray();
            char[] UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Random rand = new Random();
            int randomSpecialCharNumber = rand.Next(0, SPECIAL_CHARACTERS.Length - 1);
            int randomUppercasChars = rand.Next(0, UPPERCASE_CHARACTERS.Length - 1);
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            string hashstring = "";
            foreach (var hashbyte in randomBytes)
            {
                hashstring += hashbyte.ToString("x2");
            }
            return UPPERCASE_CHARACTERS[randomUppercasChars] + hashstring + SPECIAL_CHARACTERS[randomSpecialCharNumber];
        }
    }
}
