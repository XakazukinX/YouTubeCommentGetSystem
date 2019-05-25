using System.Collections;
using System.Collections.Generic;
using shigeno_EditorUtility;
using UnityEngine;

public class LightIntensityDecreaser : MonoBehaviour
{
    //モード一覧
    public enum DeceaseMode
    {
        AutoMode,
        SemiAutoMode,
        ManualMode,
    }
    //モードを選択
    [CustomLabel("モードを選択")]
    public DeceaseMode _mode = DeceaseMode.SemiAutoMode;
    
    //実行するキー
    [SerializeField] private KeyCode decreaseKey;
    
    //徐々に光を減らしていく対象のLightObject
    [CustomLabel("対象のLight")]
    [SerializeField] private Light[] decreaseTargets;
    //どのくらいずつ光量を減らしていくか
    [CustomLabel("減らす量")]
    [SerializeField] private float decreaseWeight = 1.0f;
    //光量が減っていく時間
    [CustomLabel("光量が減る時間(単位/秒)")]
    [SerializeField] private float decreaseTimeSpan = 10.0f;

    //ManyLightIntensityIncreaseActionを設定。
    public ManyLightIntensityIncreaseAction manyLightAction;
    private float intensityMax = 2.0f;
    
    //コルーチンが走っているかどうかを管理。
    private bool isStart;
    
    private void Start()
    {
        //光量の最大値を初期化。
        initIntensityMax();
                
        //manualモードなら何もしない。
        if (_mode == DeceaseMode.ManualMode)
        {
            return;
        }
        
        //autoモードの時はStartのタイミングでコルーチンを走らせる。
        if (_mode == DeceaseMode.AutoMode)
        {
            StartCoroutine(decreaseCoroutine());
        }
    }

    private void Update()
    {

        //auto時にはUpdateでは何もしない。
        if (_mode == DeceaseMode.AutoMode)
        {
            return;
        }

        //キーの入力を受ける
        if (Input.GetKeyDown(decreaseKey))
        {
            keyGetAct();
        }
    }


    //光量の最大値を初期化する関数
    private void initIntensityMax()
    {
        //ManyLightIntensityIncreaseActionがアサインされていなかったら探す。
        if (manyLightAction == null)
        {
            manyLightAction = FindObjectOfType<ManyLightIntensityIncreaseAction>();
            //それでもNullだった場合は暫定的に2.0を格納する。
            if (manyLightAction == null)
            {
                intensityMax = 2.0f;
                Debug.LogWarning("<color=#ff0000>シーン上にManyLightIntensityIncreaseActionがありませんでした。</color>とりあえず2.0を最大値とします。");
            }
            else
            {
                intensityMax = manyLightAction.intensityMax;
            }
        }
        else
        {
            intensityMax = manyLightAction.intensityMax;
        }
    }
    
    
    //キーの入力を受けたときに実行する関数
    private void keyGetAct()
    {
        //コルーチンが走っていない場合だけコルーチンを開始する。
        if (_mode == DeceaseMode.SemiAutoMode && !isStart)
        {
            StartCoroutine(decreaseCoroutine());
        }
        
        //キー入力を受けるたびに光を減らす。
        if (_mode == DeceaseMode.ManualMode)
        {
            decrease();
        }
        
    }


    //auto、もしくはsemiautoの時に走るコルーチン
    private IEnumerator decreaseCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(decreaseTimeSpan);
            
            decrease();
            
            yield return null;
            
        }
    }
    
    //光量を減少させる関数
    private void decrease()
    {
        for (int i = 0; i < decreaseTargets.Length; i++)
        {
            var _intensity = decreaseTargets[i].intensity;

            //光量が最大値に達している、もしくは0の場合はスキップ。
            if (_intensity >= intensityMax || _intensity == 0)
            {
                continue;
            }
            _intensity = Mathf.Clamp(_intensity -= decreaseWeight, 0, intensityMax);
            decreaseTargets[i].intensity = _intensity;
        }
    }
    
    
}
