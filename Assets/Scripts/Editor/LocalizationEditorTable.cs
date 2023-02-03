using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;



public class LocalizationEditorTable : EditorWindow
{
    public string baseFolder;
    List<TextAsset> locales;
    Dictionary<string, LocaleReader> readers;
    string newKeyword;
    Dictionary<string, string> newTranslations;


    bool inEditMode;
    private Vector2 horizontalOuterScrollPos;
    private Vector2 verticalInnerScrollPos;

    public enum Filtering{
        ShowAll,
        ExistingTranslationsOnly,
        MissingTranslationsOnly,
        HideAll
    }
    struct LanguageOptions {
        public string language;
        public Filtering filter;
    }
    bool languageOptionsFoldout;
	ReorderableList languageOptionsReordableList;
    List<LanguageOptions> languageOptions;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Localization/LocalizationTable")]
    static void InitLocalization()
    {
        // Get existing open window or if none, make a new one:
        LocalizationEditorTable window = (LocalizationEditorTable)EditorWindow.GetWindow(typeof(LocalizationEditorTable), false, "LocalizationTable", false);
        // despite setting focus=false ^, OnFocus and OnGUI will already be called here, before baseFolder is set or Show is used
        // workaround: leave those methods if it is not set, set it, call OnFocus again explicitly to maintain execution order and set focus explicitly
        window.baseFolder="Localization";
        window.OnFocus();
        window.Show();
        window.Focus();
    }
    // Add menu named "My Window" to the Window menu
    [MenuItem("Balancing/BalancingTable")]
    static void InitBalancing()
    {
        // Get existing open window or if none, make a new one:
        LocalizationEditorTable window = (LocalizationEditorTable)EditorWindow.GetWindow(typeof(LocalizationEditorTable), false, "BalancingTable", false);
        // despite setting focus=false ^, OnFocus and OnGUI will already be called here, before baseFolder is set or Show is used
        // workaround: leave those methods if it is not set, set it, call OnFocus again explicitly to maintain execution order and set focus explicitly
        window.baseFolder="Balancing";
        window.OnFocus();
        window.Show();
        window.Focus();
    }


