using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Youtube;

public class YoutubeCommentActionBase : SingletonMonoBehaviour<YoutubeCommentActionBase>
{
    
    //コメントに対する反応を実行する
    public virtual void applyCommentAction(string comment,string target,bool isSuperChat)
    {        
        if (isSuperChat)
        {
            superChatAction();
        }
        commentAction();
    }
    
    //コメントに対する反応を定義。ここを継承して使う。
    public virtual void commentAction()
    {
        
    }
    
    //スパチャだったときのアクション
    public virtual void superChatAction()
    {
        //処理
    }


}
