using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Dictionaries */
[System.Serializable]
public class AudioClipDictionary
{
    [SerializeField]
    public List<AudioClipDictionaryItem> dictionary;

    public AudioClip Get(string name)
    {
        return dictionary[GetIndex(name)].value;
    }

    public void Set(string name, AudioClip value)
    {
        //If the item exists in the dictionary, set it's value, if it doesn't, add it to the List
        if (GetIndex(name) != -1)
        {
            dictionary[GetIndex(name)].value = value;
        }
        else
        {
            AudioClipDictionaryItem item = new AudioClipDictionaryItem();
            item.key = name;
            item.value = value;
            dictionary.Add(item);
        }
    }

    public int GetIndex(string name)
    {
        int index = 0;
        foreach (AudioClipDictionaryItem item in dictionary)
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
public class AudioClipDictionaryItem
{
    public string key;
    public AudioClip value;
}