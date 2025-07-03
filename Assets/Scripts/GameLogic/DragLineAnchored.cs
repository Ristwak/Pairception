// DragLineAnchored.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragLineAnchored : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Setup")]
    public RectTransform lineContainer;  // Full-board RectTransform
    public float thickness = 4f;
    public Color defaultColor = Color.gray;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private RectTransform _lineRect;
    private Image _lineImg;
    private Vector3 _startWorld;
    private QuestionBox _qb;

    private void Awake()
    {
        _qb = GetComponent<QuestionBox>();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (_qb.answered) return;
        if (_qb.currentLineRect != null) Destroy(_qb.currentLineRect.gameObject);

        var go = new GameObject("LineUI", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(lineContainer, false);
        _lineRect = go.GetComponent<RectTransform>();
        _lineImg = go.GetComponent<Image>();
        _lineImg.color = defaultColor;
        _lineRect.pivot = new Vector2(0, 0.5f);

        // calculate start at question border
        var qRect = GetComponent<RectTransform>();
        Camera cam = data.pressEventCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            qRect, data.position, cam, out Vector2 qLocal);
        Vector2 qBorder = GetBorderPoint(qRect, qLocal);
        _startWorld = qRect.TransformPoint(qBorder);

        _lineRect.position = _startWorld;
        _lineRect.sizeDelta = new Vector2(0, thickness);
        _qb.currentLineRect = _lineRect;
    }

    public void OnDrag(PointerEventData data)
    {
        if (_lineRect == null) return;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            lineContainer, data.position, data.pressEventCamera, out var worldPos);
        var dir = worldPos - _startWorld;
        float len = dir.magnitude;
        _lineRect.sizeDelta = new Vector2(len, thickness);
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _lineRect.rotation = Quaternion.Euler(0, 0, ang);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (_lineRect == null) return;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        Camera cam = data.pressEventCamera;

        foreach (var r in results)
        {
            var ab = r.gameObject.GetComponent<AnswerBox>();
            if (ab != null)
            {
                var aRect = r.gameObject.GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    aRect, r.screenPosition, cam, out Vector2 aLocal);
                Vector2 aBorder = GetBorderPoint(aRect, aLocal);
                Vector3 worldEnd = aRect.TransformPoint(aBorder);

                var delta = worldEnd - _startWorld;
                float len = delta.magnitude;
                _lineRect.sizeDelta = new Vector2(len, thickness);
                _lineRect.rotation = Quaternion.Euler(0, 0,
                    Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);

                // Save match — DO NOT evaluate yet
                _lineImg.color = defaultColor;
                _qb.answered = true;
                _qb.matchedAnswerText = ab.answerText;
                break;
            }
        }

        _lineRect = null;
    }

    // returns the point on the rect border nearest the local point direction
    private Vector2 GetBorderPoint(RectTransform rect, Vector2 localPt)
    {
        Rect r = rect.rect;
        float halfW = r.width * 0.5f;
        float halfH = r.height * 0.5f;
        float dx = localPt.x, dy = localPt.y;
        if (Mathf.Approximately(dx, 0) && Mathf.Approximately(dy, 0))
            return Vector2.zero;
        float absX = Mathf.Abs(dx), absY = Mathf.Abs(dy);
        Vector2 border;
        if (absX / halfW > absY / halfH)
        {
            float signX = Mathf.Sign(dx);
            border.x = signX * halfW;
            border.y = dy * (halfW / absX);
        }
        else
        {
            float signY = Mathf.Sign(dy);
            border.y = signY * halfH;
            border.x = dx * (halfH / absY);
        }
        return border;
    }
}