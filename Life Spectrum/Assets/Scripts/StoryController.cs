using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace LifeSpectrum
{
    public class StoryController : MonoBehaviour
    {


        public StoryObject story;
        public GameManager gameManager;
        public GameObject card;
        public bool isLeft = false;
        private Vector3 lastMousePosition;
        private Vector3 curveNormal;

        private void Start()
        {
            gameManager = GameManager.Instance;
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
            float rotationAngle = 0;
            float maxRotation;
            float actualRotation;
            float rotationDirection = 0;

            if (target.x < 0)
            {
                isLeft = true;

                if (target.x < -1)
                {
                    target.x = -1;
                }
                rotationAngle = Mathf.Lerp(0, -13, Mathf.InverseLerp(0, -1, target.x));
                rotationDirection = -1;
            }
            else if (target.x > 0)
            {
                isLeft = false;

                if (target.x > 1)
                {
                    target.x = 1;
                }
                rotationAngle = Mathf.Lerp(0, 13, Mathf.InverseLerp(0, 1, target.x));
                rotationDirection = 1;
            }

            maxRotation = Mathf.Abs(rotationAngle - card.transform.rotation.eulerAngles.x);
            actualRotation = Mathf.Clamp(delta * maxRotation, 0, maxRotation);

            var modifiedVector = new Vector3(target.x, -1, -0.5f);

            delta *= Vector3.Distance(transform.position, modifiedVector);
            card.transform.position = Vector3.MoveTowards(card.transform.position, modifiedVector, delta);
            card.transform.rotation = Quaternion.Euler(card.transform.rotation.eulerAngles.x + rotationDirection * actualRotation, 90, -90);
        }
        private void EndMouseDrag()
        {
            card.transform.position = new Vector3(0, -1, -0.5f);
            card.transform.rotation = Quaternion.Euler(0, 90, -90);
            Debug.Log((isLeft == true) ? "¿ÞÂÊ" : "¿À¸¥ÂÊ");
            isLeft = false;
            card = null;
        }

    }
}
