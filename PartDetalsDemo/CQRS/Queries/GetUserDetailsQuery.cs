using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PartDetailsDemo.Models;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace PartDetailsDemo.CQRS.Queries
{
    public class GetUserDetailsQuery : IRequest<User>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, User>
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<GetUserDetailsQueryHandler> seriLogger;

        public GetUserDetailsQueryHandler(IConfiguration configuration, ILogger<GetUserDetailsQueryHandler> seriLogger)
        {
            this.configuration = configuration;
            this.seriLogger = seriLogger;
        }
        public async Task<User> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
        {            
            var sql = "Select * from [User] where UserId = @UserId and Password = @Password";

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<User>(sql, query);
                    return result.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
