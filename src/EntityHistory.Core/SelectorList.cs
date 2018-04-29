using System.Collections.Generic;
using EntityHistory.Core.Interfaces;

namespace EntityHistory.Core
{
    internal class SelectorList : List<NamedTypeSelector>, ISelectorList
    {
        public bool RemoveByName(string name)
        {
            return RemoveAll(s => s.Name == name) > 0;
        }
    }
}
