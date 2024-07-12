using ConsistRestAPI.Entities;
using ConsistRestAPI.Helpers;
using ConsistRestAPI.Models.Users;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace ConsistRestAPI.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int userId);
        User CreateNewUserBySP(UserRequest model);
        User GetByFields(UserRequest model);
        bool IsUserAndPasswordExists(UserRequest model, out int userId);
        bool DeleteUser(int userId);
        string RemoveSqlInjectionSigns(string input);
        string CheckModelData(UserRequest model);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        
        public UserService(
            DataContext context)
        {
            _context = context;         
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int userId)
        {
            return getUser(userId);
        }

        public User GetByFields(UserRequest model)
        {
            return getUser(model);
        }              

        public bool IsUserAndPasswordExists(UserRequest model, out int userId)
        {
            userId = -1;
            var isUserExists = true;
            var user = _context.Users.FirstOrDefault(a => a.UserName.ToLower() == model.UserName.ToLower() &&
                                   a.UserPassword.ToLower() == model.UserPassword.ToLower());
            if (user == null)
            {
                isUserExists = false;
                return isUserExists;
            }
            userId = user.UserId;
            return isUserExists;
        }        
                               
        public bool DeleteUser(int userId)
        {
            var user = getUser(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            } 
            return user != null;
        }

        private User getUser(int userId)
        {
            return _context.Users.FirstOrDefault(a => a.UserId == userId);                     
        }

        private User getUser(UserRequest model)
        {
            return _context.Users.FirstOrDefault(a => a.UserName.ToLower() == model.UserName.ToLower());
        }

        public User CreateNewUserBySP(UserRequest model)
        {            
             return _context.CreateNewUserByProcedure(model);
        }

        public string CheckModelData( UserRequest model)
        {
            var message = string.Empty;
            if (model != null)
            {
                if (string.IsNullOrEmpty(model.UserName))
                    message = "User Name is empty";

                if (string.IsNullOrEmpty(model.UserPassword))
                    message = "User password is empty";

                if (model.UserName.Length > 50)
                    message = "UserName must be less then 50 simbols.";
                
                if (model.UserPassword.Length > 50)
                    message = "UserPassword must be less then 50 simbols.";
                          
                model.UserName = RemoveSqlInjectionSigns(model.UserName);
                model.UserPassword = RemoveSqlInjectionSigns(model.UserPassword);
            }
            else
                message = "Not sending data.";

            return message;
        }

        public string RemoveSqlInjectionSigns(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // List of common SQL injection signs to remove or replace
            string[] sqlInjectionSigns = new string[]
            {
            "--", ";", "/\\*", "\\*/", "@@", "@", "char", "nchar", "varchar", "nvarchar",
            "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop",
            "end", "exec", "execute", "fetch", "insert", "kill", "open", "select",
            "sys", "sysobjects", "syscolumns", "table", "update"
            };

            // Escape the signs
            foreach (var sign in sqlInjectionSigns)
            {
                input = Regex.Replace(input, Regex.Escape(sign), "", RegexOptions.IgnoreCase);
            }

            return input;
        }
    }
}
