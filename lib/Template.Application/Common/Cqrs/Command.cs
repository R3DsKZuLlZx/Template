using MediatR;

namespace Template.Application.Common.Cqrs;

public abstract class Command : IRequest
{
}

public abstract class Command<T> : IRequest<T>
{
}
