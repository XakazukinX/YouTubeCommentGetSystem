using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using shigeno_EditorUtility;


public class QuestionnaireAction : YoutubeCommentActionBase
{
    [Header("アンケートのUIの本体")]
    [SerializeField] private GameObject questionUI;
    
    [Header("質問内容。実際にUI側に表示される")]
    [SerializeField] public string questionContent = "ねこ派？いぬ派？";
    [SerializeField] private Text questionText;
    
    //回答の設定
    [System.Serializable]
    public class AnswerSettings
    {
        //選択肢の内容とそれを表示するUIテキスト
        [NonEditable]
        public string answerContent;
        [NonEditable]
        public Text answerText;
        //選択された回数とそれを表示するUIテキスト        
        [NonEditable]
        internal int answerCount;
        [NonEditable]
        public Text answerCountText;

        public AnswerSettings(string _answerContent, Text _answerText, int _answerCount, Text _answerCountText)
        {
            this.answerContent = _answerContent;
            this.answerText = _answerText;
            this.answerCount = _answerCount;
            this.answerCountText = _answerCountText;
        }
        
    }



    [Header("回答内容。実際にUI側に表示される")] 
    public List<AnswerSettings> _answerSettingses = new List<AnswerSettings>();

    private Dictionary<string, AnswerSettings> answerSettingDictionary;



    //アンケートを開始するキー
    [SerializeField] private KeyCode startQuestionnaireKey;
    
    //アンケートを中断するキー
    [SerializeField] private KeyCode interruptQuestionnaireKey;
    
    //アンケートが始まっているかどうかを監視
    private bool isStartVote;

    private void Awake()
    {
        isStartVote = false;
        initDictionary();
    }


    private void Update()
    {
        //アンケートの実行を開始するキー入力を監視
        if (Input.GetKeyUp(startQuestionnaireKey) && !isStartVote)
        {
            //辞書を初期化
            initDictionary();
            
            //UIを初期化
            initQuestion();
        }

        if (Input.GetKeyUp(interruptQuestionnaireKey))
        {
            isStartVote = false;
            questionUI.SetActive(false);
        }
        
    }


    public override void applyCommentAction(string comment, string target ,bool isSuperChat)
    {
        //投票が始まっていなかったら弾く
        if (!isStartVote)
        {
            return;
        }
        
        commentAction(target);
    }
    
    public void commentAction(string targetComment)
    {
        if (answerSettingDictionary[targetComment] == null)
        {
            Debug.LogError("エラー！辞書に登録されてないです");
            return;
        }
        
        Debug.Log("Count!");
        answerSettingDictionary[targetComment].answerCount += 1;
        AnswerSettings settings = answerSettingDictionary[targetComment];
        settings.answerCountText.text = settings.answerCount.ToString();
    }

    internal void setQuestionList(string _questionContent,string[] answers)
    {
        questionContent = _questionContent;
        var beforeanswer = new List<AnswerSettings>();

        for (int i = 0; i < _answerSettingses.Count; i++)
        {
            beforeanswer.Add(_answerSettingses[i]);
        }
        
        //AnswerListの初期化
        _answerSettingses = new List<AnswerSettings>();
        
        //新しいAnswerSettingsを追加
        for (int i = 0; i < beforeanswer.Count; i++)
        {
            _answerSettingses.Add(new AnswerSettings(answers[i], beforeanswer[i].answerText, 0,
                beforeanswer[i].answerCountText));
        }
        
        initDictionary();
    }

    
    
    
    private void initDictionary()
    {
        //辞書の初期化
        answerSettingDictionary = new Dictionary<string, AnswerSettings>();
        for (int i = 0; i < _answerSettingses.Count; i++)
        {
            answerSettingDictionary.Add(_answerSettingses[i].answerContent,_answerSettingses[i]);
        }
    }
    
    
    //アンケートの実行
    private void initQuestion()
    {
        isStartVote = true;
        questionUI.SetActive(true);
        
        //各UIを初期化
        questionText.text = questionContent;
        for (int i = 0; i < _answerSettingses.Count; i++)
        {
            _answerSettingses[i].answerText.text = _answerSettingses[i].answerContent;
            _answerSettingses[i].answerCount = 0;
            _answerSettingses[i].answerCountText.text = _answerSettingses[i].answerCount.ToString();
        }
        
        QuestionTimer.Instance.startVote();
        
        
    }

    public void endQuestion()
    {
        isStartVote = false;
        //questionUI.SetActive(false);
        questionText.text = "投票終了！\n" + questionText.text;

    }


    public string testTargetComment;

    [ContextMenu("testInit")]
    public void test()
    {
        initDictionary();
        initQuestion();
    }

    [ContextMenu("testAnswer")]
    public void testAns()
    {
        commentAction(testTargetComment);
    }
}