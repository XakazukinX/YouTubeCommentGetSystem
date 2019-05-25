using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Youtube
{
	public class YouTubeChannelInfoCheck : SingletonMonoBehaviour<YouTubeChannelInfoCheck>
	{
		[SerializeField] private string channelGetAPIKey;
		[Header("チャンネルのアイコンを使ってすることを決める")]
		[SerializeField] private YouTubeChannelActionBase channelAction;

		
		private Youtube.URIGenerator uriGenerator = null;
		private Youtube.YouTubeChannelJSON.JSON youTubeChannelJson = null;

		[Header("叩いたサムネのURLを見たいとき用")] public bool isDebug;
		
		private void Start()
		{
			uriGenerator = new Youtube.URIGenerator (channelGetAPIKey);
			youTubeChannelJson = new YouTubeChannelJSON.JSON();
		}

		public void channelCheck(string channelId)
		{
			StartCoroutine(channelInfoCheck(channelId));
		}

		private IEnumerator channelInfoCheck(string channelId)
		{
			var channelURI = uriGenerator.GetChannel(channelId);
			UnityWebRequest channelInfoRequest = UnityWebRequest.Get(channelURI);
			yield return channelInfoRequest.SendWebRequest();
			var jsonText = channelInfoRequest.downloadHandler.text;
			string channelThumbnailURL = youTubeChannelJson.GetThumbnailURL(jsonText);

			if (isDebug)
			{
				Debug.Log("さむねのURL : "+channelThumbnailURL);				
			}
			
			if (channelThumbnailURL == null)
			{
				yield break;
			}
			
			using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(channelThumbnailURL))
			{
				yield return request.SendWebRequest();

				if (request.isNetworkError || request.isHttpError)
				{
					Debug.Log(request.error);
				}
				else
				{
					// Get downloaded asset bundle
					var texture = DownloadHandlerTexture.GetContent(request);
					channelAction.channelAction(texture);
				}
			}
			
			yield break;
		}
	}
}
