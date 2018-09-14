using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Nerl
{
    public class NerlServer : IDisposable
    {
    private readonly HttpListener _listener;
    private readonly Router _router;
    private readonly List<Task> _ongoingTasks = new List<Task>();
    private bool shouldStop = false;
    private Semaphore _semaphore;

    public NerlServer(int port)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        _listener  = new HttpListener();
        _router = new Router(callingAssembly);
        _listener.Prefixes.Add($"http://localhost:{port}/");
        _semaphore = new Semaphore(Environment.ProcessorCount * 64, Environment.ProcessorCount * 64);
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Serveur listening");
        while (!shouldStop)
        {
            _semaphore.WaitOne();

            _listener.GetContextAsync().ContinueWith(async (t) => {
                _semaphore.Release();
                HttpListenerContext context = await t;
                Task routerTask = _router.Handle(context);
                _ongoingTasks.Add(routerTask);

                await routerTask;
                context.Response.Close();
            });
        }
    }

    public void Dispose()
    {
        shouldStop = true;
        Task.WaitAll(_ongoingTasks.ToArray());
        ((IDisposable)_listener).Dispose();
    }
  }
}
