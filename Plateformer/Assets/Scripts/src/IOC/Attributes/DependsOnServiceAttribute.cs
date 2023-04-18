using System;

namespace src.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DependsOnServiceAttribute : Attribute
    {}
}