using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelIconFalling : MonoBehaviour 
{  
    private float _fallingSpeed;
    private Vector3 fallingVector3 = new Vector3(0,0,0);

    private Vector3 spawnPos;
	
    private void Start ()
    {
        _fallingSpeed = YoutubeCommentCheck.Instance.fallingSpeed;
        fallingVector3.y = _fallingSpeed;

        spawnPos = transform.position;
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(falling());
    }

    private void OnDisable()
    {
        transform.position = spawnPos;
    }


    private IEnumerator falling()
    {
        while (true)
        {
            transform.position -= fallingVector3;

            if (transform.position.y < 0)
            {
                reset();
                break;
            }
            
            yield return null;
        }
    }

    private void reset()
    {
        
        StopCoroutine(falling());
        gameObject.SetActive(false);
    }
    
    
    
    
    
    
    
    
    
    
}
