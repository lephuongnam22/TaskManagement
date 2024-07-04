using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Queries.Tasks
{
    public record GetTaskByIdQuery (int Id) : IRequest<TaskModel>;

    public class GetTaskByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTaskByIdQuery, TaskModel>
    {
        public async Task<TaskModel> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await unitOfWork.DatabaseContext.Tasks.FirstOrDefaultAsync(n => n.Id == request.Id,
                cancellationToken);

            return mapper.Map<TaskModel>(task);
        }
    }
}
