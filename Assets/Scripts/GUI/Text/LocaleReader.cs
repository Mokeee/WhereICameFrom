using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[Serializable]
public struct Locale
{
    public string keyword;
    public string translation;
}

[Serializable]
public struct LocaleLookUp
{
    public List<Locale> locales;
}


public class LocaleReader
{
    private Dictionary<string, string> localeDictionary;

#if UNITY_EDITOR
    public static Dictionary<string, LocaleReader> GetReaders(string baseFolder, out List<TextAsset> locales)
    {
        Dictionary<string, LocaleReader> readers = new Dictionary<string, LocaleReader>();
        locales = new List<TextAsset>();

        HashSet<string> keys = new HashSet<string>();
        var info = new DirectoryInfo(Application.dataPath + "/" + baseFolder);
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            var asset = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets" + "/" + baseFolder + "/" + file.Name, typeof(TextAsset));
            locales.Add(asset);
            var reader = new LocaleReader(asset);
            readers.Add(asset.name, reader);
            keys.UnionWith(reader.localeDictionary.Keys);
        }
        foreach (LocaleReader reader in readers.Values)
            foreach (string key in keys)
                // reader.localeDictionary.TryAdd(key, "");
                if (!reader.localeDictionary.ContainsKey(key))
                    reader.localeDictionary.Add(key, "");

        return readers;
    }

    public static void SortAndSaveEdits(List<TextAsset> locales, Dictionary<string, LocaleReader> readers)
    {
        foreach (var locale in locales)
        {
            // update table itself instead of saved asset only
            // otherwise, there will be a desinc which can cause issues; either immediately or when it gets fixed without you noticing
            readers[locale.name].sortDict();
            File.WriteAllText(AssetDatabase.GetAssetPath(locale), readers[locale.name].GetLocale());
            EditorUtility.SetDirty(locale);
        }
    }
#endif

    public LocaleReader(TextAsset localeAsset)
    {
        localeDictionary = new Dictionary<string, string>();

        ReadLanguageLocales(localeAsset);
    }

    public void ReadLanguageLocales(TextAsset localeAsset)
    {
        if (localeAsset == null)
            throw new NullReferenceException("There is no translation asset for the selected language!");
        if (localeAsset.text.Length == 0)
            throw new InvalidOperationException("The translation asset for the selected language is empty!");

        var locales = JsonUtility.FromJson<LocaleLookUp>(localeAsset.text);
        localeDictionary = new Dictionary<string, string>();

        foreach (var locale in locales.locales)
            localeDictionary.Add(locale.keyword, locale.translation);
    }

    public string FindTranslation(string keyword)
    {
        return (localeDictionary.ContainsKey(keyword)) ? localeDictionary[keyword] : keyword;
    }

    public bool FindTranslation(string keyword, out string translation)
    {
        return localeDictionary.TryGetValue(keyword, out translation);
    }

    public bool SetTranslation(string keyword, string translation)
    {
        if (localeDictionary.ContainsKey(keyword))
        {
            localeDictionary[keyword] = translation;
            return true;
        }
        return false;
    }

    public static bool FindKeyword(string keyword, Dictionary<string, LocaleReader> readers)
    {
        foreach (LocaleReader reader in readers.Values)
            if (reader.localeDictionary.ContainsKey(keyword))
                return true;
        return false;
    }

    public static bool AddKeyword(string keyword, Dictionary<string, LocaleReader> readers)
    {
        foreach (var reader in readers.Values)
            if (reader.localeDictionary.ContainsKey(keyword))
                return false;
        foreach (var reader in readers.Values)
            reader.localeDictionary.Add(keyword, "");
        return true;
    }

    public static bool RenameKeyword(string oldKeyword, string newKeyword, Dictionary<string, LocaleReader> readers)
    {
        foreach (LocaleReader reader in readers.Values)
        {
            if (reader.localeDictionary.ContainsKey(newKeyword))
                return false;
            if (!reader.localeDictionary.ContainsKey(oldKeyword))
                throw new ArgumentException(oldKeyword + " cannot be renamed because it is not in the dictionary.");
        }
        foreach (LocaleReader reader in readers.Values)
        {
            reader.localeDictionary.Add(newKeyword, reader.localeDictionary[oldKeyword]);
            reader.localeDictionary.Remove(oldKeyword);
        }
        return true;
    }

    public static void RemoveKeyword(string keyword, Dictionary<string, LocaleReader> readers)
    {
        foreach (var reader in readers.Values)
            if (!reader.localeDictionary.ContainsKey(keyword))
                throw new ArgumentException(keyword + " cannot be removed from locale reader because it is not in there.");

        foreach (var reader in readers.Values)
            reader.localeDictionary.Remove(keyword);
    }

    public List<string> GetAllKeywords()
    {
        return new List<string>(localeDictionary.Keys);
    }

    private void sortDict()
    {
        Dictionary<string, string> newDict = new Dictionary<string, string>(localeDictionary.Count);
        foreach (var locale in localeDictionary.OrderBy(key => key.Key))
            newDict.Add(locale.Key, locale.Value);
        localeDictionary = newDict;
    }

    private string GetLocale()
    {
        LocaleLookUp lookUp = new LocaleLookUp { locales = new List<Locale>() };
        foreach (var locale in localeDictionary)
            lookUp.locales.Add(new Locale { keyword = locale.Key, translation = locale.Value });
        return JsonUtility.ToJson(lookUp, true);
    }
}
