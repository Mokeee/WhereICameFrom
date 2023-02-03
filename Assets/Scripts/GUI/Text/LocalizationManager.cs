using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    public LocalizationChannel channel;

    [SerializeField]
    public Dictionary<string, List<LocalizedText>> textRegisters;

    [SerializeField]
    private SystemLanguage _currentLanguage;
    public SystemLanguage CurrentLanguage
    {
        get
        {
            return _currentLanguage;
        }
        internal set
        {
            _currentLanguage = value;
        }
    }

    public Dictionary<SystemLanguage, TextAsset> localeAssets;
    [HideInInspector]
    public TextAsset[] _textAssets;

    private LocaleReader localeReader;

    #region Serialization
    public void OnBeforeSerialize()
    {
        if (localeAssets != null)
        {
            var currentLocales = new TextAsset[localeAssets.Count];
            localeAssets.Values.CopyTo(currentLocales, 0);

            if (currentLocales != _textAssets)
                _textAssets = currentLocales;
        }
    }

    public void OnAfterDeserialize()
    {
        localeAssets = new Dictionary<SystemLanguage, TextAsset>();

        for (int i = 0; i < _textAssets.Length; i++)
            localeAssets.Add((SystemLanguage)i, _textAssets[i]);
    }
    #endregion

    // Start is called before the first frame update
    void OnEnable()
    {
        localeReader = new LocaleReader(localeAssets[CurrentLanguage]);

        textRegisters = new Dictionary<string, List<LocalizedText>>();

        channel.OnRegisterText += RegisterText;
        channel.OnTranslateText += TranslateText;
        channel.OnLanguageChanged += ChangeLanguage;

        SceneManager.sceneLoaded += AddNewRegister;
        SceneManager.sceneUnloaded += RemoveRegister;

        SetLanguage(CurrentLanguage);
    }
    private void OnDisable()
    {
        channel.OnRegisterText -= RegisterText;
        channel.OnTranslateText -= TranslateText;
        channel.OnLanguageChanged -= ChangeLanguage;
    }

    /// <summary>
    /// Adds a new register entry.
    /// 
    /// Automatically called upon loading a new scene.
    /// </summary>
    /// <param name="loadedScene"></param>
    /// <param name="loadSceneMode"></param>
    private void AddNewRegister(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
        textRegisters.Add(loadedScene.name, new List<LocalizedText>());
    }

    /// <summary>
    /// Adds the text to the register of the active scene.
    /// Also translates the keyword.
    /// </summary>
    /// <param name="text"></param>
    public void RegisterText(LocalizedText text)
    {
        textRegisters[SceneManager.GetActiveScene().name].Add(text);

        text.UpdateLocalization(LookUpKeyword(text.Keyword));
    }

    /// <summary>
    /// Clear list of registered texts.
    /// 
    /// Automatically called upon switching scenes.
    /// </summary>
    private void RemoveRegister(Scene unloadedScene)
    {
        textRegisters.Remove(unloadedScene.name);
    }

    //Changes the current language without translating.
    public void SetLanguage(SystemLanguage language)
    {
        CurrentLanguage = language;
        channel.currentLanguage = language;
    }

    /// <summary>
    /// Changes the current language.
    /// </summary>
    /// <param name="language">New language</param>
    public void ChangeLanguage(SystemLanguage language)
    {
        SetLanguage(language);

        var locale = localeAssets[language];
        if (locale.text == "")
            locale = localeAssets[SystemLanguage.English];

        localeReader.ReadLanguageLocales(locale);

        UpdateLocalization();
    }

    /// <summary>
    /// Updates the localization for all registered texts.
    /// </summary>
    private void UpdateLocalization()
    {
        foreach (var key in textRegisters.Keys)
            foreach (var text in textRegisters[key])
                TranslateText(text);
    }

    /// <summary>
    /// Retrieves the locale for the keyword for the current language.
    /// </summary>
    /// <param name="keyword">Keyword to be looked for</param>
    /// <returns></returns>
    private string LookUpKeyword(string keyword)
    {
        return localeReader.FindTranslation(keyword);
    }

    /// <summary>
    /// Translates a given localized text.
    /// </summary>
    /// <param name="text">Text to translate</param>
    public void TranslateText(LocalizedText text)
    {
        if (text.Keyword == "" || text.Keyword == null)
            Debug.LogWarning(text.name + " has no keyword!");
        else
            text.UpdateLocalization(LookUpKeyword(text.Keyword));
    }
}
