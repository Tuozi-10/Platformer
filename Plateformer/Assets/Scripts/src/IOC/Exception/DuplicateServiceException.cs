using System;
using src.IOC.Service;

namespace src.IOC.Exception
{
    internal class DuplicateServiceException: System.Exception
    {
        internal DuplicateServiceException(Type serviceType, IService existing, IService redundant)
            :base($"Tried to register {redundant} as {serviceType.Name}, when {existing} was already registered as this service.")
        { }
    }
}