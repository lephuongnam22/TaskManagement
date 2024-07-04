using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Queries.Tasks
{
    public record GetTasksQuery : IRequest<IList<TaskModel>>;

    public class GetTasksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTasksQuery, IList<TaskModel>>
    {
        public async Task<IList<TaskModel>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await unitOfWork.DatabaseContext.Tasks.ToListAsync(cancellationToken);

            return mapper.Map<IList<TaskModel>>(tasks);
        }
    }
}
