using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using TaskManagement.Application.Commands;
using TaskManagement.Application.Models;
using TaskManagement.Application.Queries.Tasks;
using TaskManagement.Domain.Entities;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace TaskManagement.WebApi.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var result = await mediator.Send(new GetTasksQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "GetTasks");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var result = await mediator.Send(new GetTaskByIdQuery(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "GetTaskById");
            }
        }

        [HttpGet("task-status")]
        public IActionResult GetTaskStatus()
        {
            try
            {
                var result = Enum.GetNames(typeof(TaskStatusDb)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "GetTaskStatus");
            }
        }

        [HttpGet("task-priority")]
        public IActionResult GetTaskPriority()
        {
            try
            {
                var result = Enum.GetNames(typeof(Priority)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "GetTaskPriority");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel taskModel)
        {
            try
            {
                var result = await mediator.Send(new AddTaskCommand(taskModel));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "AddTask");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskModel taskModel)
        {
            try
            {
                var result = await mediator.Send(new UpdateTaskCommand(taskModel));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "UpdateTask");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await mediator.Send(new DeleteTaskCommand(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "DeleteTask");
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTask(SearchTaskRquest searchTaskRquest)
        {
            try
            {
                var result = await mediator.Send(new SearchTaskQuery(searchTaskRquest));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleProblemReturn(ex, "SearchTask");
            }
        }
    }
}