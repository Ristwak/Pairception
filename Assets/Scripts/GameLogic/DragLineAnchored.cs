using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragLineAnchored : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Settings")]
    public float thickness = 4f;
    public Color defaultColor = Color.gray;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private RectTransform _lineRect;
    private Image _lineImg;
    private Vector3 _startWorld;
    private QuestionBox _qb;
    private Transform _canvasTransform;

    private void Awake()
    {
        _qb = GetComponent<QuestionBox>();

        // Cache the canvas transform once
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            _canvasTransform = canvas.transform;
        }
        else
        {
            Debug.LogError("Canvas not found in parent hierarchy.");
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (_qb.answered) return;
        if (_qb.currentLineRect != null)
            Destroy(_qb.currentLineRect.gameObject);

        // Create line UI element directly under the canvas
        GameObject go = new GameObject("LineUI", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(_canvasTransform, false);
        go.transform.SetAsLastSibling(); // Draw above everything

        _lineRect = go.GetComponent<RectTransform>();
        _lineImg = go.GetComponent<Image>();
        _lineImg.color = defaultColor;
        _lineRect.pivot = new Vector2(0, 0.5f);

        // Calculate starting position at edge of question circle
        if (_qb.circleAnchor != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasTransform as RectTransform, data.position, data.pressEventCamera, out var pointerWorld);
            _startWorld = GetEdgePosition(_qb.circleAnchor, pointerWorld);
        }
        else
        {
            _startWorld = transform.position;
        }

        _lineRect.position = _startWorld;
        _lineRect.sizeDelta = new Vector2(0, thickness);
        _qb.currentLineRect = _lineRect;
    }

    public void OnDrag(PointerEventData data)
    {
        if (_lineRect == null || _qb.circleAnchor == null)
            return;

        // 1) get pointer in world coords
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _canvasTransform as RectTransform,
            data.position,
            data.pressEventCamera,
            out Vector3 pointerWorld);

        // 2) see if we're over an AnswerBox right now...
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        foreach (var r in results)
        {
            var ab = r.gameObject.GetComponent<AnswerBox>();
            if (ab != null && ab.circleAnchor != null)
            {
                // clamp end to the *left* edge of the answer circle
                Vector3 dirToStart = (_startWorld - ab.circleAnchor.position).normalized;
                float answerRadius = ab.circleAnchor.rect.width * 0.5f * ab.circleAnchor.lossyScale.x;
                Vector3 endWorld = ab.circleAnchor.position + dirToStart * answerRadius;

                // draw that final segment
                Vector3 delta = endWorld - _startWorld;
                float len = delta.magnitude;
                _lineRect.position = _startWorld;
                _lineRect.sizeDelta = new Vector2(len, thickness);
                _lineRect.rotation = Quaternion.Euler(0, 0,
                    Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);

                // immediately finalize the drag
                OnEndDrag(data);
                return;
            }
        }

        // 3) if we're *not* over an AnswerBox, do your normal
        //    “follow the finger but stop at the start‐circle edge” logic:
        Vector3 start = GetEdgePosition(_qb.circleAnchor, pointerWorld);
        float startRadius = _qb.circleAnchor.rect.width * 0.5f * _qb.circleAnchor.lossyScale.x;
        Vector3 dir = (pointerWorld - start).normalized;
        Vector3 end = pointerWorld - dir * startRadius;

        Vector3 d = end - start;
        float length = d.magnitude;
        _lineRect.position = start;
        _lineRect.sizeDelta = new Vector2(length, thickness);
        _lineRect.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (_lineRect == null) return;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (var r in results)
        {
            var ab = r.gameObject.GetComponent<AnswerBox>();
            if (ab != null && ab.gameObject != gameObject)
            {
                Vector3 endWorld;

                if (ab.circleAnchor != null)
                {
                    // Use _startWorld as reference to stop at edge of the answer circle
                    endWorld = GetEdgePosition(ab.circleAnchor, _startWorld);
                }
                else
                {
                    endWorld = ab.transform.position;
                }

                // Update line
                Vector3 delta = endWorld - _startWorld;
                float len = delta.magnitude;
                _lineRect.position = _startWorld;
                _lineRect.sizeDelta = new Vector2(len, thickness);
                _lineRect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);

                // Save match
                _lineImg.color = defaultColor;
                _qb.answered = true;
                _qb.matchedAnswerText = ab.answerText;

                // Change both circles to black
                if (_qb.circleAnchor != null)
                {
                    var img = _qb.circleAnchor.GetComponent<Image>();
                    if (img != null) img.color = Color.black;
                }
                if (ab.circleAnchor != null)
                {
                    var img = ab.circleAnchor.GetComponent<Image>();
                    if (img != null) img.color = Color.black;
                }

                break;
            }
        }

        _lineRect = null;
    }

    /// <summary>
    /// Returns a point on the edge of the given circle facing the target.
    /// </summary>
    private Vector3 GetEdgePosition(RectTransform rect, Vector3 toward)
    {
        Vector3 center = rect.position;
        Vector3 dir = (toward - center).normalized;

        float width = rect.rect.width * rect.lossyScale.x * 0.5f;
        float height = rect.rect.height * rect.lossyScale.y * 0.5f;

        // Clamp direction to edge
        float dx = dir.x;
        float dy = dir.y;

        float scaleX = Mathf.Abs(1f / dx);
        float scaleY = Mathf.Abs(1f / dy);

        float scale = Mathf.Min(scaleX * width, scaleY * height);

        Vector3 edgeOffset = new Vector3(dx, dy) * scale;

        return center + edgeOffset;
    }
}
