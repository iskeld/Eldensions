namespace EldSharp.Eldensions.Cecil
{
    public sealed class InterfaceImplementation
    {
        public static readonly InterfaceImplementation NotImplemented = new InterfaceImplementation
        {
            IsImplemented = false
        };

        public static readonly InterfaceImplementation Implemented = new InterfaceImplementation
        {
            IsImplemented = true
        };

        public bool IsImplemented { get; private set; }
        public GenericArgumentsMap GenericArguments { get; private set; }

        private InterfaceImplementation()
        {
            GenericArguments = GenericArgumentsMap.Empty;
        }

        public InterfaceImplementation(GenericArgumentsMap genericMap)
            : this()
        {
            IsImplemented = true;
            GenericArguments = genericMap;
        }
    }
}
