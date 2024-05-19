using System.Collections.Generic;

public static class CollectionUtility
{
    public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDiscionary, K key, V value)
    {
        if (serializableDiscionary.ContainsKey(key))
        {
            serializableDiscionary[key].Add(value);

            return;
        }

        serializableDiscionary.Add(key, new List<V>() { value });
    }
}
