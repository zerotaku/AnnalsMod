using AnnalsMod.UI.Prefabs;
using NeoModLoader.General;
using NeoModLoader.General.UI.Window;
using NeoModLoader.General.UI.Window.Layout;
using NeoModLoader.General.UI.Window.Utils.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace AnnalsMod.UI.Windows;

/// <summary>
///     This is an example of window with auto layout.
///     <para>这是一个自动布局的窗口示例</para>
/// </summary>
public class AnnalsAutoLayoutWindow : AutoLayoutWindow<AnnalsAutoLayoutWindow>
{
    private Transform grid1;
    private Transform grid2;
    private Text line1_text;
    private Text line2_text;
    private Text line3_text;
    private AutoGridLayoutGroup TestColumnListGup;
    private AutoGridLayoutGroup TestRowListGup;
    private UiUnitAvatarElement unit_avatar;
    private int width = 100;
    // [Hotfixable] is not available in methods provided by MonoBehaviour. If you want to use it, please use it in the the method `update` and call `update` in `Update`
    // [Hotfixable] 不可用于 MonoBehaviour 提供的方法. 如果你想使用它, 请在 `update` 方法中使用, 并在 `Update` 中调用 `update`
    private void Update()
    {
        if (!Initialized || !IsOpened) return;
    }

    protected override void Init()
    {
        GetLayoutGroup().spacing = 3;
        #region Top part 顶部部分

        var top = this.BeginHoriGroup(new Vector2(200, 60), pSpacing: 5, pPadding: new RectOffset(3, 3, 0, 5));
        unit_avatar = Instantiate( 
            Resources.Load<GameObject>("windows/inspect_unit")
                .transform.Find("Background/Scroll View/Viewport/Content/Part 1/BackgroundAvatar")
                .GetComponent<UiUnitAvatarElement>(),
            null);
        unit_avatar.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
        top.AddChild(unit_avatar.gameObject);

        #region MultiText group 多行文本组

        var multi_text_group = top.BeginVertGroup(new Vector2(150, 40), pSpacing: 3);

        SimpleText line1 = Instantiate(SimpleText.Prefab, null);
        line1.Setup("点击<a href=\"www.baidu.com\">这里</a>生效", TextAnchor.MiddleCenter, new Vector2(150, 18));
        //line1.text.resizeTextMaxSize = 10;
        //line1_text = line1.text;

        SimpleText line2 = Instantiate(SimpleText.Prefab, null);
        line2.Setup("", TextAnchor.MiddleCenter, new Vector2(150, 18));
        //line2.text.resizeTextMaxSize = 10;
        //line2_text = line2.text;

        multi_text_group.AddChild(line1.gameObject);
        multi_text_group.AddChild(line2.gameObject);

        #endregion

        #endregion

        #region Inline text 1 内联文本 1

        SimpleText inline_text1 = Instantiate(SimpleText.Prefab, null);
        inline_text1.Setup("", TextAnchor.MiddleCenter, new Vector2(150, 11));
        inline_text1.background.enabled = false;
        var auto_localized_text = inline_text1.text.gameObject.AddComponent<LocalizedText>();
        auto_localized_text.key = "auto_layout_window_text_1_key";
        auto_localized_text.autoField = true;
        LocalizedTextManager.addTextField(auto_localized_text);

        AddChild(inline_text1.gameObject);

        #endregion

        #region Grid group 1 网格组 1

        var grid1_group = this.BeginHoriGroup(new Vector2(200, 50), pSpacing: 5, pPadding: new RectOffset(3, 3, 0, 0));
        grid1 = grid1_group.transform;
        Image grid1_group_background = grid1_group.gameObject.AddComponent<Image>();
        grid1_group_background.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        grid1_group_background.type = Image.Type.Sliced;
        grid1_group_background.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        for (int i = 0; i < 5; i++)
        {

            SimpleText text = Instantiate(SimpleText.Prefab, null);
            text.Setup("默认网格" + i.ToString(), TextAnchor.MiddleCenter, new Vector2(150, 11));
            text.background.enabled = true;
            text.background.color = Color.yellow;
            AddChild(text.gameObject);
        }

        #endregion

        #region Inline text 2 内联文本 2

        SimpleText inline_text2 = Instantiate(SimpleText.Prefab, null);
        inline_text2.Setup("", TextAnchor.MiddleCenter, new Vector2(150, 11), OnHyperlinkTextInfo);
        inline_text2.background.enabled = false;
        auto_localized_text = inline_text2.text.gameObject.AddComponent<LocalizedText>();
        auto_localized_text.key = "auto_layout_window_text_2_key";
        auto_localized_text.autoField = true;
        LocalizedTextManager.addTextField(auto_localized_text);
        
        //inline_text2.onHrefClick.AddListener(OnHyperlinkTextInfo);
        AddChild(inline_text2.gameObject);

        for (int i = 0; i < 4; i++)
        {

            SimpleText text = Instantiate(SimpleText.Prefab, null);
            text.Setup("内联文本" + i.ToString(), TextAnchor.MiddleLeft, new Vector2(150, 11));
            text.background.enabled = false;
            text.background.color = Color.blue;
            AddChild(text.gameObject);
        }


        #endregion

        #region Grid group 2 网格组 2


        TestColumnListGup = this.BeginGridGroup(5, GridLayoutGroup.Constraint.FixedColumnCount, new Vector2(200, 100), new Vector2(50, 24), new Vector2(4, 2));
        grid2 = TestColumnListGup.transform;
        Image grid2_group_background = TestColumnListGup.gameObject.AddComponent<Image>();
        grid2_group_background.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        grid2_group_background.type = Image.Type.Sliced;
        grid2_group_background.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);


