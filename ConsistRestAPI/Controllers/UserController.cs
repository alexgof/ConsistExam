using ConsistRestApi.Logger;
using ConsistRestAPI.Models.Users;
using ConsistRestAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using ConsistRestAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using ConsistRestAPI.Entities;

namespace ConsistRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly IMapper _mapper;
        private readonly string StatusError = "Error";
        private readonly string StatusOk = "Ok";
       
        public UserController(IUserService userService,            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }                            
        
        [HttpDelete("{userId}")]
        public JsonResponse DeleteUser(int userId)
        {
            var response = new JsonResponse() { Status = StatusOk, Message = "User deleted" };
            if (userId > 0)
            {
                if (!_userService.DeleteUser(userId))
                {
                    response.Status = StatusError;
                    response.Message = "UserId not found";
                }
            }
            else
            {
                response.Status = StatusError;
                response.Message = "Wrong userId";
            }
            return response;

        }
                             
        [Route("CreateUser")]
        [HttpPost]
        public JsonResponse CreateUser(UserRequest model)
        {            
            string myName = NlogLogger.InitMethodName();
            NlogLogger.Log.Info($"{myName} started...");

            var response = new JsonResponse() { Status = StatusOk, Message = "User created successfully" };
            try
            {
                var errorMessage = _userService.CheckModelData(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.Status = StatusError;
                    response.Message = errorMessage;
                    return response;
                }

                var user = _userService.GetByFields(model);
                if (user != null)
                {
                    response.Status = StatusError;
                    response.Message = "User already exists";
                }
                else
                {
                    if (_userService.CreateNewUserBySP(model) == null)
                    {
                        response.Status = StatusError;
                        response.Message = "User with the UserName '" + model.UserName + "' already exists";
                    }
                }               
            }
            catch (Exception ex)
            {
                response.Status = StatusError;
                response.Message = "User already exists";
                NlogLogger.Log.Error($"{myName} ex.Message: {ex.Message}.");
            }
            return response;
        }
               
        [Route("ValidateUser")]
        [HttpPost]
        public JsonResponse ValidateUser(UserRequest model)
        {
            string myName = NlogLogger.InitMethodName();
            NlogLogger.Log.Info($"{myName} started...");

            var response = new JsonResponse() { Status = StatusError, Message = $"User: {model.UserName} not found or wrong password" };
            try
            {
                var errorMessage = _userService.CheckModelData(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.Status = StatusError;
                    response.Message = errorMessage;
                    return response;
                }

                var userId = -1;
                if (_userService.IsUserAndPasswordExists(model,out userId))
                {
                    response.Status = StatusOk;
                    response. Message = $"User: {model.UserName} with UserID: {userId} was found and validated.";
                }                                         
            }
            catch (Exception ex)
            {
                response.Status = StatusError;
                response.Message = $"User with name: {model.UserName} not found or wrong password";
                NlogLogger.Log.Error($"{myName} ex.Message: {ex.Message}.");
            }
            return response;
        }                
    }
}
