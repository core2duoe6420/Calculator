using System;
using System.Collections.Generic;

namespace Net.AlexKing.Calculator.Core
{
    public class Selector
    {
        private Dictionary<string, Object> dictionary;

        public Selector() {
            dictionary = new Dictionary<string, Object>();
        }

        public bool HasValue(string key) {
            return dictionary.ContainsKey(key);
        }

        public void AddValue(Object value) {
            this.AddValue(value.ToString(), value);
        }

        public void AddValue(string key, Object value) {
            if (HasValue(key))
                dictionary.Remove(key);
            dictionary.Add(key, value);
        }

        public Object GetValue(string key) {
            if (!HasValue(key))
                return null;
            return dictionary[key];
        }

        public void PrintValue() {
            foreach (KeyValuePair<string, Object> kvp in dictionary) {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
