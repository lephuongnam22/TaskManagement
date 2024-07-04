using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Queries.Tasks
{
    public record SearchTaskQuery(SearchTaskRquest SearchTaskRequest) : IRequest<List<TaskModel>>;

    public class SearchTaskQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : 
        IRequestHandler<SearchTaskQuery, List<TaskModel>>
    {
        public async Task<List<TaskModel>> Handle(SearchTaskQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = unitOfWork.DatabaseContext.Tasks.AsQueryable();

            if(request.SearchTaskRequest.TaskStatus.HasValue)
            {
                baseQuery = baseQuery.Where(n => n.TaskStatus == request.SearchTaskRequest.TaskStatus.Value);
            }

            if(request.SearchTaskRequest.Priority.HasValue)
            {
                baseQuery = baseQuery.Where(n => n.Priority == request.SearchTaskRequest.Priority.Value);
            }

            var results = await baseQuery.ToListAsync(cancellationToken);

            return mapper.Map<List<TaskModel>>(results);
        }
    }
}
