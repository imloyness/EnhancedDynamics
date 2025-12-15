#region Assembly MTM101BaldAPI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\BBP Dependencies\MTM101BaldAPI.dll
// Decompiled with ICSharpCode.Decompiler 9.1.0.7988
#endregion

using System.Reflection;
using HarmonyLib;
using MTM101BaldAPI.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MTM101BaldAPI.OptionsAPI;

public abstract class CustomOptionsCategory : MonoBehaviour
{
    public OptionsMenu optionsMenu;

    public TooltipController tooltipController;

    public Transform toolTipHotspot;

    private static FieldInfo _textBox = AccessTools.Field(typeof(TextLocalizer), "textBox");

    private static FieldInfo _bars = AccessTools.Field(typeof(AdjustmentBars), "bars");

    private static FieldInfo _highlighted = AccessTools.Field(typeof(AdjustmentBars), "highlighted");

    private static FieldInfo _unhighlighted = AccessTools.Field(typeof(AdjustmentBars), "unhighlighted");

    private static FieldInfo _val = AccessTools.Field(typeof(MenuToggle), "val");

    private static FieldInfo _checkmark = AccessTools.Field(typeof(MenuToggle), "checkmark");

    private static FieldInfo _disableCover = AccessTools.Field(typeof(MenuToggle), "disableCover");

    private static FieldInfo _hotspot = AccessTools.Field(typeof(MenuToggle), "hotspot");

    protected Sprite bar => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("Bar");

    protected Sprite barFaded => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("BarTransparent");

    protected Sprite menuArrowLeft => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("MenuArrowLeft");

    protected Sprite menuArrowLeftHighlight => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("MenuArrowLeftHighlight");

    protected Sprite menuArrowRight => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("MenuArrowRight");

    protected Sprite menuArrowRightHighlight => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("MenuArrowRightHighlight");

    protected Sprite checkBox => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("CheckBox");

    protected Sprite checkMark => MTM101BaldiDevAPI.AssetMan.Get<Sprite>("Check");

    protected SoundObject silence => MTM101BaldiDevAPI.AssetMan.Get<SoundObject>("Silence");

    public abstract void Build();

    protected TextMeshProUGUI CreateText(string name, string text, Vector3 position, BaldiFonts font, TextAlignmentOptions alignment, Vector2 sizeDelta, Color color, bool createLocalizer = true)
    {
        TextMeshProUGUI textMeshProUGUI = UIHelpers.CreateText<TextMeshProUGUI>(font, text, base.transform, position);
        textMeshProUGUI.alignment = alignment;
        textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
        textMeshProUGUI.color = color;
        textMeshProUGUI.name = name;
        if (createLocalizer)
        {
            TextLocalizer textLocalizer = textMeshProUGUI.gameObject.AddComponent<TextLocalizer>();
            _textBox.SetValue(textLocalizer, textMeshProUGUI);
            textLocalizer.key = text;
            textLocalizer.GetLocalizedText(text);
        }
        else
        {
            textMeshProUGUI.text = text;
        }

        return textMeshProUGUI;
    }

    protected StandardMenuButton CreateTextButton(UnityAction action, string name, string text, Vector3 position, BaldiFonts font, TextAlignmentOptions alignment, Vector2 sizeDelta, Color color)
    {
        TextMeshProUGUI textMeshProUGUI = CreateText(name, text, position, font, alignment, sizeDelta, color);
        textMeshProUGUI.raycastTarget = true;
        StandardMenuButton standardMenuButton = textMeshProUGUI.gameObject.ConvertToButton<StandardMenuButton>();
        standardMenuButton.underlineOnHigh = true;
        standardMenuButton.OnPress.AddListener(action);
        return standardMenuButton;
    }

    protected Image CreateImage(Sprite background, string name, Vector3 position, Vector2? sizeDelta = null)
    {
        Image image = new GameObject(name).AddComponent<Image>();
        image.transform.SetParent(base.transform, worldPositionStays: false);
        image.transform.localScale = Vector3.one;
        image.name = name;
        image.transform.localPosition = position;
        image.gameObject.layer = LayerMask.NameToLayer("UI");
        if (sizeDelta.HasValue)
        {
            image.rectTransform.sizeDelta = sizeDelta.Value;
        }
        else
        {
            image.rectTransform.sizeDelta = background.rect.size;
        }

        image.sprite = background;
        return image;
    }

    protected StandardMenuButton CreateButton(UnityAction action, Sprite background, string name, Vector3 position, Vector2? sizeDelta = null)
    {
        Image image = CreateImage(background, name, position, sizeDelta);
        image.raycastTarget = true;
        StandardMenuButton standardMenuButton = image.gameObject.ConvertToButton<StandardMenuButton>();
        standardMenuButton.OnPress.AddListener(action);
        return standardMenuButton;
    }

