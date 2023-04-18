namespace src.Editor
{
    internal class DependencyAssignerException : System.Exception
    {
        internal DependencyAssignerException(string path) :
            base($"Failed to assign dependency at path {path}")
        {}
    }
}