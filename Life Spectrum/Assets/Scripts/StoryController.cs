using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public StoryObject story;
    public GameManager gameManager;

    public TMP_Text statText;
    public TMP_Text storyNameText;
    public TMP_Text storyText;
    public TMP_Text leftButtonText;
    public TMP_Text rightButtonText;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void ChangeData()
    {
        statText.text = $"체력 : {gameManager.stats.statLife}, 배고픔 : {gameManager.stats.statFullness}, 행복도 : {gameManager.stats.statFeel}, 자원 : {gameManager.stats.money}";
        storyNameText.text = story.storyName;
        storyText.text = story.story;
        leftButtonText.text = story.leftWay.wayText;
        rightButtonText.text = story.rightWay.wayText;
    }

    public void Way()
    {

    }
}
