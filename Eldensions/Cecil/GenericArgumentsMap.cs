using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace EldSharp.Eldensions.Cecil
{
    public sealed class GenericArgumentsMap : IEnumerable<KeyValuePair<string, TypeReference>>
    {
        private readonly List<KeyValuePair<GenericParameter, TypeReference>> _map;

        public static readonly GenericArgumentsMap Empty = new GenericArgumentsMap(null);

        public bool IsEmpty
        {
            get { return _map.Count == 0; }
        }

        public int Count
        {
            get { return _map.Count; }
        }

        public TypeReference this[GenericParameter parameter]
        {
            get { return _map.FirstOrDefault(kvp => kvp.Key == parameter).Value; }
        }

        public TypeReference this[string name]
        {
            get { return _map.FirstOrDefault(kvp => kvp.Key.Name == name).Value; }
        }

        public TypeReference this[int index]
        {
            get { return index >= _map.Count ? null : _map[index].Value; }
        }

        private GenericArgumentsMap(List<KeyValuePair<GenericParameter, TypeReference>> map)
        {
            _map = map ?? new List<KeyValuePair<GenericParameter, TypeReference>>();
        }

        public static GenericArgumentsMap Build(IList<GenericParameter> genericParameters,
            IList<TypeReference> genericArguments, GenericArgumentsMap currentMap)
        {
            if (genericParameters == null)
            {
                throw new ArgumentNullException("genericParameters");
            }
            if (genericArguments == null)
            {
                throw new ArgumentNullException("genericArguments");
            }

            if (genericParameters.Count != genericArguments.Count)
            {
                throw new ArgumentException();
            }

            var map = new List<KeyValuePair<GenericParameter, TypeReference>>(genericParameters.Count);

            for (int i = 0; i < genericArguments.Count; i++)
            {
                GenericParameter parameter = genericParameters[i];
                TypeReference argument = genericArguments[i];

                if (argument.IsGenericParameter)
                {
                    TypeReference found = currentMap[(GenericParameter)argument];

                    if (found != null)
                    {
                        argument = found;
                    }
                }

                map.Add(new KeyValuePair<GenericParameter, TypeReference>(parameter, argument));
            }

            return new GenericArgumentsMap(map);
        }

        public static GenericArgumentsMap Build(IList<GenericParameter> genericParameters,
            IList<TypeReference> genericArguments)
        {
            return Build(genericParameters, genericArguments, Empty);
        }

        public IEnumerator<KeyValuePair<string, TypeReference>> GetEnumerator()
        {
            return _map.Select(kvp => new KeyValuePair<string, TypeReference>(kvp.Key.Name, kvp.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
