using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Dictionaries */
[System.Serializable]
public class Dictionary<T>
{
    [SerializeField]
    public List<DictionaryItem<T>> dictionary;
    //[HideInInspector]
    public int length = 0;

    [HideInInspector]
    public bool lockKey = false;

    public Dictionary(List<DictionaryItem<T>> newDictionary = null)
    {
        dictionary = newDictionary;
    }

    public void Load(Dictionary<T> from)
    {
        dictionary = from.dictionary;
        length = dictionary.Count;
    }

    public T Get(string name)
    {
        if (length != 0)
        {
            return dictionary[GetIndex(name)].value;
        }
        else
        {
            return default(T);
        }
    }

    public void Set(string name, T value)
    {
        //If the item exists in the dictionary, set it's value, if it doesn't, add it to the List
        if (GetIndex(name) != -1)
        {
            dictionary[GetIndex(name)].value = value;
        }
        else
        {
            DictionaryItem<T> item = new DictionaryItem<T>(name, value);
            dictionary.Add(item);
            length++;
        }
    }

    public int GetIndex(string name)
    {
        int index = 0;

        if (length > 0)
        {
            foreach (DictionaryItem<T> item in dictionary)
            {
                if (item.key == name)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }
}

[System.Serializable]
public class DictionaryItem<T>
{
    [HideInInspector] public bool lockKey = false;
    public string key;
    public T value;
    //public string valueReadOnly;


    public DictionaryItem(string key = "", T value = default(T))
    {
        this.key = key;
        //this.valueReadOnly = value;
        this.value = value;
    }
}