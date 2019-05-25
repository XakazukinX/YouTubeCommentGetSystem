using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireEditor : EditorWindow {

	[MenuItem("YouTubeCommentGetter/QuestionEdit")]
	public static void Create()
	{
		// 生成
		GetWindow<QuestionnaireEditor>("アンケート作成");
		_questionnaireAction = FindObjectOfType<QuestionnaireAction>();
		EditorApplication.playModeStateChanged += OnChangedPlayMode;

	}

	//プレイモードが変更された
	private static void OnChangedPlayMode(PlayModeStateChange state) 
	{
	}
	

	public string _question;
	
	public string[] _answers = new string[4];

	public float _voteTime = 60;

	static QuestionnaireAction _questionnaireAction;
	private QuestionnaireAction.AnswerSettings _answerSettings;

	private static string logMessage = "ここにログが出力されます";


	private void OnGUI()
	{

		_question = EditorGUILayout.TextField("質問内容", _question);


		for(int i = 0; i < _answers.Length; ++i)
		{
			_answers[i] = EditorGUILayout.TextField("選択肢"+i,_answers[i]);
		}


		_voteTime = EditorGUILayout.Slider("アンケートの時間",_voteTime, 0, 999);
		
		
		if (GUILayout.Button("アンケート作成！"))
		{
			_questionnaireAction = FindObjectOfType<QuestionnaireAction>();
			if (_questionnaireAction == null)
			{
				Debug.LogError("QuestionnaireActionが見つかりません！");
				return;
			}
			applyQuestions();
		}
		

		GUILayout.TextArea(logMessage,400);

		EditorApplication.update();
	}
	
	
	//各スクリプトのアンケート内容を変更する。
	private void applyQuestions()
	{
		
		QuestionTimer.Instance.voteTime = _voteTime;

		_questionnaireAction.setQuestionList(_question, _answers);
		

		//適用済の個数を監視
		var applyCount = 0;
		//YouTubeCommentCheck本体の設定変更
		for (int i = 0; i < YoutubeCommentCheck.Instance.commentActionSettings.Count; i++)
		{
			//ターゲットコメントに対してのActionBaseがQuestionnaireActionのものだけを取り出してそのターゲットコメントを変更する。
			if (YoutubeCommentCheck.Instance.commentActionSettings[i]._commentAction.GetType() == typeof(QuestionnaireAction))
			{
				YoutubeCommentCheck.Instance.commentActionSettings[i].targetComment = _answers[applyCount];
				applyCount += 1;
			}
		}
		
		

		logMessage = "以下の内容でアンケートを作成しました。\n" + "質問内容：" + _question + "\n";
		logMessage += "アンケート時間：" + _voteTime + "\n";
		for (int i = 0; i < _answers.Length; i++)
		{
			logMessage += ("選択肢" + (i + 1) + ":" + _questionnaireAction._answerSettingses[i].answerContent+"\n");
		}
	}


}
