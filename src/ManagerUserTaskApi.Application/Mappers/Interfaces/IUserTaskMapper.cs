namespace ManagerUserTaskApi.Application.Mappers.Interfaces;

using Domain.Models;
using Domain.Requests;
using Infrastructure.Database.Entities;

public interface IUserTaskMapper
{
    UserTask Map(CreateUserTask request);
    UserTask Map(UpdateUserTask request);
    UserTaskModel Map(UserTask entity);
    UserTaskGroupModel Map(UserTaskGroup entity);
}