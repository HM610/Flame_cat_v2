using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ローカルファイルの読み書きクラス
/// </summary>
public sealed class FileReadWriter : IReadWriterAsync
{
    /// <summary>
    /// 読み書きするディレクトリ
    /// </summary>
    public string FileDirectory {
        set;
        get;
    }

    /// <summary>
    /// 保存するファイルパスの取得
    /// </summary>
    /// <param name="name">ファイル名</param>
    /// <returns>保存パス</returns>
    string GetFilePath(string name) => System.IO.Path.Combine(FileDirectory, name);

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="fileDirectory">読み書きするパス(デフォルト値の場合は `Application.dataPath` が代入されます)</param>
    public FileReadWriter(string fileDirectory = "")
    {
        FileDirectory = fileDirectory;
        if (string.IsNullOrEmpty(fileDirectory)) {
            FileDirectory = Application.dataPath;
        }
    }

    /// <summary>
    /// バイトデータの書き込み
    /// </summary>
    /// <param name="key">ファイル名</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    public async UniTask<bool> WriteByteAsync(string key, byte[] data)
    {
        try {
            await System.IO.File.WriteAllBytesAsync(GetFilePath(key), data);
            return true;
        } catch (Exception e) {
            Debug.LogError("ファイルの読み込みに失敗しました : \n" + e.Message);
        }
        return false;
    }

    /// <summary>
    /// クラスデータの書き込み
    /// </summary>
    /// <param name="key">ファイル名</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    public async UniTask<bool> WriteClassAsync<T>(string key , T data)
    {
        try {
            var json = JsonUtility.ToJson(data);
            await System.IO.File.WriteAllTextAsync(GetFilePath(key), json);
            return true;
        } catch (Exception e) {
            Debug.LogError("ファイルの読み込みに失敗しました : \n" + e.Message);
        }
        return false;
    }

    /// <summary>
    /// バイトデータの読み込み
    /// </summary>
    /// <param name="key">ファイル名</param>
    /// <returns>true: 読み込んだデータ, false: null</returns>
    public async UniTask<byte[]> ReadByteAsync(string key)
    {
        try {
            return await System.IO.File.ReadAllBytesAsync(GetFilePath(key));
        } catch (Exception e) {
            Debug.LogError("ファイルの読み込みに失敗しました : \n" + e.Message);
        }
        return null;
    }

    /// <summary>
    /// テキストデータの読み込み
    /// </summary>
    /// <param name="key">ファイル名</param>
    /// <returns>true: 読み込んだデータ, false: default</returns>
    public async UniTask<T> ReadeClassAsync<T>(string key)
    {
        try {
            var data = await System.IO.File.ReadAllTextAsync(GetFilePath(key));
            return JsonUtility.FromJson<T>(data);
        } catch (Exception e) {
            Debug.LogError("ファイルの読み込みに失敗しました : \n" + e.Message);
        }
        return default;
    }
}
