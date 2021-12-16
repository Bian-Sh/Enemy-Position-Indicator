using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此管理器必须放在 Canvas 节点下
/// Todo : 世界坐标的画布暂不支持
/// </summary>
public class PositionIndicator : MonoBehaviour
{
    public Transform arrow;
    public float offsetCenterPos = 0.5f;
    private Transform indicatorRoot;
    public Transform target;
    private Camera m_Camera;
    private Canvas canvas;
    private Vector2 center = Vector2.one * 0.5f;
    private bool IsVisable => ValidateVisableState();
    private float z_Axis;
    private bool ValidateVisableState()
    {
        var visable = false;
        if (target)
        {
            var bounds = target.GetComponent<Renderer>().bounds;
            var panels = GeometryUtility.CalculateFrustumPlanes(m_Camera);
            visable = GeometryUtility.TestPlanesAABB(panels, bounds);
        }
        return visable;
    }

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        m_Camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : canvas.worldCamera;
        z_Axis = canvas.worldCamera?.ScreenToWorldPoint(arrow.position).z ?? 0;
        indicatorRoot = arrow.parent;
        arrow.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置指示器追踪的游戏对象
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        if (target)
        {
            this.target = target;
            CaculateArrowPose();
            arrow.gameObject.SetActive(true);
        }
    }

    private void CaculateArrowPose()
    {
        if (!arrow.gameObject.activeInHierarchy)
        {
            arrow.gameObject.SetActive(true);
        }
        var pos = (Vector2)m_Camera.WorldToViewportPoint(this.target.position);
        var direction = pos - center;
        var radius = Mathf.Clamp(Vector3.Magnitude(direction), 0.0f, Mathf.Clamp(offsetCenterPos, 0.0f, 0.5f));
        pos = new Vector2
        {
            x = direction.normalized.x,
            y = direction.normalized.y
        };
        var angle = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, pos));
        indicatorRoot.localEulerAngles = angle;
        pos = center + direction.normalized * radius;
        arrow.position = new Vector3
        {
            x = pos.x * canvas.pixelRect.width * canvas.transform.localScale.x,
            y = pos.y * canvas.pixelRect.height * canvas.transform.localScale.x,
            z =z_Axis
    };
    }

    void Update()
    {
        if (this.target && !IsVisable)
        {
            CaculateArrowPose();
        }
        else if (arrow.gameObject.activeInHierarchy)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        canvas = GetComponentInParent<Canvas>();
        if (!canvas)
        {
            Debug.LogError($"{nameof(PositionIndicator)}: 请放在 Canvas 节点之下 ！");
            DestroyImmediate(this);
        }
    }
}
