using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PartDetailsDemo.Models;
using System.Threading;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PartDetailsDemo.Repositories;

namespace PartDetailsDemo.CQRS.Commands
{
    public class DeletePartByIdCommand: IRequest<int>
    {
        public int PartId { get; set; }
       
    }
    public class DeletePartByIdCommandHandler : IRequestHandler<DeletePartByIdCommand, int>
    {
        //private readonly IConfiguration configuration;
        //private readonly ILogger<DeletePartByIdCommandHandler> seriLogger;
        private readonly IPartRepository partRepository;

        public DeletePartByIdCommandHandler(IPartRepository partRepository)
        {
            //this.configuration = configuration;
            //this.seriLogger = seriLogger;
            this.partRepository = partRepository;
        }
        public async Task<int> Handle(DeletePartByIdCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //var sql = "DELETE FROM Part WHERE PartId = @PartId";
                //using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                //{
                //    connection.Open();
                //    var result = await connection.ExecuteAsync(sql, command);
                //    return result;
                //}
                return await partRepository.DeletePart(command);
            }
            catch 
            {
                //seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
