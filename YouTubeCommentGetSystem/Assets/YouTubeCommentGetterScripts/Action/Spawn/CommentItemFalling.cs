using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentItemFalling : MonoBehaviour
{

	private float _fallingSpeed;
	private Vector3 fallingVector3 = new Vector3(0,0,0);
	
	// Use this for initialization
	void Start ()
	{
		_fallingSpeed = YoutubeCommentCheck.Instance.fallingSpeed;
		fallingVector3.y = _fallingSpeed;
		StartCoroutine(falling());
	}

	private IEnumerator falling()
	{
		while (true)
		{
			transform.position -= fallingVector3;
			yield return null;
		}
	}
}
