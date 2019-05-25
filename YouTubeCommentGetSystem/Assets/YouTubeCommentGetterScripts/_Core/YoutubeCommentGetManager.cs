using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using shigeno_EditorUtility;
using UnityEngine;
using UnityEngine.Networking;
using Youtube;

public class YoutubeCommentGetManager : MonoBehaviour
{

    [CustomLabel("コメント取得を開始するキー")]
    [SerializeField] private KeyCode CommentGetStartKey = KeyCode.F5;
    ///APIKey。GameObjectからセットする想定
    [SerializeField]
    private string apiKey = "!PleaseSetYourAPIKey!";
    ///ChannelID。GameObjectからセットする想定
    [SerializeField]
    private string focusCannelId = "!PleaseSetYourChannel!";
    
    ///コメント取得間隔(秒)
    [SerializeField]
    float commentFetchInterval = 5.0f;
    bool doFetchComment = true;
    Youtube.URIGenerator uriGenerator = null;

    private int targetCommentCount = 0;

    private bool isInitFetch = true;

    [Header("WebRequestしたURIを見たいとき用")]
    public bool isDebug;
    
    
    void Start ()
    {   
        targetCommentCount = YoutubeCommentCheck.Instance.commentActionSettings.Count;
    }

    private void Update()
    {
        //ファンクションキーを押すとコメント取得を開始する。
        //YouTubeAPIの仕様上、生放送が始まってからしばらくたたないとコメント取得を開始できないので
        //任意のタイミングで実行できるようにしてある。
        if (Input.GetKeyUp(CommentGetStartKey))
        {
            isInitFetch = true;
            StartCoroutine (ProcessSearchVideoId ());            
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            resetTargetCommentCount();
        }
    }


    ///指定チャンネルのVIDEOIDをしぼりこむプロセス
    private IEnumerator ProcessSearchVideoId () {
        uriGenerator = new Youtube.URIGenerator (apiKey);
        var searchURI = uriGenerator.GetSearch (focusCannelId);
        Debug.Log ("Connect to: " + searchURI);
        UnityWebRequest liverequest = UnityWebRequest.Get (searchURI);
        ///取得をまつ
        yield return liverequest.SendWebRequest ();

        if (liverequest.isNetworkError)
        {
            Debug.LogError("ネットワーク鰓");
        }
        //エラー
        if (liverequest.isHttpError || liverequest.isNetworkError) {
            Debug.LogError ("Youtube APIの取得に失敗しました。");
            Debug.LogWarning ("Hint: ChannelIDやAPIKeyは設定しましたか？");
            Debug.Log (liverequest.error);
            yield break;
        } else { //成功
            var jsonText = liverequest.downloadHandler.text;
            var videoId = JSON.GetVideoId (jsonText);
            if (string.IsNullOrEmpty (videoId)) {
                Debug.LogError ("VideoIdの取得に失敗しました");
                yield break;
            }
            StartCoroutine (ProcessChatIdFromVideoId (videoId));
        }
    }
    ///ビデオIDからチャットIDをしぼりこむプロセス
    private IEnumerator ProcessChatIdFromVideoId (string videoId) {
        StopCoroutine (ProcessSearchVideoId ());
        var videoURI = uriGenerator.GetVideo (videoId);
        Debug.Log ("GetChat from : " + videoURI);
        UnityWebRequest channelrequest = UnityWebRequest.Get (videoURI);
        yield return channelrequest.SendWebRequest ();
        var jsonText = channelrequest.downloadHandler.text;
        var chatId = JSON.GetChatId (jsonText);
        if (string.IsNullOrEmpty (chatId)) {
            Debug.LogError ("チャットIDの取得に失敗しました");
            yield break;
        }
        Debug.Log(chatId);
        StartCoroutine (ProcessComment (chatId));
    }
    ///コメントを取得しつづけるプロセス
    private IEnumerator ProcessComment (string chatId) 
    {
        var nextPageTokenstr = string.Empty;
        while (doFetchComment) 
        {
            yield return new WaitForSeconds (commentFetchInterval);
            var chatURI = uriGenerator.GetLiveChat (chatId, nextPageTokenstr);
            if (isDebug)
            {
                Debug.Log (chatURI);                
            }
            UnityWebRequest connectChatrequest = UnityWebRequest.Get(chatURI);
            yield return connectChatrequest.SendWebRequest ();
            var jsonText = connectChatrequest.downloadHandler.text;
            var token = nextPageTokenstr;
            var comments = JSON.GetComments(jsonText, token, out nextPageTokenstr);
           //Debug.Log(comments.Count);
            
            Debug.Log("YouTubeコメント取得は正常に動作しています。");

            if (isInitFetch)
            {
                isInitFetch = false;
                continue;
            }            
            
            if (comments.Count != 0)
            {
                for (int i = 0; i < comments.Count; i++)
                {
                    YoutubeCommentCheck.Instance.startCommentCheck(comments.ToList()[i]);
                }
            }
            
            Debug.Log(gameObject.name+"/" + comments.Count);
            
        }    
    }
    
    
    
    
    //実行中に反応コメントとか増やした時用
    private void resetTargetCommentCount()
    {
        targetCommentCount = YoutubeCommentCheck.Instance.commentActionSettings.Count;
    }
    
}
