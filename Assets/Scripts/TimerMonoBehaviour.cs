using UnityEngine;

/// <summary>
/// タイマークラス
/// </summary>
public class TimerMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// Start時に自動起動するか
    /// </summary>
    [SerializeField] bool autoStart = false;

    /// <summary>
    /// タイマーが動いているか
    /// </summary>
    public bool IsStarted { 
        private set;
        get;
    } = false;


    /// <summary>
    /// 経過時間(秒)
    /// </summary>
    public float ElapsedTime {
        private set;
        get;
    } = 0.0f;

    /// <summary>
    /// 経過時間
    /// </summary>
    public int EpalsedTimeInt => (int)ElapsedTime;

    void Start()
    {
        IsStarted = autoStart;
    }

    void Update()
    {
        if (!IsStarted) {
            return;
        }

        ElapsedTime += Time.deltaTime;
    }

    /// <summary>
    /// 経過時間を0に戻し開始する
    /// </summary>
    public void Restart()
    {
        ElapsedTime = 0.0f;
        IsStarted = true;
    }

    /// <summary>
    /// 経過時間をそのままタイマーを再開する
    /// </summary>
    public void Resume() {
        IsStarted = true;
    }

    /// <summary>
    /// タイマーを停止
    /// </summary>
    public void Stop()
    {
        IsStarted = false;
    }

    /// <summary>
    /// 経過時間を0に変更
    /// </summary>
    public void ResetTime()
    {
        ElapsedTime = 0.0f;
    }
}
