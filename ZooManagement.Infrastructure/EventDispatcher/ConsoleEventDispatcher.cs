using System.Text.Json;
using ZooManagement.Application.Interfaces;

namespace ZooManagement.Infrastructure.EventDispatching;

public class ConsoleEventDispatcher : IEventDispatcher
{
    public Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class
    {
        if (@event == null)
        {
            Console.WriteLine("[Infrastructure Event Dispatcher] Received a null event.");
            return Task.CompletedTask;
        }

        string eventTypeName = @event.GetType().Name;
        string serializedEvent = "Could not serialize event.";

        try
        {
            serializedEvent = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }
        catch (Exception ex)
        {
            serializedEvent = $"Error serializing event: {ex.Message}";
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[Infrastructure Event Dispatcher] Dispatched Event: {eventTypeName}");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine(serializedEvent);
        Console.WriteLine("--------------------------------------------------");
        Console.ResetColor();

        return Task.CompletedTask;
    }
}
