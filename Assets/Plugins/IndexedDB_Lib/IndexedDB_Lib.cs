#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// WebGL値保存用IndexedDBクラス
/// </summary>
public class IndexedDB
{
    [DllImport("__Internal")]
    private static extern void OpenIndexedDB(string dbname);
    [DllImport("__Internal")]
    private static extern void CloseIndexedDB();
    [DllImport("__Internal")]
    private static extern void ClearDB();
    [DllImport("__Internal")]
    private static extern void SetBytesRequest(string key, byte[] bytes, int size);
    [DllImport("__Internal")]
    private static extern void GetBytesRequest(string key);
    [DllImport("__Internal")]
    private static extern IntPtr GetBytes();
    [DllImport("__Internal")]
    private static extern void FreeBytes(IntPtr ptr);
    [DllImport("__Internal")]
    private static extern void SetJsonRequest(string key, string json);
    [DllImport("__Internal")]
    private static extern void GetJsonRequest(string key);
    [DllImport("__Internal")]
    private static extern string GetJson();
    [DllImport("__Internal")]
    private static extern bool IsFailed();
    [DllImport("__Internal")]
    private static extern bool InProgress();

    public IndexedDB(string dbname = "")
    {
        this.dbname = dbname;
    }

    /// <summary>
    /// プロセス完了待機
    /// </summary>
    /// <returns></returns>
    async UniTask<bool> waitProcess()
    {
        await UniTask.WaitWhile(InProgress);
        return !IsFailed();
    }

    /// <summary>
    /// データベースが開いているかのマップ
    /// </summary>
    static Dictionary<string, bool> isOpenMap = new Dictionary<string, bool>();

    /// <summary>
    /// DBを開いているか
    /// </summary>
    public bool IsOpen {
        private set {
            isOpenMap[dbname] = value;
        }
        get {
            if (!isOpenMap.ContainsKey(dbname)) {
                return false;
            }
            return isOpenMap[dbname];
        }
    }

    /// <summary>
    /// データベース名
    /// </summary>
    string dbname = "";

    /// <summary>
    /// IndexedDBを開く
    /// </summary>
    /// <param name="name">DB名</param>
    public async UniTask<bool> OpenDB(string name)
    {
        OpenIndexedDB(name);
        var result = await waitProcess();
        IsOpen = !IsFailed();
        if (!result) {
            Debug.LogError("Failed IndexedDB open");
        }
        return result;

    }

    /// <summary>
    /// IndexedDBを閉じる
    /// </summary>
    public void CloseDB()
    {
        CloseIndexedDB();
        IsOpen = false;
    }

    /// <summary>
    /// 開いているDBを空にする
    /// </summary>
    public async UniTask<bool> Clear()
    {
        ClearDB();

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB clear");
        }
        return result;
    }

    /// <summary>
    /// バイナリデータをIndexedDBへ設定
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="bytes">バイナリデータ</param>
    public async UniTask<bool> SetBytes(string key, byte[] bytes)
    {
        SetBytesRequest(key, bytes, bytes.Length);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB clear");
        }
        return result;
    }

    /// <summary>
    /// IndexedDBからキーのバイナリデータを取得
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>バイナリデータ</returns>
    public async UniTask<byte[]> GetBytes(string key)
    {
        GetBytesRequest(key);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB GetBytes");
            return null;
        }

        var ptr = GetBytes();
        var size = Marshal.ReadInt32(ptr);
        var ret = new byte[size];
        Marshal.Copy(IntPtr.Add(ptr, 4), ret, 0, size); // 最初の4バイトは配列サイズのため4から始める
        FreeBytes(ptr);
        return ret;
    }

    /// <summary>
    /// JsonデータをIndexedDBへ設定
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="json">Jsonデータ</param>
    public async UniTask<bool> SetJson(string key, string json)
    {
        SetJsonRequest(key, json);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB SetJson");
        }
        return result;
    }

    /// <summary>
    /// IndexedDBからキーのJsonデータを取得
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>Jsonデータ</returns>
    public async UniTask<string> GetJson(string key)
    {
        GetJsonRequest(key);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB GetJson");
            return null;
        }

        return GetJson();
    }

    /// <summary>
    /// オブジェクトをIndexedDBへ保存
    /// </summary>
    /// <remarks>
    /// objはJsonUtilityを使用してJson形式へ変更してJS側へ送られます
    /// </remarks>
    /// <param name="key">キー</param>
    /// <param name="obj">保存するデータ</param>
    public async UniTask<bool> SetObject<T>(string key, T obj)
    {
        var json = JsonUtility.ToJson(obj);
        if (string.IsNullOrEmpty(json)) {
            Debug.LogError("Faild SetObject convet to json");
            return false;
        }
        SetJsonRequest(key, json);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB SetObject");
        }
        return result;
    }

    /// <summary>
    /// IndexedDBからキーのデータを取得
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>データ</returns>
    public async UniTask<T> GetObject<T>(string key)
    {
        GetJsonRequest(key);

        var result = await waitProcess();
        if (!result) {
            Debug.LogError("Failed IndexedDB GetJson");
            return default;
        }

        var json = GetJson();
        return JsonUtility.FromJson<T>(json);
    }
}
#endif
