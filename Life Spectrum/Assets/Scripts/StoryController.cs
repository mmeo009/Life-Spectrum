using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace LIFESPECTRUM
{
    public class StoryController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject card;
        [SerializeField] private StoryObject story;
        [SerializeField] private bool isLeft = false;
        private Tweener moveTween, rotateTween;
        private void Start()
        {
            gameManager = GameManager.Instance;
        }
        private void Update()
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
                if (hit.transform.gameObject.name == "StoryCard")
                {
                    Debug.Log("Hit: " + hit.transform.gameObject.name);
                    card = hit.transform.parent.gameObject;
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

                if (target.x < -2)
                {
                    target.x = -2;
                }

            }
            else if (target.x > 0)
            {
                isLeft = false;

                if (target.x > 2)
                {
                    target.x = 2;
                }

            }
            var modifiedVector = new Vector3(target.x, 0, -0.5f);
            var rotationAngle = isLeft ? 13f : -13f;

            delta *= Vector3.Distance(transform.position, modifiedVector);

            if (moveTween != null && moveTween.IsActive())
            {
                moveTween.Kill();
            }

            if (rotateTween != null && rotateTween.IsActive())
            {
                rotateTween.Kill();
            }

            gameManager.ChangeStoryUI(GameSystem.Instance.nowStory, true, isLeft);
            moveTween = card.transform.DOMove(modifiedVector, delta).SetEase(Ease.Linear);
            rotateTween = card.transform.DORotate(new Vector3(0, 0, rotationAngle), delta).SetEase(Ease.Linear);
        }
        private void EndMouseDrag()
        {
            if (moveTween != null && moveTween.IsActive())
            {
                moveTween.Kill();
            }

            if (rotateTween != null && rotateTween.IsActive())
            {
                rotateTween.Kill();
            }

            if(Mathf.Abs(card.transform.position.x) > 0.3)
            {
                if(isLeft == true)
                {
                    GameSystem.Instance.ApplyOption(GameSystem.Instance.nowOptions[1]);
                }
                else
                {
                    GameSystem.Instance.ApplyOption(GameSystem.Instance.nowOptions[0]);
                }
            }

            card.transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.Linear);
            card.transform.DOMove(new Vector3(0, 0, -0.5f), 0.1f);
            Debug.Log((isLeft == true) ? "¿ÞÂÊ" : "¿À¸¥ÂÊ");
            isLeft = false;
            card = null;
        }
    }
}
