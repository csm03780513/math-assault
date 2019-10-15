using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(AudioSource))]
public class GamePlayController : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI SumText;
    public TextMeshProUGUI[] SumAnsText;
    public TextMeshProUGUI ModeratorText,submitNextText,scoreText,challengeText,passedText,livesLeftText;

    public AudioClip correctAnsAudio,wrongAnsAudio,timeOutAudio,nextAudio,gameOverAudio;
    AudioSource audioSource;

    public InputField AnswerInputField;
    public Slider TimerCount;
    bool canContinueTimer, isNextChallenge,isCheckAnswer,isRestartGame,isGameOverPlayed;
    public float maxTime=12;
    float currentTime;
    int SumAnsTextCount;
   

   private int firstNo,secondNo,livesLeft,score,passed,challenge;

    void Start()
    {
        SumAnsTextCount = SumAnsText.Length;
        isRestartGame = false;

        audioSource = GetComponent<AudioSource>();

        isNextChallenge = false;
        isCheckAnswer = true;
        isGameOverPlayed = false;

        livesLeft = 3;
        livesLeftText.text = "Lives Left:" + livesLeft.ToString();

        passed = 0;
        challenge = 1;

        ModeratorText.text = "";
        GetChallenge();
    }

    // Update is called once per frame
    void Update()
    {
        if (livesLeft <= 0)
        {
            if (!isGameOverPlayed)
            {
                audioSource.PlayOneShot(gameOverAudio, 0.9f);
                isGameOverPlayed = true;
            }
            canContinueTimer = false;
            isNextChallenge = false;
            isCheckAnswer = false;
            isRestartGame = true;

            float accuracyPercentage = 0;
            accuracyPercentage =Mathf.Round(((float)passed/challenge)*100);

            ModeratorText.text = "(^_^) \n You are out of Lives. \n GAME OVER \n Accuracy:"+ accuracyPercentage+"%";
            submitNextText.text = "START AGAIN";
        }
    }

    int GetRandomNumber()
    {
        return UnityEngine.Random.Range(2, 20);
    }

    string finalAnswer;
    string playerAnswer;


    public void GamePlayModerator()
    {
        if (isNextChallenge)
        {
            audioSource.PlayOneShot(nextAudio, 1f);

            GetChallenge();
            challenge++;
            challengeText.text = "Challenge:" + challenge;
        }
        else if(isCheckAnswer)
        {
            //GetAnswer();
        }else if (isRestartGame)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void GetAnswer(int AnsPos)
    {
        if (isCheckAnswer)
        {
            finalAnswer = (firstNo * secondNo).ToString();
            playerAnswer = SumAnsText[AnsPos].text;

            if (finalAnswer == playerAnswer)
            {
                audioSource.PlayOneShot(correctAnsAudio, 0.7f);

                ModeratorText.text = "Correct Answer!!\nTry the Next Challenge";
                canContinueTimer = false;

                SetGamePlayState();

                score += 100;
                passed++;
                scoreText.text = "Score:" + score.ToString();
                passedText.text = "Passed:" + passed.ToString();
            }
            else
            {
                audioSource.PlayOneShot(wrongAnsAudio, 0.7f);
                livesLeft--;
                livesLeftText.text = "Lives Left:" + livesLeft.ToString();
                ModeratorText.text = "Incorrect answer!!\nPlease answer before your time expires!!";
                canContinueTimer = true;

                //set gameplay state
                isNextChallenge = false;
                isCheckAnswer = true;
            }
        }

    }

    string DoSum()
    {
        finalAnswer = (firstNo * secondNo).ToString();
        return finalAnswer;
    }

    int DoSumReturnint()
    {
       int finalAnswer = firstNo * secondNo;
        return finalAnswer;
    }

    IEnumerator Timer()
    {
        canContinueTimer = true;

        //resets silder time counts 
        TimerCount.maxValue = maxTime;
        TimerCount.value = maxTime;
        currentTime = maxTime;

        for (float curTime =currentTime; curTime<=maxTime; curTime-=0.0625f)
        {
            if (canContinueTimer)
            {
                TimerCount.value = curTime;
                yield return new WaitForSeconds(0.0625f);
            }
            else
            {
                //breaks the loop
                StopCoroutine(Timer());
                break;
            }
            if (curTime <= 0)
            {
                audioSource.PlayOneShot(timeOutAudio, 0.7f);
                
                livesLeft--;
                livesLeftText.text = "Lives Left:" + livesLeft.ToString();
                ModeratorText.text = "(^_^)\nToo Slow!\nAnswer is: "+ DoSum()+ " \nTry the next challenge.";
                SetGamePlayState();

                break;
            }
        }
    }

    private void SetGamePlayState()
    {
        submitNextText.text = "NEXT CHALLENGE";

        //set gameplay state
        isNextChallenge = true;
        isCheckAnswer = false;
    }

    public void GetChallenge()
    {
        firstNo = GetRandomNumber();
        secondNo = GetRandomNumber();

        SumText.text = firstNo.ToString() + " x " + secondNo.ToString();
        submitNextText.text = "SUBMIT";
        ModeratorText.text = "(^_^)\n Which Answer is Correct!!";

       //rests the answer input field;
       AnswerInputField.text = "";

        isNextChallenge = false;
        isCheckAnswer = true;

        GiveAnswers();

        StartCoroutine(Timer());
       
    }

    int secondaryAnsPos;
    int numToAdd;
    int usedPositionsCount = 0;
    int finalAnsPos;
    private void GiveAnswers()
    {
         usedPositionsCount = 0;
        //first set correct answer in a random position
        //store the position in a variable
        finalAnsPos = UnityEngine.Random.Range(0, 3);
        //Debug.Log("finalAns pos " + finalAnsPos);
        SumAnsText[finalAnsPos].text = DoSum();

        while (usedPositionsCount <= 2)
        {
             secondaryAnsPos = UnityEngine.Random.Range(0, 2);
            
            if (finalAnsPos != secondaryAnsPos)
            {
                if (usedPositionsCount == 0)
                {
                    SumAnsText[secondaryAnsPos].text = (DoSumReturnint()+10).ToString();
                    Debug.Log("Answer " + (DoSumReturnint() + 10).ToString() +" at position "+secondaryAnsPos);
                    Debug.Log("used pos " + usedPositionsCount);
                }      
                usedPositionsCount++;
            }

        }
    }
}
