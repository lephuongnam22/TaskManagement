using AutoMapper;
using TaskManagement.Application.Models;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappers
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskModel, TaskDb>().ReverseMap();
        }
    }
}
