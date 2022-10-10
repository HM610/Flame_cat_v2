#if UNITY_WEBGL && !UNITY_EDITOR

using Cysharp.Threading.Tasks;
using UnityEngine;

class IndexedDBReadWriter : IReadWriterAsync
{
    IndexedDB db;

    /// <summary>
    /// データベース名
    /// </summary>
    string dbname{
        set;
        get;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="dbname">データベース名((デフォルト値の場合は `Application.productName` が代入されます))</param>
    public IndexedDBReadWriter(string dbname = "")
    {
        this.dbname = dbname;
        if (string.IsNullOrEmpty(this.dbname)) {
            this.dbname = Application.productName;
        }

        db = new IndexedDB(dbname);
    }

    /// <summary>
    /// データベースを開く
    /// </summary>
    /// <returns></returns>
    public async UniTask<bool> OpenDatabase()
    {
        if (db.IsOpen) {
            return true;
        }
        return await db.OpenDB(dbname);
    }

    /// <summary>
    /// データベースを閉じる
    /// </summary>
    public void CloseDB()
    {
        db.CloseDB();
    }

    /// <summary>
    /// バイトデータの書き込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    public async UniTask<bool> WriteByteAsync(string key, byte[] data)
    {
        var isOpend = db.IsOpen;
        await OpenDatabase();

        var ret = await db.SetBytes(key, data);
        if (!isOpend) {
            db.CloseDB();
        }
        return ret;
    }

    /// <summary>
    /// クラスデータの書き込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="data">書き込むデータ</param>
    /// <returns>true: 成功, false: 失敗</returns>
    public async UniTask<bool> WriteClassAsync<T>(string key , T data)
    {
        var isOpend = db.IsOpen;
        await OpenDatabase();

        var ret = await db.SetObject(key, data);
        if (!isOpend) {
            db.CloseDB();
        }
        return ret;
    }

    /// <summary>
    /// バイトデータの読み込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>true: 読み込んだデータ, false: null</returns>
    public async UniTask<byte[]> ReadByteAsync(string key)
    {
        var isOpend = db.IsOpen;
        await OpenDatabase();

        var ret = await db.GetBytes(key);
        if (!isOpend) {
            db.CloseDB();
        }
        return ret;
    }
    /// <summary>
    /// テキストデータの読み込み
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>true: 読み込んだデータ, false: default</returns>
    public async UniTask<T> ReadeClassAsync<T>(string key)
    {
        var isOpend = db.IsOpen;
        await OpenDatabase();

        var ret = await db.GetObject<T>(key);
        if (!isOpend) {
            db.CloseDB();
        }
        return ret;
    }
}
#endif
