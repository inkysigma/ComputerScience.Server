using System;
using System.Collections.Generic;
using ComputerScience.Server.Grader;

public class Service
{
    protected Func<ServiceContainer, object> Constructor { get; }

    public Service(Func<ServiceContainer, object> constructor)
    {
        Constructor = constructor;
    }

    public object Run(ServiceContainer serviceMapper)
    {
        return Constructor.Invoke(serviceMapper);
    }
}