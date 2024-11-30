using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();

    public static void RegisterService<T>(T service)
    {
        services[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        return (T)services[typeof(T)];
    }
}