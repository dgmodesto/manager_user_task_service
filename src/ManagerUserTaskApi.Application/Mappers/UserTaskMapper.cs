namespace ManagerUserTaskApi.Application.Mappers;

using AutoMapper;
using Domain.Models;
using Domain.Requests;
using Infrastructure.Database.Entities;
using Interfaces;

public class UserTaskMapper : IUserTaskMapper
{
    private readonly IMapper _mapper;

    public UserTaskMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserTask, UserTask>(MemberList.Destination);

            cfg.CreateMap<UpdateUserTask, UserTask>(MemberList.Destination)
                .AfterMap((request, client) => { client.SetVersion(request.Version); });

            cfg.CreateMap<UserTaskModel, UserTask>().ReverseMap()
                .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));
        });

        _mapper = config.CreateMapper();
    }

    public UserTask Map(CreateUserTask request)
    {
        return _mapper.Map<CreateUserTask, UserTask>(request);
    }

    public UserTask Map(UpdateUserTask request)
    {
        return _mapper.Map<UpdateUserTask, UserTask>(request);
    }

    public UserTaskModel Map(UserTask entity)
    {
        return _mapper.Map<UserTask, UserTaskModel>(entity);
    }

    public UserTaskGroupModel Map(UserTaskGroup grouping)
    {
        return _mapper.Map<UserTaskGroupModel>(grouping);
    }
}