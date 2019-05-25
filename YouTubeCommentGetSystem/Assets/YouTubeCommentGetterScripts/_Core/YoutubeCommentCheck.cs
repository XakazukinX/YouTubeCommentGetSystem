using System.Collections;
using System.Collections.Generic;
using shigeno_EditorUtility;
using UnityEngine;
using Youtube;

public class YoutubeCommentCheck : SingletonMonoBehaviour<YoutubeCommentCheck>
{
	[Header("落ちものするときようのfloat")]
	public float fallingSpeed = 0.03f;

	[System.Serializable]
	public class CommentActionSettings
	{
		[CustomLabel("反応するコメント")]
		public string targetComment;
		[CustomLabel("コメントへの反応")]
		public YoutubeCommentActionBase _commentAction;
	}
	
	[SerializeField] public List<CommentActionSettings> commentActionSettings = new List<CommentActionSettings>();
	
	[CustomLabel("全てのスパチャへの反応")]
	[SerializeField] private YoutubeSuperChatActionBase superChatAction;
	
	
	[Header("チャンネル情報を取得したいとき用")] 
	public bool isUseChannelInfo = false;

	[Header("取得項目のチェック用")]
	public bool isDebug = false;
	
	
	//コメントチェックのコルーチンを実行する
	internal void startCommentCheck(JSON.CommentInfo youtubeCommentInfo)
	{
		StartCoroutine(commentChecker(youtubeCommentInfo));
	}
    
	
    
	//コルーチンでコメントをチェックする。
	private IEnumerator commentChecker(JSON.CommentInfo _commentInfo)
	{
		var comment = _commentInfo.Message;
		var isSuperChat = false;
		
		if (isDebug)
		{
			Debug.Log("check:" + comment);			
		}

		if (isUseChannelInfo)
		{
			YouTubeChannelInfoCheck.Instance.channelCheck(_commentInfo.AuthorId);
		}

		if (YouTubeCommentType.isSuperChat(_commentInfo.Type))
		{
			superChatAction.superChatAction();
			isSuperChat = true;
		}

		for (int i = 0; i < commentActionSettings.Count; i++)
		{
			string _target = commentActionSettings[i].targetComment;
			if (comment.Contains(_target))
			{
				commentActionSettings[i]._commentAction.applyCommentAction(comment,_target,isSuperChat);
				yield break; 
			}
		}

		yield break;
	}
	
	
}
