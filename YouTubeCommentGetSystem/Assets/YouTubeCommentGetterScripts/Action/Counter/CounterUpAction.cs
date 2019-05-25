using System.Collections;
using System.Collections.Generic;
using shigeno_EditorUtility;
using UnityEngine;
using UnityEngine.UI;

public class CounterUpAction : YoutubeCommentActionBase
{
    [SerializeField] internal Slider _slider;
    [CustomLabel("どのくらいずつゲージをためるか")]
    [SerializeField] private float additionalWeight;
    [CustomLabel("ゲージの最大値")]
    [SerializeField] internal float counterMax;
    [CustomLabel("スパチャでどのくらいブーストするか")]
    [SerializeField] private float boostWeight = 2.0f;

    private float defAdditionalWeight;

    private void Start()
    {
        _slider.value = 0;
        _slider.maxValue = counterMax;
        defAdditionalWeight = additionalWeight;
    }

    public override void commentAction()
    {
        base.commentAction();
        
        _slider.value = Mathf.Clamp(_slider.value + additionalWeight, 0, counterMax);
        
        //スパチャのときにブーストしてたらウェイトを戻す。
        if (additionalWeight != defAdditionalWeight)
        {
            additionalWeight = defAdditionalWeight;
        }
        
    }

    //スパチャ時の処理
    public override void superChatAction()
    {
        additionalWeight *= boostWeight;
    }

    [ContextMenu("test")]
    private void test()
    {
        commentAction();
    }
}
