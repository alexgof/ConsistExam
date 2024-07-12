using ConsistRestAPI.Entities;
using ConsistRestAPI.Models.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsistRestAPI.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            options.UseSqlServer("server=ALEX-PC;database=ConsistDB;trusted_connection=true;");
        }

        public DbSet<User> Users { get; set; }

        public User CreateNewUserByProcedure(UserRequest model)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@UserPassword",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.UserPassword
                        },
                        new SqlParameter() {
                            ParameterName = "@UserName",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.UserName
                        },
                        new SqlParameter() {
                            ParameterName = "@result",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Output
                        }};
            return Users.FromSqlRaw("[dbo].[sp_CreateNewUser]  @UserPassword, @UserName", param).ToList()[0];
        }
    }
}
