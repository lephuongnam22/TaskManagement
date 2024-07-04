using AutoMapper;
using MediatR;
using TaskManagement.Infrastructure.UnitOfWorks;

namespace TaskManagement.Application.Commands
{
    public record DeleteTaskCommand(int Id) : IRequest<bool>;

    public class DeleteTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteTaskCommand, bool>
    {
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await unitOfWork.DatabaseContext.Tasks.FindAsync(request.Id);

            if (task == null)
            {
                throw new Exception("Task not found");
            }

            unitOfWork.DatabaseContext.Tasks.Remove(task);
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
