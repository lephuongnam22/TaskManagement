using Microsoft.AspNetCore.Mvc;
using System.Text;
using MediatR;

namespace TaskManagement.WebApi.Controllers
{
    public class BaseController(IMediator mediator) : ControllerBase
    {
        protected virtual IActionResult HandleProblemReturn(Exception ex, string methodName)
        {
            var errMsg = ExceptionHelper(ex);
            HttpContext.Response.StatusCode = 500;
            return Problem(errMsg, methodName, 500, "An error occurred while processing your request", "Error");
        }

        private string ExceptionHelper(Exception e)
        {
            StringBuilder errMsg = new StringBuilder();

            while (e != null)
            {
                errMsg.Append(e.Message + " ");
                e = e.InnerException;
            }

            return errMsg.ToString();
        }
    }
}
