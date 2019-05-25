﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Youtube {
	//コメント情報クラス

	[System.Serializable]
	class ChatURIResponse {
		public ChatResponseItem[] items = null;
	}

	[System.Serializable]
	class ChatResponseItem {
		public string kind = string.Empty;
		public string id = null;
		public ChatResponseDetail liveStreamingDetails = null;
	}

	[System.Serializable]
	class ChatResponseDetail {
		public string actualStartTime = string.Empty;
		public string scheduledStartTime = string.Empty;
		public string scheduledEndTime = string.Empty;
		public string concurrentViewers = string.Empty;
		public string activeLiveChatId = string.Empty;
	}

	[System.Serializable]
	class VieoURIResponse {
		public string kind = string.Empty;
		public string etag = string.Empty;
		public VideoResponseItem[] items = null;
	}

	[System.Serializable]
	class VideoResponseItem {
		public string kind = string.Empty;
		public VideoIdItem id = null;
	}

	[System.Serializable]
	class VideoIdItem {
		public string kind = string.Empty;
		public string videoId = string.Empty;
	}

	[System.Serializable]
	public class CommentResponse {
		public string kind = string.Empty;
		public string nextPageToken = string.Empty;
		public PageInfoResponse pageInfo = null;
		public CommentResponseItem[] items = null;
	}

	[System.Serializable]
	public class CommentResponseItem {
		public string kind = string.Empty;
		public string id = string.Empty;
		public CommentsResponseSnippet snippet = null;
		public CommentAutherDetail authorDetails = null;
	}

	[System.Serializable]
	public class PageInfoResponse {
		public int totalResults = 0;
		public int resultsPerPage = 0;
	}

	[System.Serializable]
	public class CommentsResponseSnippet
	{
		public string type = string.Empty;
		public string authorChannelId = string.Empty;
		public string publishedAt = string.Empty;
		public string displayMessage = string.Empty;
	}

	[System.Serializable]
	public class CommentAutherDetail {
		public string channelId = string.Empty;
		public string displayName = string.Empty;
		public bool isChatOwner = false;
	}
	public class JSON {
		public class CommentInfo {
			public bool IsOwnerMessage;
			public string DisplayName;
			public string Message;
			public string Type;
			public string AuthorId;
			public CommentInfo (bool IsOwner, string name, string message,string type,string authorChannelId) {
				IsOwnerMessage = IsOwner;
				DisplayName = name;
				Message = message;
				Type = type;
				AuthorId = authorChannelId;

			}
			public int commentIdHash;
		}
		///JSONからChatIdをえる
		///異常があった場合にはから文字列を返す
		public static string GetChatId (string jsonText) {
			var ret = string.Empty;
			var obj = JsonUtility.FromJson<ChatURIResponse> (jsonText);
			if (obj == null || obj.items == null || obj.items.Length < 1 || obj.items[0].liveStreamingDetails == null) {
				Debug.LogWarning ("チャンネルがライブ中ではないのではありませんか？");
				return ret;
			}
			return obj.items[0].liveStreamingDetails.activeLiveChatId;
		}
		//JSON文字列からVideoIdを取得して返す
		//見つからなかった場合String.Emptyを返す
		public static string GetVideoId (string jsonText) {
			var ret = string.Empty;
			var obj = JsonUtility.FromJson<VieoURIResponse>
				(jsonText);
			if (null == obj) {
				Debug.LogWarning ("チャンネルがライブ中ではないのではありませんか？");
				return ret;
			}
			if (obj.items == null || obj.items.Length < 1) {
				Debug.LogWarning ("チャンネルがライブ中ではないのではありませんか？");
				return ret;
			}
			return obj.items[0].id.videoId;
		}

		public static IList<CommentInfo> GetComments (string jsonText, string nextPageTokenstr, out string nextToken) {
			List<CommentInfo> ret = new List<CommentInfo> ();
			nextToken = string.Empty;
			var obj = JsonUtility.FromJson<CommentResponse> (jsonText);
			if (obj == null) {
				return ret;
			}
			nextToken = obj.nextPageToken;
			var pi = obj.pageInfo;
			if (null == pi) {
				return ret;
			}
			var items = obj.items;
			if (null == items) {
				return ret;
			}
			foreach (var item in items) {
				var name = item.authorDetails.displayName;
				var isOwner = item.authorDetails.isChatOwner;
				var msg = item.snippet.displayMessage;
				var _type = item.snippet.type;
				var _authorId = item.authorDetails.channelId;
				if (!string.IsNullOrEmpty (name) && !string.IsNullOrEmpty (msg)) {
					var ci = new CommentInfo (isOwner, name, msg , _type,_authorId);
					//重複をなんとなく避けるための手がかりを付与
					ci.commentIdHash = item.id.GetHashCode ();
					ret.Add (ci);
				}
			}
			return ret;
		}
	}
}