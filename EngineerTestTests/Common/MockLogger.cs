using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace EngineerTestTests.Common
{
    public class MockLogItem<T>
    {
        public EventId EventId { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public object Scope { get; set; }
        public T LogType { get; set; }
    }
    
    public class MockLogger : MockLogger<string> {}
    
    public class MockLogger<T> : ILogger<T>, IDisposable
    {
        public Dictionary<LogLevel, List<MockLogItem<T>>> Logs;
        public Stack<object> Scope;
        public T LogType;
        
        public MockLogger()
        {
            Logs = new Dictionary<LogLevel, List<MockLogItem<T>>>();
            Scope = new Stack<object>();
        }
        
        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId,
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (!Logs.ContainsKey(logLevel))
                Logs[logLevel] = new List<MockLogItem<T>>();
            
            Logs[logLevel].Add(new MockLogItem<T>()
            {
                EventId = eventId,
                Exception = exception,
                Message = formatter(state, exception),
                Scope = Scope,
                LogType = LogType
            });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            Scope.Push(state);
            return this;
        }

        public void Dispose()
        {
           // get rid of one scope 
            Scope.Pop();
        }
    }
}