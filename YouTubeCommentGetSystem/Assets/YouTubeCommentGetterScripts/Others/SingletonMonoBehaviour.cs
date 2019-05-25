using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				//そのクラスを検索
				instance = (T)FindObjectOfType(typeof(T));
				//見つからなかったらエラーで通知
				if (instance == null)
				{
					Debug.LogError(typeof(T).Name + "が存在してないよ！");
				}
			}
			//シーンに存在しているのであればインスタンスを返す
			return instance;
		}
		set
		{
			
		}
	}

	protected void Awake()
	{
		isExistInstance();
	}

	protected bool isExistInstance()
	{
		if (this == Instance)
		{
			return true;
		}
		else
		{
			Destroy(this);
			return false;
		}

	}

}