    protected StandardMenuButton CreateButton(UnityAction action, Sprite unhighlighted, Sprite highlighted, string name, Vector3 position, Vector2? sizeDelta = null)
    {
        Image image = CreateImage(unhighlighted, name, position, sizeDelta);
        image.raycastTarget = true;
        StandardMenuButton standardMenuButton = image.gameObject.ConvertToButton<StandardMenuButton>();
        standardMenuButton.unhighlightedSprite = unhighlighted;
        standardMenuButton.highlightedSprite = highlighted;
        standardMenuButton.swapOnHigh = true;
        standardMenuButton.OnPress.AddListener(action);
        return standardMenuButton;
    }

    protected AdjustmentBars CreateBars(UnityAction onChanged, string name, Vector3 position, int count)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.SetActive(value: false);
        gameObject.layer = LayerMask.NameToLayer("UI");
        gameObject.transform.SetParent(base.transform, worldPositionStays: false);
        AdjustmentBars adjustBar = gameObject.AddComponent<AdjustmentBars>();
        StandardMenuButton standardMenuButton = CreateButton(delegate
        {
            adjustBar.Adjust(-1);
        }, menuArrowLeft, menuArrowLeftHighlight, "LeftCategoryButton", Vector3.zero);
        standardMenuButton.transform.SetParent(gameObject.transform, worldPositionStays: false);
        standardMenuButton.transform.localScale = Vector3.one;
        Vector3 vector = Vector3.right * (standardMenuButton.unhighlightedSprite.rect.size.x / 2f + 6f);
        gameObject.SetActive(value: true);
        GameObject gameObject2 = new GameObject("Bars");
        gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
        gameObject2.transform.localScale = Vector3.one;
        gameObject2.layer = LayerMask.NameToLayer("UI");
        Image[] array = new Image[count];
        for (int num = 0; num < count; num++)
        {
            Image image = CreateImage(barFaded, "Bar" + num, Vector3.zero);
            image.transform.SetParent(gameObject2.transform);
            image.transform.localScale = Vector3.one;
            image.transform.localPosition = Vector3.right * ((float)num * 10f) + vector;
            image.raycastTarget = false;
            array[num] = image;
        }

