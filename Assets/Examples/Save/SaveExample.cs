using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class SaveExampleItems
{
    public Image image;
    public InputField score;
    public Button button;
}

public class SaveExample : MonoBehaviour
{
    [SerializeField] SaveExampleItems save;
    [SerializeField] SaveExampleItems load;

    byte[] imageBytes;

    SaveData data;

    const string ImageUrl = "https://1.bp.blogspot.com/-tVeC6En4e_E/X96mhDTzJNI/AAAAAAABdBo/jlD_jvZvMuk3qUcNjA_XORrA4w3lhPkdQCNcBGAsYHQ/s1048/onepiece01_luffy.png";

    void Start()
    {
        save.score.text = "0";

        // Saveボタンを押したとき
        save.button.OnClickAsObservable()
            .Subscribe(_ => {
                Debug.Log("セーブ開始");

                // ① SaveSystemからセーブするクラスのインスタンスを取得
                data = SaveSystem.Instance.Data;

                var score = 0;
                int.TryParse(save.score.text, out score);

                // ② ①で取得したインスタンスを書き換える
                data.Score = score;

                // ③ SaveSystem の SaveAsync() を呼び出す (saveTask関数内で呼び出し)
                saveTask().Forget();

                // 完了を待たない場合は SaveAsync().Forget(); と呼び出し
                // SaveSystem.Instance.SaveAsync().Forget();

                //完了通知だけが必要な場合は下記のようなコードで対応してください
                //SaveSystem.Instance.SaveAsync()
                //    .ToObservable()
                //    .Subscribe(success => {
                //        Debug.Log("セーブ終了 : " + success);
                //    }).AddTo(this);
            }).AddTo(this); ;

        // Loadボタンを押したとき
        load.button.OnClickAsObservable()
            .Subscribe(_ => {
                Debug.Log("ロード開始");
                //④ SaveSystem の LoadAsync() を呼び出す (loadTask関数内で呼び出し)
                // 読み込んだデータは LoadAsync() の戻り値で取得または SaveSystem.Instance.Data; で取得できます
                loadTask().Forget();

                // 読み込みもSaveと同様に完了通知を受けたい場合は SaveAsync を LoadAsync へ変更してください
                // 読み込まれたデータは Subscribe で設定している 関数オブジェクトの引数に入ります 以下の場合は "data" へ読み込まれだデータが入る
                //SaveSystem.Instance.LoadAsync()
                //    .ToObservable()
                //    .Subscribe(data => {
                //        Debug.Log("ロード終了: " + data.Score);
                //    }).AddTo(this);

            }).AddTo(this);

        downloadAndDrawImage().Forget();
    }

    async UniTask saveTask()
    {
        await SaveSystem.Instance.SaveAsync();

        var readWriter = SaveSystem.Instance.ReadWriter;

        // バイトの書き込み
        await readWriter.WriteByteAsync("testImage", imageBytes);
    }


    async UniTask loadTask()
    {
        var data = await SaveSystem.Instance.LoadAsync();
        // data = SaveSystem.Instance.Data;

        load.score.text = data.Score.ToString();

        var readWriter = SaveSystem.Instance.ReadWriter;
        var bytes = await readWriter.ReadByteAsync("testImage");
        drawImage(load.image, bytes);
    }

    async UniTask downloadAndDrawImage()
    {

        imageBytes = await downloadImage();
        drawImage(save.image, imageBytes);
    }

    async UniTask<byte[]> downloadImage()
    {
        var request = new UnityEngine.Networking.UnityWebRequest(ImageUrl);
        request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
        await request.SendWebRequest();
        if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success) {
            return request.downloadHandler.data;
        }
        return null;
    }

    void drawImage(Image image, byte[] data)
    {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(data);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.preserveAspect = true;
    }
}
