using System.Threading.Tasks;

namespace ZooManagement.Application.Interfaces; 

public interface IEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class;
}
