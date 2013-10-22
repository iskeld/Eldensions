using Mono.Cecil;

namespace EldSharp.Eldensions.Cecil.Extensions
{
    public static class TypeDefinitionExtensions
    {
        public static InterfaceImplementation GetImplementationDetails(this TypeDefinition typeDefinition,
            TypeDefinition iface)
        {
            return typeDefinition.GetImplementationDetails(iface, GenericArgumentsMap.Empty, false);
        }

        public static InterfaceImplementation GetImplementationDetails(this TypeDefinition typeDefinition,
            TypeDefinition iface, bool ignoreResolutionErrors)
        {
            return typeDefinition.GetImplementationDetails(iface, GenericArgumentsMap.Empty, ignoreResolutionErrors);
        }

        public static InterfaceImplementation GetImplementationDetails(this TypeDefinition typeDefinition,
            TypeDefinition iface, GenericArgumentsMap map, bool ignoreResolutionErrors)
        {
            foreach (TypeReference implementedInterface in typeDefinition.Interfaces)
            {
                TypeDefinition interfaceDefinition;
                try
                {
                    interfaceDefinition = implementedInterface.Resolve();
                }
                catch (AssemblyResolutionException)
                {
                    if (ignoreResolutionErrors)
                    {
                        continue;
                    }
                    throw;
                }

                if (interfaceDefinition == iface)
                {
                    if (!implementedInterface.IsGenericInstance)
                    {
                        return InterfaceImplementation.Implemented;
                    }

                    GenericInstanceType genericInterface = (GenericInstanceType) implementedInterface;

                    return new InterfaceImplementation(
                        GenericArgumentsMap.Build(interfaceDefinition.GenericParameters,
                            genericInterface.GenericArguments, map));
                }
            }

            TypeReference baseType = typeDefinition.BaseType;
            if (baseType == null)
            {
                return InterfaceImplementation.NotImplemented;
            }

            InterfaceImplementation result = baseType.GetImplementationDetails(iface, ignoreResolutionErrors);
            return result;
        }
    }
}