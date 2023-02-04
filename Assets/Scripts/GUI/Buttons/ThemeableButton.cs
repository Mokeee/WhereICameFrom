using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ThemeableButton : MonoBehaviour, ITooltipable, IPointerDownHandler, IPointerClickHandler
{
    public bool needsDoubleClick;
    public ButtonType buttonType;
    public string tooltipKeyword;
    public Image backgroundImage;
    public Image selectionImage;

    [Header("Buttons Sounds")]
    public string clickSound;
    public string hoverSound;
    public string pointerDownSound = "";

    public UIAnimationStartEvent OnBeginHoverEvent = new UIAnimationStartEvent();
    public UIAnimationStartEvent OnEndHoverEvent = new UIAnimationStartEvent();
    private bool isHovering;
    private Button button;

    private bool tooltipSpawned;
    private float tooltipTimeDelta;
    private float tooltipAppearanceMeantime = 1.0f;

    private float lastClick;
    private float clickInterval = 0.2f;

#if UNITY_EDITOR
    [MenuItem("GameObject/UI/ThemeableButton")]
    static void CreateNewButton(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Button", typeof(CanvasRenderer), typeof(Image), typeof(ThemeableButton));

        var text = LocalizedText.SpawnLocalizedText(go);

        var rect = text.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2();
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = new Vector2();
        rect.offsetMax = new Vector2();

        text.GetComponent<LocalizedText>().textType = TextType.Button;

        go.GetComponent<ThemeableButton>().clickSound = "defaultClickSound";
        go.GetComponent<ThemeableButton>().hoverSound = "defaultHoverSound";

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
#endif

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonClickSound);
        
        if(needsDoubleClick)
            button.interactable = false;
    }

    public void CloseTooltip()
    {
        tooltipSpawned = false;
    }

    public bool MouseOnTooltip()
    {
        return isHovering;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnBeginHoverEvent.Invoke(GetComponent<RectTransform>(), 0);
        isHovering = true;
        PlayButtonSound(hoverSound);
        if (needsDoubleClick)
            selectionImage.color = button.colors.highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnEndHoverEvent.Invoke(GetComponent<RectTransform>(), 0);
        isHovering = false;
        if (needsDoubleClick)
            selectionImage.color = new Color(0,0,0,0);
    }

    void Update()
    {
        if (isHovering && tooltipKeyword != "")
        {
            if (!tooltipSpawned)
            {
                tooltipTimeDelta += Time.deltaTime;

                if (tooltipTimeDelta >= tooltipAppearanceMeantime)
                {
                    var rectT = GetComponent<RectTransform>();
                    tooltipSpawned = true;
                    GameObject.FindObjectOfType<TooltipManager>().ShowTooltip(tooltipKeyword,
                        transform.TransformPoint(rectT.anchorMax + (new Vector2(1, 1) - rectT.pivot) * rectT.sizeDelta),
                        null,
                        this);
                    tooltipTimeDelta = 0;
                }
            }
        }
    }

    private void PlayButtonClickSound()
    {
        PlayButtonSound(clickSound);
    }

    public void PlayButtonSound(string soundname)
    {
        if (soundname != "")
        {
            Debug.Log("Play sound: " + soundname + "\n not yet implemented!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayButtonSound(pointerDownSound);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (needsDoubleClick)
        {
            if (lastClick + clickInterval > Time.time)
                GetComponent<Button>().onClick.Invoke();
            else
            {
                lastClick = Time.time;
                selectionImage.color = button.colors.pressedColor;
            }
        }
        else
            GetComponent<Button>().onClick.Invoke();
    }
}