        StandardMenuButton standardMenuButton2 = CreateButton(delegate
        {
            adjustBar.Adjust(1);
        }, menuArrowRight, menuArrowRightHighlight, "RightCategoryButton", Vector3.right * (standardMenuButton.unhighlightedSprite.rect.size.x + 2f + (float)count * 10f));
        standardMenuButton2.transform.SetParent(gameObject.transform, worldPositionStays: false);
        standardMenuButton2.transform.localScale = Vector3.one;
        _bars.SetValue(adjustBar, array);
        _highlighted.SetValue(adjustBar, bar);
        _unhighlighted.SetValue(adjustBar, barFaded);
        adjustBar.onValueChanged = new UnityEvent();
        adjustBar.onValueChanged.AddListener(onChanged);
        gameObject.transform.localPosition = position;
        return adjustBar;
    }

    protected void SetupTooltipHotspots()
    {
        toolTipHotspot = new GameObject("TooltipHotspots").transform;
        toolTipHotspot.SetParent(base.transform, worldPositionStays: false);
        toolTipHotspot.localScale = Vector3.one;
        toolTipHotspot.localPosition = Vector3.zero;
        toolTipHotspot.SetAsFirstSibling();
    }

    protected void AddTooltip(StandardMenuButton button, string tooltip)
    {
        button.eventOnHigh = true;
        button.OnHighlight.AddListener(delegate
        {
            tooltipController.UpdateTooltip(tooltip);
        });
        button.OffHighlight.AddListener(delegate
        {
            tooltipController.CloseTooltip();
        });
    }

    protected void AddTooltip(AdjustmentBars bar, string tooltip)
    {
        AddTooltip(bar.transform.Find("LeftCategoryButton").GetComponent<StandardMenuButton>(), tooltip);
        AddTooltip(bar.transform.Find("RightCategoryButton").GetComponent<StandardMenuButton>(), tooltip);
        float num = Mathf.Abs(bar.transform.Find("LeftCategoryButton").position.x - bar.transform.Find("RightCategoryButton").position.x) / 3f;
        AddTooltipRegion(bar.name + "Region", bar.transform.localPosition + Vector3.right * (num / 2f), new Vector2(num, 32f), tooltip);
    }

    protected StandardMenuButton AddTooltipRegion(string name, Vector3 position, Vector2 size, string tooltip, bool visible = false)
    {
        if (toolTipHotspot == null)
        {
            SetupTooltipHotspots();
        }

        GameObject obj = new GameObject(name, typeof(Image));
        obj.transform.SetParent(toolTipHotspot, worldPositionStays: false);
        Image component = obj.GetComponent<Image>();
        component.color = new Color(1f, 1f, 0f, visible ? 1f : 0f);
        obj.transform.localPosition = position;
        component.rectTransform.sizeDelta = size;
        StandardMenuButton standardMenuButton = obj.ConvertToButton<StandardMenuButton>();
        standardMenuButton.audConfirmOverride = silence;
        AddTooltip(standardMenuButton, tooltip);
        return standardMenuButton;
    }

    protected MenuToggle CreateToggle(string name, string text, bool value, Vector3 position, float width)
    {
        Vector2 vector = new Vector2(width, 32f);
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.transform.SetParent(base.transform, worldPositionStays: false);
        gameObject.transform.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer("UI");
        gameObject.GetComponent<RectTransform>().sizeDelta = vector + new Vector2(8f, 0f);
        TextMeshProUGUI textMeshProUGUI = CreateText("ToggleText", text, Vector3.zero, BaldiFonts.ComicSans24, TextAlignmentOptions.TopRight, vector, Color.black);
        textMeshProUGUI.transform.SetParent(gameObject.transform);
        textMeshProUGUI.rectTransform.pivot = new Vector2(1f, 0.5f);
        textMeshProUGUI.transform.localScale = Vector3.one;
        textMeshProUGUI.transform.localPosition = new Vector3(-8f, 0f, 0f);
        MenuToggle toggle = gameObject.AddComponent<MenuToggle>();
        Image image = CreateImage(checkBox, "Box", Vector3.zero);
        image.transform.SetParent(gameObject.transform);
        image.transform.localScale = Vector3.one;
        image.transform.localPosition = new Vector3(8f, 0f, 0f);
        image.rectTransform.pivot = new Vector2(0f, 0.5f);
        Image image2 = CreateImage(checkMark, "Check", Vector3.zero, Vector2.one * 32f);
        image2.transform.SetParent(image.transform);
        image2.transform.localScale = Vector3.one;
        image2.transform.localPosition = new Vector3(21f, 6f, 0f);
        StandardMenuButton standardMenuButton = CreateButton(delegate
        {
            toggle.Toggle();
        }, null, "HotSpot", Vector3.zero, new Vector2(vector.x / 1.25f, vector.y));
        standardMenuButton.image.color = Color.clear;
        standardMenuButton.transform.SetParent(gameObject.transform);
        standardMenuButton.transform.localScale = Vector3.one;
        standardMenuButton.text = textMeshProUGUI;
        standardMenuButton.underlineOnHigh = true;
        standardMenuButton.transform.localPosition += Vector3.left * (width / 1.25f / 4f);
        _val.SetValue(toggle, value);
        _checkmark.SetValue(toggle, image2.gameObject);
        _hotspot.SetValue(toggle, standardMenuButton.gameObject);
        image2.gameObject.SetActive(value);
        gameObject.transform.localPosition = position;
        return toggle;
    }

    protected StandardMenuButton CreateApplyButton(UnityAction onApply)
    {
        StandardMenuButton standardMenuButton = CreateButton(onApply, null, "ApplyButton", new Vector3(136f, -160f, 0f), new Vector2(100f, 32f));
        standardMenuButton.image.color = Color.clear;
        TextMeshProUGUI textMeshProUGUI = CreateText("ApplyText", "Opt_Apply", Vector3.zero, BaldiFonts.ComicSans24, TextAlignmentOptions.TopRight, new Vector2(96f, 32f), Color.black);
        textMeshProUGUI.transform.SetParent(standardMenuButton.transform, worldPositionStays: false);
        textMeshProUGUI.transform.localScale = Vector3.one;
        standardMenuButton.text = textMeshProUGUI;
        standardMenuButton.underlineOnHigh = true;
        standardMenuButton.transform.localScale = Vector3.one;
        return standardMenuButton;
    }

    protected void AddTooltip(MenuToggle toggle, string tooltip)
    {
        AddTooltip(((GameObject)_hotspot.GetValue(toggle)).GetComponent<StandardMenuButton>(), tooltip);
    }
}
#if false // Decompilation log
'128' items in cache
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Users\Loyness\.nuget\packages\netstandard.library\2.0.3\build\netstandard2.0\ref\netstandard.dll'
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.CoreModule.dll'
------------------
Resolve: '0Harmony, Version=2.9.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: '0Harmony, Version=2.9.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\0Harmony.dll'
------------------
Resolve: 'BepInEx, Version=5.4.22.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'BepInEx, Version=5.4.23.4, Culture=neutral, PublicKeyToken=null'
WARN: Version mismatch. Expected: '5.4.22.0', Got: '5.4.23.4'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\BepInEx.dll'
------------------
Resolve: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\EnhancedDynamics\obj\Debug\netstandard2.0\publicized\Assembly-CSharp.dll'
------------------
Resolve: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.UI.dll'
------------------
Resolve: 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\BBP Dependencies\Newtonsoft.Json.dll'
------------------
Resolve: 'UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\Unity.TextMeshPro.dll'
------------------
Resolve: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.TextCoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.TextCoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.TextRenderingModule.dll'
------------------
Resolve: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.ImageConversionModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.ImageConversionModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.UnityWebRequestAudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.UnityWebRequestAudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'System.Runtime.InteropServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.1.2.0'
Load from: 'C:\Users\Loyness\.nuget\packages\netstandard.library\2.0.3\build\netstandard2.0\ref\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'System.Runtime.CompilerServices.Unsafe, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
#endif
