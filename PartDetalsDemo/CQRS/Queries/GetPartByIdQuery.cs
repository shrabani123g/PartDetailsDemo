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
    public class GetPartByIdQuery : IRequest<Part>
    {
        public int Id { get; set; }
    }

    public class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, Part>
    {       
        private readonly IPartRepository partRepository;

        public GetPartByIdQueryHandler(IPartRepository partRepository)
        {            
            this.partRepository = partRepository;
        }
        public async Task<Part> Handle(GetPartByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {                
                return await partRepository.GetPartById(query);
            }
            catch 
            {
                throw;
            }
        }
    }

}
