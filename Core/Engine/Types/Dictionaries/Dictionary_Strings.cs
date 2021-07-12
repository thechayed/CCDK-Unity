using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Dictionaries */
[System.Serializable]
public class StringsDictionary
{
    [SerializeField]
    public List<StringsDictionaryItem> dictionary;
//[HideInInspector]
public int length = 0;

    [HideInInspector]
    public bool lockKey = false;

    public StringsDictionary(List<StringsDictionaryItem> newDictionary = null)
    {
        dictionary = newDictionary;
    }

    public void Load(StringsDictionary from)
    {
        dictionary = from.dictionary;
        length = dictionary.Count;
    }

    public string Get(string name)
    {
        if (length != 0)
        {
            return dictionary[GetIndex(name)].value;
        }
        else
        {
            return "";
        }
    }

    public void Set(string name, string value)
    {
        //If the item exists in the dictionary, set it's value, if it doesn't, add it to the List
        if (GetIndex(name) != -1)
        {
            dictionary[GetIndex(name)].value = value;
        }
        else
        {
            StringsDictionaryItem item = new StringsDictionaryItem(name, value);
            dictionary.Add(item);
            length++;
        }
    }

    public int GetIndex(string name)
    {
        int index = 0;

        if (length > 0)
        {
            foreach (StringsDictionaryItem item in dictionary)
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
public class StringsDictionaryItem
{
    [HideInInspector] public bool lockKey = false;
    public string key;
    public string value;
    //public string valueReadOnly;


    public StringsDictionaryItem(string key = "", string value = "", bool lockedKey = false)
    {
        this.key = key;
        //this.valueReadOnly = value;
        this.value = value;
    }
}