using NeoModLoader.General.UI.Prefabs;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using TMPro;

namespace AnnalsMod.UI.Prefabs;

/// <summary>
///     A simple text prefab. Documentation is coming soon.
///     <para>一个简单的可选背景的文本框预制体, 文档很快就来了</para>
/// </summary>
public class SimpleText : APrefab<SimpleText>
{
    public Text text { get; private set; }
    public mTextPro textp { get; private set; }
    public Image background { get; private set; }


    [Serializable]
    public class HrefClickEvent : UnityEvent<string> { }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }

    public string TextConten
    {
        get
        {
            return textp.text;
        }
        set
        {
            text.text = value;  
            textp.text = value;
        }
    }

    protected override void Init()
    {
        if (Initialized) return;
        base.Init();
        text = transform.Find("Text").GetComponent<Text>();
        textp = transform.Find("mTextPro").GetComponent<mTextPro>();
        background = GetComponent<Image>();
    }
    public void Setup(string pText, TextAnchor pAlignment = TextAnchor.MiddleLeft, Vector2 pSize = default, UnityAction<string> action = null, Sprite pBackground = null)
    {
        Init();
        SetSize(pSize == default ? new Vector2(200, 18) : pSize);
        TextConten = pText;
        text.alignment = pAlignment;
        background.sprite = pBackground == null ? SpriteTextureLoader.getSprite("ui/special/windowInnerSliced") : pBackground;

        /*if (action != null)
        {
            Button buttonI = text.gameObject.AddComponent<Button>();
            buttonI.onClick.AddListener(onClick);
            onHrefClick.AddListener(action);
        }*/
    }

    public void onClick()
    {
        m_OnHrefClick?.Invoke(text.text);
    }

    public override void SetSize(Vector2 pSize)
    {
        base.SetSize(pSize);
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(pSize.x * 0.9f, pSize.y * 0.95f);
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        // 检查文本内容
        string textContent = text.text;
        // 使用正则表达式查找超链接
        MatchCollection matches = Regex.Matches(textContent, @"<a\s+value=""(?<url>[^""]+)"">(?<label>[^<]+)</a>");

        // 检查点击位置
        if (eventData.pointerCurrentRaycast.gameObject == text.gameObject)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(text.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            int characterIndex = GetCharacterIndexAtPoint(localPoint);

            foreach (Match match in matches)
            {
                if (characterIndex >= match.Index && characterIndex <= match.Index + match.Length)
                {
                    string url = match.Groups["url"].Value;
                    OpenURL(url);
                    break;
                }
            }
        }
    }*/

    //private int GetCharacterIndexAtPoint(Vector2 point)
    //{
    //    // 使用 Text 的字符索引来确定点击位置
    //    Vector3[] worldCorners = new Vector3[4];
    //    text.rectTransform.GetWorldCorners(worldCorners);

    //    // 计算相对位置
    //    Vector2 localPoint = point - (Vector2)worldCorners[0];

    //    // 获取字符索引
    //    int characterIndex = TMP_TextUtilities.FindIntersectingCharacter(text, Input.mousePosition, Camera.main, true);
    //    return characterIndex;
    //}

    //private void OpenURL(string url)
    //{
    //    Debug.LogWarning(url);
    //}

    private static void _init()
    {
        GameObject obj = new("SimpleText", typeof(Image));
        obj.transform.SetParent(ModClass.prefab_library);
        obj.GetComponent<Image>().sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        obj.GetComponent<Image>().type = Image.Type.Sliced;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 18);

        GameObject text = new("Text", typeof(Text));
        text.transform.SetParent(obj.transform);
        text.transform.localScale = Vector3.one;
        text.transform.localPosition = Vector3.one;

        Text textComponent = text.GetComponent<Text>();
        textComponent.alignment = TextAnchor.MiddleLeft;
        textComponent.resizeTextForBestFit = true;
        textComponent.resizeTextMinSize = 1;
        textComponent.resizeTextMaxSize = 18;
        textComponent.text = "";
        textComponent.color = Color.white;
        textComponent.enabled = true;
        textComponent.font = LocalizedTextManager.currentFont;

        GameObject text1 = new("mTextPro", typeof(mTextPro));
        text1.transform.SetParent(obj.transform);
        text1.transform.localScale = Vector3.one;
        text1.transform.localPosition = Vector3.one;

        mTextPro textComponent1 = text1.GetComponent<mTextPro>();
        textComponent1.alignment = TextAnchor.MiddleLeft;
        textComponent1.resizeTextForBestFit = true;
        textComponent1.resizeTextMinSize = 1;
        textComponent1.resizeTextMaxSize = 18;
        textComponent1.text = "";
        textComponent1.color = Color.white;
        textComponent1.font = LocalizedTextManager.currentFont;

        Prefab = obj.AddComponent<SimpleText>();
    }
}