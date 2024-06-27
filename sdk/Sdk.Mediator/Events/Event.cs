namespace Sdk.Mediator.Events;

using Abstractions.Events;
using Abstractions.Records;

public class Event<TModel> : BaseEvent
    where TModel : class
{
    public Event(TModel model, UserData user)
    {
        Model = model;
        User = user;
    }

    public TModel Model { get; set; }

    public UserData User { get; set; }
}