using System;
using System.Reflection;

namespace src.IOC.Exception
{
    internal class NotAServiceException : System.Exception
    {
        internal NotAServiceException(object dependant, FieldInfo fieldInfo, Type type) :
            base($"Field {fieldInfo.Name} of {type.Name} is marked with [DependsOnService], but has type {fieldInfo.FieldType.Name} instead of an interface derived from IService")
        {}
    }
}