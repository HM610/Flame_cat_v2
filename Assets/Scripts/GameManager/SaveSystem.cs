using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブ機能 (シングルトンクラス)
/// </summary>
public class SaveSystem
{
    static SaveSystem instance;
    static public SaveSystem Instance {
        get {
            instance = instance ?? new SaveSystem();
            return instance;
        }
    }

    /// <summary>
    /// セーブデータ
    /// </summary>
    public SaveData Data {
        private set;
        get;
    }

    /// <summary>
    /// 読み書き用
    /// </summary>
    public IReadWriterAsync ReadWriter { 
        private set;
        get;
    }

    /// <summary>
    /// 保存するときのキー
    /// </summary>
    const string Key = "Save";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    SaveSystem()
    {
        Data = new SaveData();
#if UNITY_WEBGL && !UNITY_EDITOR
        ReadWriter = new IndexedDBReadWriter();
#else
        ReadWriter = new FileReadWriter();
#endif
    }

    /// <summary>
    /// データをセーブ
    /// </summary>
    /// <returns>true: 成功, false: 失敗</returns>
    public async UniTask<bool> SaveAsync()
    {
        return await ReadWriter.WriteClassAsync(Key, Data);
    }
    /// <summary>
    /// データをロード
    /// </summary>
    /// <returns>true: 読み込んだデータ, false: default</returns>
    public async UniTask<SaveData> LoadAsync()
    {
        Data = await ReadWriter.ReadeClassAsync<SaveData>(Key);
        return Data;
    }
}
