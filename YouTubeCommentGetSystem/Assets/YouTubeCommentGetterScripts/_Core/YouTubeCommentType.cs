using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Youtube
{
    public class YouTubeCommentType {

        enum CommentTypes
        {
            textMessageEvent,
            superChatEvent
        }

        
        public static bool isSuperChat(string commentType)
        {
            if (commentType == "superChatEvent")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    

}

