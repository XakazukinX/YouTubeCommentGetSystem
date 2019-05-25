using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Youtube;

public class ChannelIconContainerAction : YouTubeChannelActionBase 
{

    [SerializeField] private GameObject[] channelIconContainers;

    public override void channelAction(Texture getTexture)
    {
        for (int i = 0; i < channelIconContainers.Length; i++)
        {
            if (!channelIconContainers[i].activeSelf)
            {
                channelIconContainers[i].SetActive(true);
                channelIconContainers[i].GetComponent<Renderer>().material.mainTexture = getTexture;
                break;
            }
        }
    }

}
