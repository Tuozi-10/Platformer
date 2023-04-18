using System;

namespace src.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ServiceInitAttribute: Attribute
    {}
}