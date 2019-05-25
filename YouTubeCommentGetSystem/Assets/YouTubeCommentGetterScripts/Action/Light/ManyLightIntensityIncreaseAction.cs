using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyLightIntensityIncreaseAction : YoutubeCommentActionBase
{

	//コメントと対象となるLightの定義
	[System.Serializable]
	public class TargetLightSetting
	{
		public string targetComment;
		public Light[] targetLight;
		internal float defIntensity;

		internal void addIntensity(float addValue,float maxValue)
		{
			for (int i = 0; i < targetLight.Length; i++)
			{
				var increasedIntensity = Mathf.Clamp(targetLight[i].intensity + addValue, 0, maxValue);
				targetLight[i].intensity = increasedIntensity;
			}
		}
		
		internal void resetIntensity()
		{
			for (int i = 0; i < targetLight.Length; i++)
			{
				targetLight[i].intensity = defIntensity;
			}
		}
		
	}
	[Header("対象コメントと対応するLightの設定")]
	[SerializeField] private List<TargetLightSetting> _targetLightSettings = new List<TargetLightSetting>();
	
	[Header("Intensityを増加させる量")]    
	[SerializeField] private float increaseValue = 0.1f;
	[Header("Intensityのデフォルト値を設定")] 
	[SerializeField] internal float intensityDef = 1.0f;
	[Header("Intensityの最大値を設定")] 
	[SerializeField] internal float intensityMax = 2.0f;

	private int targetLightCount;

    
	[Header("Intensityをリセットするキー")]
	[SerializeField] private KeyCode resetKey;

	
	
	//SpawnSettingの内容をそのまま辞書にしたもの
	private Dictionary<string, TargetLightSetting> lightDictionary;
   
    
	private void Awake()
	{
		targetLightCount = _targetLightSettings.Count;
		
		//辞書の初期化
		lightDictionary = new Dictionary<string, TargetLightSetting>();
        
		for (int i = 0; i < targetLightCount; i++)
		{
			var _targetComment = _targetLightSettings[i].targetComment;
			var _targetLight = _targetLightSettings[i].targetLight;
			lightDictionary.Add(_targetComment,_targetLightSettings[i]);
		}

		
		//各Lightの初期値を入れたりIdxを保持させたりする。
		for (int i = 0; i < targetLightCount; i++)
		{
			_targetLightSettings[i].defIntensity = intensityDef;
			_targetLightSettings[i].resetIntensity();
		}
        
	}


	private void Update()
	{
		//初期化のキー乳力を受付
		if (Input.GetKeyDown(resetKey))
		{
			resetLights();
		}
	}

	public override void applyCommentAction(string comment, string target , bool isSuperChat)
	{
		commentAction(target);
	}
	
	
	public void commentAction(string targetComment)
	{
		//例外
		if (lightDictionary[targetComment] == null)
		{
			return;
		}
		
		lightDictionary[targetComment].addIntensity(increaseValue,intensityMax);
	}
	

	private void resetLights()
	{
		for (int i = 0; i < targetLightCount; i++)
		{
			_targetLightSettings[i].resetIntensity();
		}
	}
}
