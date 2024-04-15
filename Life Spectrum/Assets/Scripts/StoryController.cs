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

    public GameObject card;
    public bool isLeft = false;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void ChangeText()
    {
        statText.text = $"체력 : {gameManager.stats.statLife}, 배고픔 : {gameManager.stats.statFullness}, 행복도 : {gameManager.stats.statFeel}, 자원 : {gameManager.stats.money}";
        storyNameText.text = story.storyName;
        storyText.text = story.story;
        leftButtonText.text = story.leftWay.wayText;
        rightButtonText.text = story.rightWay.wayText;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (card != null)
            {
                card = null;
            }
            SendRayCast();
        }
        else if (Input.GetMouseButton(0))
        {
            if (card != null)
            {
                StartMouseDrag();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (card != null)
            {
                EndMouseDrag();
            }
        }

    }

    private void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name == "Touchable")
            {
                Debug.Log("Hit: " + hit.transform.gameObject.name);
                card = hit.transform.gameObject;
            }
        }
    }

    private void StartMouseDrag()
    {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var delta = 40 * Time.deltaTime;

        if (target.x < 0)
        {
            isLeft = true;
            leftButtonText.color = Color.red;
            rightButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);

            if (target.x < -1)
            {
                target.x = -1;
            }
        }
        else if (target.x > 0)
        {
            isLeft = false;
            rightButtonText.color = Color.red;
            leftButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);

            if (target.x > 1)
            {
                target.x = 1;
            }
        }

        var modifiedVactor = new Vector3(target.x, -1, -0.5f);

        delta *= Vector3.Distance(transform.position, modifiedVactor);
        card.transform.position = Vector3.MoveTowards(card.transform.position, modifiedVactor, delta);
    }
    private void EndMouseDrag()
    {
        card.transform.position = new Vector3(0, -1, -0.5f);
        Debug.Log((isLeft == true) ? "왼쪽":"오른쪽");
        isLeft = false;
        card = null;
        rightButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);
        leftButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);
    }
}
