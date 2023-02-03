using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LocalizationChannel", menuName = "ScriptableObjects/Channels/LocalizationChannel", order = 1)]
public class LocalizationChannel : ScriptableObject
{
	public UnityAction<LocalizedText> OnRegisterText;
	public UnityAction<LocalizedText> OnTranslateText;
	public UnityAction<SystemLanguage> OnLanguageChanged;

	public List<SystemLanguage> supportedLanguages;
	public SystemLanguage currentLanguage;

	public void RaiseRegisterEvent(LocalizedText localizedText)
	{
		if (OnRegisterText != null)
		{
			OnRegisterText.Invoke(localizedText);
		}
		else
		{
			Debug.LogWarning("A Text registration was requested, but nobody picked it up." +
				"Check why there is no Localizationmanager already present, " +
				"and make sure it's listening on this Localization channel.");
		}
	}
	public void RaiseTranslationEvent(LocalizedText localizedText)
	{
		if (OnTranslateText != null)
		{
			OnTranslateText.Invoke(localizedText);
		}
		else
		{
			Debug.LogWarning("A Text translation was requested, but nobody picked it up." +
				"Check why there is no Localizationmanager already present, " +
				"and make sure it's listening on this Localization channel.");
		}
	}
	public void RaiseLanguageChangeEvent(SystemLanguage language)
	{
		if (OnLanguageChanged != null)
		{
			OnLanguageChanged.Invoke(language);
		}
		else
		{
			Debug.LogWarning("A Text language change was requested, but nobody picked it up." +
				"Check why there is no Localizationmanager already present, " +
				"and make sure it's listening on this Localization channel.");
		}
	}
}
