using AutoMapper;
using MediatR;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Commands
{
    public record UpdateTaskCommand(TaskModel TaskModel) : IRequest<TaskModel>;


    public class UpdateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateTaskCommand, TaskModel>
    {
        public async Task<TaskModel> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await unitOfWork.DatabaseContext.Tasks.FindAsync(request.TaskModel.Id);

            if(task == null)
            {
                throw new Exception("Task not found");
            }

            task.Title = request.TaskModel.Title;
            task.Description = request.TaskModel.Description;
            task.TaskStatus = request.TaskModel.TaskStatus;
            task.DueDate = DateTime.Parse(request.TaskModel.DueDate);
            task.Priority = request.TaskModel.Priority;

            unitOfWork.DatabaseContext.Tasks.Update(task);
            await unitOfWork.CommitAsync(cancellationToken);

            return mapper.Map<TaskModel>(task);
        }
    }
}
