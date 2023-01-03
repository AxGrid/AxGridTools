using System;

namespace AxGrid.Utils.Reflections
{
    public interface IPropertyOrField
    {
        string Name { get; }
        Type ValueType { get; }
        bool IsIndexed { get; }
    }
}