    void OnFocus()
    {
        if(baseFolder == null)
            return;
        // Get existing open window or if none, make a new one:
        readers = LocaleReader.GetReaders(baseFolder, out locales);
        if(newTranslations == null){
            languageOptions= new List<LanguageOptions>();
            newTranslations = new Dictionary<string, string>();
        }

        // remove old language options
        for(int idx=0; idx < languageOptions.Count;)
        {
            if(readers.ContainsKey(languageOptions[idx].language))
                idx++;
            else{
                newTranslations.Remove(languageOptions[idx].language);
                languageOptions.RemoveAt(idx);
            }
        }
        // add new language options
        foreach(string language in readers.Keys){
            int idx=0;
            for(; idx < languageOptions.Count;idx++)
                if(languageOptions[idx].language==language)
                    break;            
            if(idx >= languageOptions.Count){
                languageOptions.Add(new LanguageOptions(){language= language, filter=Filtering.ShowAll});
                newTranslations.Add(language, "");
            }
        }
        
        // add reordable list
		languageOptionsReordableList = new ReorderableList(languageOptions, typeof(LanguageOptions), true, false, false, false);
        languageOptionsReordableList.headerHeight=0;
        languageOptionsReordableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = languageOptions[index];
            element.filter =  (Filtering)EditorGUI.EnumPopup(rect, element.language, element.filter);
            languageOptions[index]=element;
          };
    }

    private void OnLostFocus()
    {
        if (inEditMode)
            LocaleReader.SortAndSaveEdits(locales, readers);
        inEditMode=false;
    }

    void OnGUI()
    {
        if(baseFolder == null)
            return;
        languageOptionsFoldout = EditorGUILayout.Foldout(languageOptionsFoldout, "Language Options");
        if(languageOptionsFoldout)
            languageOptionsReordableList.DoLayoutList();
 
        if(inEditMode)
            GUI.backgroundColor = Color.blue;

        if (GUILayout.Button("Toggle Live Edit Translations"))
        {
            if (inEditMode)
                LocaleReader.SortAndSaveEdits(locales, readers);
            inEditMode = !inEditMode;
            // force losing focus of an input field when toggling edit mode,  
            // otherwise the input field will still have the new value when toggling back, even if it references something else now
            EditorGUI.FocusTextInControl(null);
        }


        GUIStyle gSHeader = new GUIStyle();
        gSHeader.fontStyle = FontStyle.Bold;
        gSHeader.normal.textColor = Color.white;
        gSHeader.fontSize = 15;

        GUIStyle hiddenVerticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
        hiddenVerticalScrollbar.fixedWidth = 0;
        horizontalOuterScrollPos = GUILayout.BeginScrollView(horizontalOuterScrollPos,true, false, GUI.skin.horizontalScrollbar, hiddenVerticalScrollbar, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        //Header line
        GUI.backgroundColor = Color.black;
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Keyword", gSHeader);
        foreach (var languageOption in languageOptions)
            if(languageOption.filter!=Filtering.HideAll)
                EditorGUILayout.LabelField(languageOption.language, gSHeader);
        EditorGUILayout.EndHorizontal();

        verticalInnerScrollPos = GUILayout.BeginScrollView(verticalInnerScrollPos,false,true, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
        //Content lines
        List<string> filteredKeys= GetFilteredKeys();
        if (inEditMode)
            DrawEditableContent(filteredKeys);
        else
            DrawUneditableContent(filteredKeys);
        GUILayout.EndScrollView ();

        if(inEditMode)
            DrawNewKeywordLine();

        EditorGUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.EndScrollView ();
    }

    public List<string> GetFilteredKeys(){
        List<string> existingTransationFilters = new List<string>(languageOptions.Count);
        List<string> missingTransationFilters = new List<string>(languageOptions.Count);
        foreach (var languageOption in languageOptions){
            if(languageOption.filter == Filtering.ExistingTranslationsOnly)
                existingTransationFilters.Add(languageOption.language);
            else if(languageOption.filter == Filtering.MissingTranslationsOnly)
                missingTransationFilters.Add(languageOption.language);
        }
        if(existingTransationFilters.Count==0 && missingTransationFilters.Count == 0)
            return new List<string>(readers[languageOptions[0].language].GetAllKeywords());

        List<string> filteredKeys = new List<string>(readers[languageOptions[0].language].GetAllKeywords().Count);
        foreach(string key in readers[languageOptions[0].language].GetAllKeywords()){
            bool skip = false;
            foreach(string language in missingTransationFilters)
                if(readers[language].FindTranslation(key)!=""){
                    skip = true;
                    break;
                }
            if(skip)
                continue;
            foreach(string language in existingTransationFilters)
                if(readers[language].FindTranslation(key)==""){
                    skip = true;
                    break;
                }
            if(skip)
                continue;
            filteredKeys.Add(key);
        }
        return filteredKeys;
    }

    public void DrawUneditableContent(List<string> filteredKeys)
    {
        int i = 0;
        Color[] bgColors = { Color.white, Color.grey };
        foreach (var key in filteredKeys)
        {
            GUI.backgroundColor = bgColors[i % 2];
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(key);

            foreach (var languageOption in languageOptions)
                if(languageOption.filter!=Filtering.HideAll)
                    EditorGUILayout.LabelField(readers[languageOption.language].FindTranslation(key));
            EditorGUILayout.EndHorizontal();

            i++;
        }
    }

    public void DrawEditableContent(List<string> filteredKeys)
    {
        GUIStyle gSButton = new GUIStyle();
        gSButton.fontStyle = FontStyle.Bold;
        gSButton.normal.textColor = Color.red;

        int i = 0;
        Color[] bgColors = { Color.white, Color.grey };
        foreach (var key in filteredKeys)
        {
            GUI.backgroundColor = bgColors[i % 2];
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            if (GUILayout.Button("X", gSButton, GUILayout.Width(25)))
            {
                LocaleReader.RemoveKeyword(key, readers);
            }

            string newKey = EditorGUILayout.TextField(key);
            if(newKey != key)
                LocaleReader.RenameKeyword(key, newKey, readers);

            foreach (var languageOption in languageOptions)
                if(languageOption.filter!=Filtering.HideAll)
                {
                    string transl = EditorGUILayout.TextField(readers[languageOption.language].FindTranslation(key));
                    readers[languageOption.language].SetTranslation(key, transl);
                }

            EditorGUILayout.EndHorizontal();

            i++;
        }
    }
    
    public void DrawNewKeywordLine(){
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUIStyle gSButton = new GUIStyle();
        gSButton.fontStyle = FontStyle.Bold;
        gSButton.normal.textColor = Color.white;

        bool add = GUILayout.Button("+", gSButton, GUILayout.Width(25));
        newKeyword = EditorGUILayout.TextField(newKeyword);
        
        if (newKeyword!=null && newKeyword!="" && !LocaleReader.FindKeyword(newKeyword, readers)){

            if (add){
                LocaleReader.AddKeyword(newKeyword, readers);
                foreach (var languageOption in languageOptions)
                    if(languageOption.filter!=Filtering.HideAll && newTranslations[languageOption.language]!=null){
                        readers[languageOption.language].SetTranslation(newKeyword, newTranslations[languageOption.language]);
                        newTranslations[languageOption.language]="";
                    }
                newKeyword = "";
            }

            foreach (var languageOption in languageOptions)
                if(languageOption.filter!=Filtering.HideAll)
                    newTranslations[languageOption.language] = EditorGUILayout.TextField(newTranslations[languageOption.language]);
        }

        EditorGUILayout.EndHorizontal();
    }
}
