using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Youtube
{
    public class YouTubeChannelJSON : MonoBehaviour
    {
        [Serializable]
        public class ChannelURIResponse
        {
            public ChannelResponseDetail[] items = null;
        }


        [Serializable]
        public class ChannelResponseDetail
        {
	        public ChannelThumbnailInfo tumbnails = null;
            public ChannelSnippet snippet = null;
	        public string kind = String.Empty;
        }

        [Serializable]
        public class ChannelSnippet
        {
            //多分チャンネル名
            public string title = String.Empty;
            public ChannelThumbnailInfo thumbnails = null;
        }
        
        
        [Serializable]
        public class ChannelThumbnailInfo
        {
            //変数名としてdefaultは使えないのでコメントアウト
/*            public ChannelThumbnail default  = null;*/
            public ChannelThumbnail medium  = null;
            public ChannelThumbnail high  = null;
        }
        
        [Serializable]
        public class ChannelThumbnail
        {
            public string url = String.Empty;
            public int width = 0;
            public int height = 0;
        }

	    public class JSON
	    {
		    public string GetThumbnailURL(string jsonText)
		    {
			    var obj = JsonUtility.FromJson<ChannelURIResponse>(jsonText);
			    if (obj == null)
			    {
				    return null;
			    }

			    var items = obj.items;
			    if (items == null)
			    {
				    return null;
			    }
			    
			    Debug.Log(jsonText);
			    Debug.Log(items[0]);

			    return items[0].snippet.thumbnails.medium.url;
		    }
	    }

    }
}

