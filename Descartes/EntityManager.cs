using System;
using System.Collections.Generic;

namespace Descartes
{
    public struct EntityManager
    {
        internal EntityManager(World w)
        {
            _componentGroups = new List<ComponentGroup<dynamic>>();
            _componentIds = new Dictionary<Type, int>();
        }
        
        private readonly List<ComponentGroup<dynamic>> _componentGroups;
        private readonly Dictionary<Type, int> _componentIds;
    
    }
}