using MediatR;

namespace Template.Application.Common.Cqrs;

public abstract class Query : IRequest
{
}

public abstract class Query<T> : IRequest<T>
{
}
