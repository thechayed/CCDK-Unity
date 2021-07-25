using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Dictionaries */
[System.Serializable]
public class FlagDictionary
{
    [SerializeField]
    public List<FlagDictionaryItem> dictionary;

    public bool Get(string name)
    {
        return dictionary[GetIndex(name)].value;
    }

    public void Set(string name, bool value)
    {
        //If the item exists in the dictionary, set it's value, if it doesn't, add it to the List
        if (GetIndex(name) != -1)
        {
            dictionary[GetIndex(name)].value = value;
        }
        else
        {
            FlagDictionaryItem item = new FlagDictionaryItem(); 
            item.key = name;
            item.value = value;
            dictionary.Add(item);
        }
        
    }

    public int GetIndex(string name)
    {
        int index = 0;
        foreach (FlagDictionaryItem item in dictionary)
        {
            if (item.key == name)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}

[System.Serializable]
public class FlagDictionaryItem
{
    public string key;
    public bool value;
}