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

        public TMP_Text statText;
        public TMP_Text storyNameText;
        public TMP_Text storyText;
        public TMP_Text leftButtonText;
        public TMP_Text rightButtonText;

        public GameObject card;
        public bool isLeft = false;

        [SerializeField] private Vector3 curveStart = new Vector3(-4f, -2, 0);
        [SerializeField] private Vector3 curveEnd = new Vector3(-4f, -2, 0);
        [SerializeField] private List<Vector3> curveDots = new List<Vector3>();


        private void Start()
        {
            gameManager = GameManager.Instance;
            curveDots = GenerateCurvePoints(curveStart, new Vector3(0, 1f, 0), curveEnd, 25);
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
            List<float> angles = new List<float>();

            float minAngle = -20f;
            float maxAngle = 20f;


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
            Debug.Log((isLeft == true) ? "¿ÞÂÊ" : "¿À¸¥ÂÊ");
            isLeft = false;
            card = null;
            rightButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);
            leftButtonText.color = new Color(0.2735849f, 0.2735849f, 0.2735849f, 1);
        }

        public List<Vector3> GenerateCurvePoints(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int numPoints)
        {
            List<Vector3> curvePoints = new List<Vector3>();

            if (numPoints <= 5) numPoints = 5;


            for (int i = 0; i <= numPoints; i++)
            {
                float t = i / (float)numPoints;
                Vector3 point = GetCurvePoint(startPoint, controlPoint, endPoint, t);
                curvePoints.Add(point);

            }

            return curvePoints;
        }

        public Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return (oneMinusT * oneMinusT * a) + (4f * oneMinusT * t * b) + (t * t * c);
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(curveStart, 0.03f);
            //Gizmos.DrawSphere(Vector3.zero, 0.03f);
            Gizmos.DrawSphere(curveEnd, 0.03f);

            Vector3 p1 = curveStart;
            for (int i = 0; i < 20; i++)
            {
                float t = (i + 1) / 20f;
                Vector3 p2 = GetCurvePoint(curveStart, new Vector3(0, 1f, 0), curveEnd, t);
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }
        }
    }
}
