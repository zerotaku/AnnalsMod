
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnnalsMod.UI.Prefabs
{

    public class mTextPro : Text, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private enum eventDataType
        {
            None,
            Enter,
            Exit,
            Click,
        }

        /// <summary>
        /// 超链接信息类
        /// </summary>
        private class TextProInfo
        {
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();
        }

        /// <summary>
        /// 正则表达式
        /// </summary>
        private readonly Regex s_HrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        /// <summary>
        /// 解析完最终的文本
        /// </summary>
        private string m_OutputText;

        /// <summary>
        /// 文本信息列表
        /// </summary>
        private List<TextProInfo> m_HrefInfos = new List<TextProInfo>();

        /// <summary>
        /// 文本构造器
        /// </summary>
        protected StringBuilder s_TextBuilder = new StringBuilder();

        /// <summary>
        /// 鼠标点击事件文字
        /// </summary>
        private UnityAction clickAction;
        /// <summary>
        /// 鼠标移入事件文本
        /// </summary>
        private UnityAction enterAction;
        /// <summary>
        /// 鼠标移出事件文本
        /// </summary>
        private UnityAction exitAction;

        /// <summary>
        /// 事件文字的颜色
        /// </summary>
        private string eventStrColor = "#FF9900";

        /// <summary>
        /// 时间文本替代字符
        /// </summary>
        public string EventStrReplace => "{eventStr}";

        /// <summary>
        /// 文本信息
        /// </summary>
        private string textMessage;

        /// <summary>
        /// 当前鼠标事件
        /// </summary>
        private PointerEventData curEventData;

        /// <summary>
        /// 当前鼠标事件的类型
        /// </summary>
        private eventDataType curEventDataType;


        public void SetText(string messageStr, string eventStr, string eventColor, UnityAction clickAc, UnityAction enterAc, UnityAction exitAc)
        {
            string tempStr = string.Format("<a href=事件文本>[{0:name}]</a>", eventStr);
            textMessage = messageStr.Replace(EventStrReplace, tempStr);
            if (string.IsNullOrWhiteSpace(eventColor))
            {
                eventStrColor = "#FF9900";
            }
            else
            {
                eventStrColor = eventColor;
            }

            clickAction = clickAc;
            enterAction = enterAc;
            exitAction = exitAc;
            SetVerticesDirty();
        }

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            text = textMessage;
            m_OutputText = GetOutputText(text);

        }

        private void OnGUI()
        {
            chackEventData();
        }


        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            string orignText = m_Text;
            m_Text = m_OutputText;
            base.OnPopulateMesh(toFill);
            m_Text = orignText;
            UIVertex vert = new UIVertex();

            // 处理文本包围框
            for (int i = 0; i < m_HrefInfos.Count; i++)
            {
                m_HrefInfos[i].boxes.Clear();
                if (m_HrefInfos[i].startIndex >= toFill.currentVertCount)
                {
                    continue;
                }

                // 将事件文本里面的文本顶点索引坐标加入到包围框
                toFill.PopulateUIVertex(ref vert, m_HrefInfos[i].startIndex);
                Vector3 pos = vert.position;
                Bounds bounds = new Bounds(pos, Vector3.zero);
                for (int TEMP = m_HrefInfos[i].startIndex, m = m_HrefInfos[i].endIndex; TEMP < m; TEMP++)
                {
                    if (TEMP >= toFill.currentVertCount)
                    {
                        break;
                    }

                    toFill.PopulateUIVertex(ref vert, TEMP);
                    pos = vert.position;
                    if (pos.x < bounds.min.x) // 换行重新添加包围框
                    {
                        m_HrefInfos[i].boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(pos); // 扩展包围框
                    }
                }
                m_HrefInfos[i].boxes.Add(new Rect(bounds.min, bounds.size));
            }
        }

        /// <summary>
        /// 获取超链接解析后的最后输出文本
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOutputText(string outputText)
        {
            s_TextBuilder.Length = 0;
            m_HrefInfos.Clear();
            var indexText = 0;
            foreach (Match match in s_HrefRegex.Matches(outputText))
            {
                s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));
                s_TextBuilder.Append("<color=" + eventStrColor + ">");  // 事件文本颜色

                var group = match.Groups[1];
                TextProInfo hrefInfo = new TextProInfo
                {
                    startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
                    endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                    name = group.Value
                };
                m_HrefInfos.Add(hrefInfo);

                s_TextBuilder.Append(match.Groups[2].Value);
                s_TextBuilder.Append("</color>");
                indexText = match.Index + match.Length;
            }
            s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
            return s_TextBuilder.ToString();
        }

        /// <summary>
        /// 点击事件检测是否点击到超链接文本
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            curEventData = eventData;
            curEventDataType = eventDataType.Click;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            curEventData = eventData;
            curEventDataType = eventDataType.Enter;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            curEventData = eventData;
            curEventDataType = eventDataType.Exit;
        }


        /// <summary>
        /// 检测事件
        /// </summary>
        private void chackEventData()
        {
            if (curEventData == null) return;


            Vector2 lp = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, curEventData.position, curEventData.pressEventCamera, out lp);

            foreach (var hrefInfo in m_HrefInfos)
            {
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(lp))
                    {
                        if (curEventDataType == eventDataType.Click)
                        {
                            clickAction?.Invoke();
                            curEventDataType = eventDataType.Exit;
                        }
                        if (curEventDataType == eventDataType.Enter || curEventDataType == eventDataType.None)
                        {
                            enterAction?.Invoke();
                            curEventDataType = eventDataType.Exit;
                        }
                        return;
                    }
                }
            }

            if (curEventDataType == eventDataType.Exit)
            {
                exitAction?.Invoke();
            }

            curEventDataType = eventDataType.None;
        }
    }


}
