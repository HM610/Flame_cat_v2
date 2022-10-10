using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データ書き込み用インターフェース
/// </summary>
public interface IReadWriterAsync
{
    /// <summary>
    /// バイトデータの書き込み
    /// </summary>
    /// <param name="key">書き込むキー</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    UniTask<bool> WriteByteAsync(string key, byte[] data);

    /// <summary>
    /// クラスデータの書き込み
    /// </summary>
    /// <param name="key">書き込むキー</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    UniTask<bool> WriteClassAsync<T>(string key , T data);

    /// <summary>
    /// バイトデータの読み込み
    /// </summary>
    /// <param name="key">読み込むキー</param>
    /// <returns>true: 読み込んだデータ, false: null</returns>
    UniTask<byte[]> ReadByteAsync(string key);

    /// <summary>
    /// クラスデータの読み込み
    /// </summary>
    /// <param name="key">読み込むキー</param>
    /// <returns>true: 読み込んだデータ, false: null</returns>
    UniTask<T> ReadeClassAsync<T>(string key);
}
