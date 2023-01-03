using System;
using System.Collections.Generic;

namespace AxGrid.Utils.Reflections
{
    public class PropertyOrFieldCollection
    {
        public Type ObjectType { get; }
        readonly Dictionary<string, IPropertyOrField> _fieldsCollection = new Dictionary<string, IPropertyOrField>();
        
        public IPropertyOrField Get(string name)
        {
            if (_fieldsCollection.ContainsKey(name))
                return _fieldsCollection[name];
            
            
            var pof = new PropertyOrField(ObjectType, name);
            _fieldsCollection.Add(name,pof);
            return pof;
        }

        
        
        public PropertyOrFieldCollection(Type objectType)
        {
            ObjectType = objectType;
        }        
        
        
    }
}