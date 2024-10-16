using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Mediator;

/// <summary>
/// Defines a scoped mediator to encapsulate request/response and publishing interaction patterns
/// </summary>
internal class ScopedMediator(IServiceScopeFactory scopeFactory) : IScopedMediator
{
    /// <inheritdoc/>
    public async IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var items = mediator.CreateStream(request, cancellationToken);

            await foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<object?> CreateStream(object request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var items = mediator.CreateStream(request, cancellationToken);

            await foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    /// <inheritdoc/>
    public async Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(notification, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(notification, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var response = await mediator.Send(request, cancellationToken);

            return response;
        }
    }

    /// <inheritdoc/>
    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(request, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        var scope = scopeFactory.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var response = await mediator.Send(request, cancellationToken);

            return response;
        }
    }
}
