using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲット追跡用クラス
/// </summary>
public class FollowTarget : MonoBehaviour
{
    /// <summary>
    /// 追跡するターゲット
    /// </summary>
    public Transform Target {
        set => target = value;
        get => target;
    }
    [SerializeField] Transform target;

    /// <summary>
    /// ターゲットからの相対ポジション
    /// </summary>
    [SerializeField]
    public Vector3 RelativePosition {
        set => relativePosition = value;
        get => relativePosition;
    }
    [SerializeField] Vector3 relativePosition = new Vector3(0.0f, 0.0f, -10.0f);

    /// <summary>
    /// カメラの移動可能範囲
    /// </summary>
    /// <remarks>
    /// Nanを設定されている場合は制限なし
    /// </remarks>
    [SerializeField]
    Rect MovableRect {
        set => movableRect = value;
        get => movableRect;
    }
    [SerializeField] Rect movableRect = new Rect(float.NaN, float.NaN, float.NaN, float.NaN);

    void Update()
    {
        if (target == null) {
            return;
        }

        var newPosition = target.position + relativePosition;

        newPosition.x = Mathf.Clamp(newPosition.x, movableRect.xMin, movableRect.xMax);
        newPosition.y = Mathf.Clamp(newPosition.y, movableRect.yMin, movableRect.yMax);
        transform.position = newPosition;
    }
}
