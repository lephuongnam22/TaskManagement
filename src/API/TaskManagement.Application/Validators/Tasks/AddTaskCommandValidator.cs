using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TaskManagement.Application.Commands;

namespace TaskManagement.Application.Validators.Tasks
{
    public class AddTaskCommandValidator : AbstractValidator<AddTaskCommand>
    {
        public AddTaskCommandValidator()
        {
            RuleFor(x => x.TaskModel.Title).NotEmpty()
                .WithMessage("Title cannot be null or empty")
                .MaximumLength(100)
                .WithMessage("Max length of title is 100");
            RuleFor(x => x.TaskModel.Description).NotEmpty()
                .WithMessage("Description cannot be null or empty")
                .MaximumLength(500)
                .WithMessage("Max length of Description is 500");
        }
    }
}
