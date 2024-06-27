using AutoMapper;
using ManagerUserTaskApi.Application.Mappers.Interfaces;
using ManagerUserTaskApi.Domain.Models;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Domain.Responses;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;

namespace ManagerUserTaskApi.Application.Mappers;

public class AuthMapper : IAuthMapper
{
    private readonly IMapper _mapper;

    public AuthMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LoginUser, LoginRequest>(MemberList.Destination).ReverseMap();
            cfg.CreateMap<LoginResponse, UserLogged>(MemberList.Destination).ReverseMap();

            cfg.CreateMap<IGrouping<DateTime, UserTaskModel>, UserTaskGroupModel>()
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Key))
               .ForMember(dest => dest.UserTasks, opt => opt.MapFrom(src => src.ToList()));



        });

        _mapper = config.CreateMapper();
    }


    public LoginRequest Map(LoginUser request)
    {
        return _mapper.Map<LoginUser, LoginRequest>(request);

    }

    public UserLogged Map(LoginResponse response)
    {
        return _mapper.Map<LoginResponse, UserLogged>(response);
    }


}
