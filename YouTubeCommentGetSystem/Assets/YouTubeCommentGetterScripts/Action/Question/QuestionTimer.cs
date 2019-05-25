using UnityEngine;
using UnityEngine.UI;

public class QuestionTimer : SingletonMonoBehaviour<QuestionTimer>
{
    //投票時間
    public float voteTime;
    private float countTime;
    private bool isVoteTime;

    [SerializeField] private QuestionnaireAction _questionnaireAction;
    [SerializeField] private Text timerText;

    private void Update()
    {
        if (isVoteTime)
        {
            countTime += Time.deltaTime;
            timerText.text = Mathf.Clamp((int)voteTime - (int)countTime, 0, (int)voteTime).ToString();

            if (countTime > voteTime)
            {
                Debug.Log("投票おわり！");
                isVoteTime = false;
                _questionnaireAction.endQuestion();
                return;
            }
        }
    }

    internal void startVote()
    {
        countTime = 0;
        isVoteTime = true;
    }

    internal void endVote()
    {
        
    }
}
