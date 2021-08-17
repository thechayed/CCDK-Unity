using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Dictionaries */
[System.Serializable]
public class Dictionary<T>
{
    [SerializeField]
    public List<DictionaryItem<T>> dictionary = new List<DictionaryItem<T>>();
    //[HideInInspector]
    public int length = 0;

    [HideInInspector]
    public bool lockKey = false;

    public Type type = typeof(T);

    public Dictionary(List<DictionaryItem<T>> newDictionary = default(List<DictionaryItem<T>>))
    {
        if (newDictionary != null)
        {
            dictionary = newDictionary;
        }
    }

    public void Load(Dictionary<T> from)
    {
        if(from != null)
        {
            dictionary = (List<DictionaryItem<T>>) from.dictionary;
            length = dictionary.Count;
        }
        else
        {
            Debug.Log("Attempt to load Dictionary from a Null value has been blocked.");
        }
    }

    public T Get(string name)
    {
        length = dictionary.Count;
        if (length != 0 && GetIndex(name)!=-1)
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
        length = dictionary.Count;
        //If the item exists in the dictionary, set it's value, if it doesn't, add it to the List
        if (GetIndex(name) != -1)
        {
            dictionary[GetIndex(name)].value = value;
        }
        else
        {
            DictionaryItem<T> item = new DictionaryItem<T>(name, value);
            dictionary.Add(item);
        }
        length = dictionary.Count;
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