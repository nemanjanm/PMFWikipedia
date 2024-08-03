using PMFWikipedia.Models;
using System.Collections.Concurrent;

namespace PMFWikipedia.SignalR
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connection = new();

        public ConcurrentDictionary<string, UserConnection> connections => _connection;
    }
}