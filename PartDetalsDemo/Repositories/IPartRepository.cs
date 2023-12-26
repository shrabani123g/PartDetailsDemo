using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PartDetailsDemo.CQRS.Commands;
using PartDetailsDemo.CQRS.Queries;
using PartDetailsDemo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PartDetailsDemo.Repositories
{
    public interface IPartRepository
    {
        public Task<IEnumerable<Part>> GetAllParts();
        public Task<Part> GetPartById(GetPartByIdQuery query);
        public Task<int> CreatePart(CreatePartCommand command);
        public Task<int> UpdatePart(UpdatePartCommand command);
        public Task<int> DeletePart(DeletePartByIdCommand command);
    }

    public class PartRepository : IPartRepository
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PartRepository> seriLogger;
        private readonly IDbConnection dbConnection;

        public PartRepository(IConfiguration configuration, ILogger<PartRepository> seriLogger)
        {
            this.configuration = configuration;
            this.seriLogger = seriLogger;
            dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Part>> GetAllParts()
        {
            try
            {
                seriLogger.Log(LogLevel.Information, "GetAllParts Started");
                var sql = "Select * from Part";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<Part>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
        public async Task<Part> GetPartById(GetPartByIdQuery query)
        {
            try
            {
                seriLogger.Log(LogLevel.Information, "GetPartById Started");
                var sql = "Select * from Part where PartId = @Id";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<Part>(sql, query);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
        public async Task<int> CreatePart(CreatePartCommand command)
        {
            try
            {
                seriLogger.Log(LogLevel.Information, "CreatePart Started");
                var sql = "INSERT INTO Part (PartName, PartDetails) VALUES (@PartName, @PartDetails)";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(sql, command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
        public async Task<int> UpdatePart(UpdatePartCommand command)
        {
            try
            {
                seriLogger.Log(LogLevel.Information, "UpdatePart Started");
                var sql = "Update Part SET PartName = @PartName, PartDetails = @PartDetails WHERE PartId=@PartId";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(sql, command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
        public async Task<int> DeletePart(DeletePartByIdCommand command)
        {
            try
            {
                seriLogger.Log(LogLevel.Information, "DeletePart Started");
                var sql = "DELETE FROM Part WHERE PartId = @PartId";
                using (dbConnection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(sql, command);
                    return result;
                }
            }
            catch (Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                throw;
            }
        }
    }

}
