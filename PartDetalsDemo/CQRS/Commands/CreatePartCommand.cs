using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PartDetailsDemo.Models;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PartDetailsDemo.Repositories;

namespace PartDetailsDemo.CQRS.Commands
{
    public class CreatePartCommand : IRequest<int>
    {
        public string PartName { get; set; }
        public string PartDetails { get; set; }
       
    }

    public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, int>
    {
        //private readonly IConfiguration configuration;
        //private readonly ILogger<CreatePartCommandHandler> seriLogger;
        private readonly IPartRepository partRepository;

        public CreatePartCommandHandler(IPartRepository partRepository)
        {
            //this.configuration = configuration;
            //this.seriLogger = seriLogger;
            this.partRepository = partRepository;
        }

        public async Task<int> Handle(CreatePartCommand command, CancellationToken cancellationToken)
        {
            {
                try
                {
                    //var sql = "INSERT INTO Part (PartName, PartDetails) VALUES (@PartName, @PartDetails)";
                    //using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    //{
                    //    connection.Open();
                    //    var result = await connection.ExecuteAsync(sql, command);
                    //    return result;
                    //}
                    return await partRepository.CreatePart(command);
                }
                catch
                {
                    //seriLogger.Log(LogLevel.Error, ex.Message, ex);
                    throw;
                }
            }
        }

    }
}
