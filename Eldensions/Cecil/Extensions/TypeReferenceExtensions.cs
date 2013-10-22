using Mono.Cecil;

namespace EldSharp.Eldensions.Cecil.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static InterfaceImplementation GetImplementationDetails(this TypeReference typeReference,
            TypeDefinition iface)
        {
            return typeReference.GetImplementationDetails(iface, GenericArgumentsMap.Empty, false);
        }

        public static InterfaceImplementation GetImplementationDetails(this TypeReference typeReference,
            TypeDefinition iface, bool ignoreResolutionErrors)
        {
            return typeReference.GetImplementationDetails(iface, GenericArgumentsMap.Empty, ignoreResolutionErrors);
        }

        public static InterfaceImplementation GetImplementationDetails(this TypeReference typeReference,
            TypeDefinition iface, GenericArgumentsMap currentMap, bool ignoreResolutionErrors)
        {
            TypeDefinition typeDefinition;
            try
            {
                typeDefinition = typeReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                if (ignoreResolutionErrors)
                {
                    return InterfaceImplementation.NotImplemented;
                }
                throw;
            }

            if (typeReference.IsGenericInstance)
            {
                GenericInstanceType genericInstance = (GenericInstanceType) typeReference;
                GenericArgumentsMap map = GenericArgumentsMap.Build(typeDefinition.GenericParameters,
                    genericInstance.GenericArguments, currentMap);
                return typeDefinition.GetImplementationDetails(iface, map, ignoreResolutionErrors);
            }

            return typeDefinition.GetImplementationDetails(iface, ignoreResolutionErrors);
        }
    }
}