        #endregion

        #region Grid group 2 网格组 3

        TestRowListGup = this.BeginGridGroup(100, GridLayoutGroup.Constraint.FixedRowCount, new Vector2(200, 300), new Vector2(100, 15), new Vector2(4, 2));
        //TestRowListGup = TestRowListGup.transform;
        Image grid2_group_background1 = TestRowListGup.gameObject.AddComponent<Image>();
        grid2_group_background1.sprite = SpriteTextureLoader.getSprite("ui/special/windowInnerSliced");
        grid2_group_background1.type = Image.Type.Sliced;
        grid2_group_background1.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);


        #endregion
    }

    public void InitList()
    {

        for (int i = 0; i < 9; i++)
        {
            TextAnchor textAnchor = (TextAnchor)i;
            string anchorName = System.Enum.GetName(typeof(TextAnchor),i);

            SimpleText textC = Instantiate(SimpleText.Prefab, null);
            textC.Setup(anchorName + i.ToString(), textAnchor, new Vector2(50, 20));
            textC.background.enabled = true;
            textC.background.color = Color.red;
            TestColumnListGup.AddChild(textC.gameObject);

            SimpleText textR = Instantiate(SimpleText.Prefab, null);
            textR.Setup(anchorName + i.ToString(), textAnchor, new Vector2(100, 11), OnHyperlinkTextInfo);
            textR.background.enabled = true;
            textR.background.color = Color.green;
            TestRowListGup.AddChild(textR.gameObject);

        }

    }

    public override void OnFirstEnable()
    {
        base.OnFirstEnable();
    }
    public override void OnNormalEnable()
    {
        base.OnNormalEnable();
        var kingdom = World.world.kingdoms.getRandom();
        if (kingdom == null || !kingdom.isAlive())
        {
            Clean();
            return;
        }
        if (kingdom.king == null || !kingdom.king.isAlive())
        {
            unit_avatar.show(kingdom.king);
            return;
        }
        line1_text.text = GetKingdomColorToHex(kingdom, kingdom.name);
        line2_text.text = kingdom.getMotto();
        //line3_text.text = unit.getAge().ToString();
        InitList();
        //InitmTextPro();

    }
    public override void OnNormalDisable()
    {
        base.OnNormalDisable();
        Clean();
    }
    private void Clean()
    {
        line1_text.text = "NONE";
        line2_text.text = "NONE";
    }
    public static string GetKingdomColorToHex(Kingdom kingdom, string text)
    {
        string color = Toolbox.colorToHex((Color32)kingdom.getColor().getColorText());
        return $"<color={color}>{text}</color>";
    }


    /// <summary>
    /// 当前点击超链接回调
    /// </summary>
    /// <param name="info">回调信息</param>
    private void OnHyperlinkTextInfo(string info)
    {
        Debug.Log("超链接信息：" + info);
    }
}