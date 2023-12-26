using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PartDetailsDemo.Models;
using System.Threading;
//using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PartDetailsDemo.Repositories;

namespace PartDetailsDemo.CQRS.Queries
{
    public class GetAllPartQuery : IRequest<IEnumerable<Part>>
    {
       
    }

    public class GetAllPartQueryHandler : IRequestHandler<GetAllPartQuery, IEnumerable<Part>>
    {
        //private readonly IConfiguration configuration;
        //private readonly ILogger<GetAllPartQueryHandler> seriLogger;
        private readonly IPartRepository partRepository;

        public GetAllPartQueryHandler(IPartRepository partRepository)
        {
            //this.configuration = configuration;
            //this.seriLogger = seriLogger;
            this.partRepository = partRepository;
        }
        public async Task<IEnumerable<Part>> Handle(GetAllPartQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //var sql = "Select * from Part";
                //using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                //{
                //    connection.Open();
                //    var result = await connection.QueryAsync<Part>(sql);
                //    return result;
                //}
                return await partRepository.GetAllParts();
            }
            catch 
            {
                //seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
