namespace Superscribe.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;

    public class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        // The inner dictionary.
        private readonly IDictionary<string, object> dictionary = new Dictionary<string, object>();
        
        // This property returns the number of elements 
        // in the inner dictionary. 
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.dictionary.IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }
        
        public object this[string key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                this.dictionary[key] = value;
            }
        }

        // If you try to get a value of a property  
        // not defined in the class, this method is called. 
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase 
            // so that property names become case-insensitive. 
            string name = binder.Name;

            // If the property name is found in a dictionary, 
            // set the result parameter to the property value and return true. 
            // Otherwise, return false. 
            return this.dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is 
        // not defined in the class, this method is called. 
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase 
            // so that property names become case-insensitive.
            this.dictionary[binder.Name] = value;

            // You can always add a value to a dictionary, 
            // so this method always returns true. 
            return true;
        }

        #region IDictionary Members
        
        public void Add(KeyValuePair<string, object> item)
        {
            this.dictionary.Add(item);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            this.dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        #endregion
    }
}
