using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Moq;
using Sdk.Db.Abstractions.Pagination;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.ListUserTaskGroupHandlerTests;

[Collection(nameof(ListUserTaskGroupHandlerCollection))]
public class ListUserTaskGroupHandlerTest
{
    private ListUserTaskGroupHandler _handler;
    private readonly ListUserTaskGroupHandlerFixture _fixture;

    public ListUserTaskGroupHandlerTest(ListUserTaskGroupHandlerFixture fixture)
    {
        _fixture = fixture;
        _handler = _fixture.GetListUserTaskGroup();
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task ListUserTaskGroupHandler_Handle_Success(bool isThereRecords)
    {
        // Arrange
        var request = _fixture.GenerateListUserTaskGroupRequest();
        var paginatedList = _fixture.GeneratePaginatedListUserTaskGroupEntity(3);
        if (!isThereRecords) paginatedList.Results = null;

        var model = _fixture.GenerateUserTaskModel();


        _fixture.IUserTaskRepositoryMock
           .Setup(m => m.ListUpcomingTasksGroupedByDatePagedAsync(
               It.IsAny<Order>(),
               It.IsAny<Page>(),
               It.IsAny<Expression<Func<UserTask, bool>>>(),
               It.IsAny<Expression<Func<UserTask, object>>[]>())
           )
           .ReturnsAsync(paginatedList!);

        // Act
        var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _fixture.IUserTaskRepositoryMock
            .Verify(m => m.ListUpcomingTasksGroupedByDatePagedAsync(
                It.IsAny<Order>(),
                It.IsAny<Page>(),
                It.IsAny<Expression<Func<UserTask, bool>>>(),
                It.IsAny<Expression<Func<UserTask, object>>[]>())
            , Times.Once);


    }
}
