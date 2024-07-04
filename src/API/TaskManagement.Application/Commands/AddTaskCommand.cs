using AutoMapper;
using MediatR;
using TaskManagement.Application.Models;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Commands
{
    public record AddTaskCommand(TaskModel TaskModel) : IRequest<TaskModel>;

    public class AddTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddTaskCommand, TaskModel>
    {
        public async Task<TaskModel> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var task = mapper.Map<TaskDb>(request.TaskModel);
            await unitOfWork.DatabaseContext.Tasks.AddAsync(task, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return mapper.Map<TaskModel>(task);
        }
    }
}
