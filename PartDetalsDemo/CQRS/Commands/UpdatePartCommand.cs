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
    public class UpdatePartCommand: IRequest<int>
    {
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string PartDetails { get; set; }
       
    }

    public class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand, int>
    {
        //private readonly IConfiguration configuration;
        //private readonly ILogger<UpdatePartCommandHandler> seriLogger;
        private readonly IPartRepository partRepository;

        public UpdatePartCommandHandler(IPartRepository partRepository)
        {
            //this.configuration = configuration;
            //this.seriLogger = seriLogger;
            this.partRepository = partRepository;
        }
        public async Task<int> Handle(UpdatePartCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //var sql = "Update Part SET PartName = @PartName, PartDetails = @PartDetails WHERE PartId=@PartId";
                //using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                //{
                //    connection.Open();
                //    var result = await connection.ExecuteAsync(sql, command);
                //    return result;
                //}
                return await partRepository.UpdatePart(command);
            }
            catch 
            {
                //seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
