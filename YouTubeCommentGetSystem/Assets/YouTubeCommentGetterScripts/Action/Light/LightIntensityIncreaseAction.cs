using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityIncreaseAction : YoutubeCommentActionBase
{

    [Header("Intensityを増加させるLight")]
    [SerializeField] private Light[] targetLights;
    [Header("Intensityを増加させる量")]    
    [SerializeField] private float increaseValue = 0.1f;
    [Header("Intensityの最大値を設定")] 
    [SerializeField] private float intensityMax = 2.0f;

    private float[] defLightIntensities;
    private int targetLightCount;

    
    [Header("Intensityをリセットするキー")]
    [SerializeField] private KeyCode resetKey;

    
    private void Awake()
    {
        //Awake時の各LightのIntensity設定をfloat配列に投げる
        targetLightCount = targetLights.Length;
        defLightIntensities = new float[targetLightCount];

        for (int i = 0; i < targetLightCount; i++)
        {
            defLightIntensities[i] = targetLights[i].intensity;
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

    public override void commentAction()
    {
        base.commentAction();
        for (int i = 0; i < targetLightCount; i++)
        {
            var increasedIntensity = Mathf.Clamp(targetLights[i].intensity + increaseValue, defLightIntensities[i], intensityMax);
            targetLights[i].intensity = increasedIntensity;
        }
    }

    private void resetLights()
    {
        for (int i = 0; i < targetLightCount; i++)
        {
            targetLights[i].intensity = defLightIntensities[i];
        }
    }
}
