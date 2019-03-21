namespace ARVRAssignments
{
    using UnityEngine;
    using System.Collections.Generic;
    using Vuforia;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class GameController : MonoBehaviour
    {
        public const float r_LineDrawSpeed = 0.5f;

        public enum GameState
        {
            AddingMarkers,
            AnimatingLineDraw,
            ReadyToSpawnAstronaut,
            ReadyToHitAstronaut
        };

        public enum GameTag
        {
            Astronaut
        };

        [SerializeField]
        public ContentPositioningBehaviour m_ContentPositioningBhvr;

        [SerializeField]
        public AnchorInputListenerBehaviour m_AnchorInputLstnrBhvr;

        [SerializeField]
        public LineRenderer m_LineRenderer;

        [SerializeField]
        public TextMesh m_DistanceTextHldr;

        [SerializeField]
        private Button m_ClearBttn;

        private GameState m_nowState;

        //TODO: Declare the class members here.
        public Vector3 markerpos1;
        public Vector3 markerpos2;
        public Vector3 movingpos;
        public GameObject myLineRend;
        public GameObject myTextMesh;
        public GameObject Astronaut;
        public GameObject Cube;
        public int count;
        public float distance;
        public float  StartTime;

        private void Start()
        {
            m_nowState = GameState.AddingMarkers;
            //TODO: Initalise the class members and event listeners.
            GameObject myPlaneFinder = GameObject.FindGameObjectWithTag("Plane");
            myLineRend = GameObject.FindGameObjectWithTag("Lines");
            myTextMesh = GameObject.FindGameObjectWithTag("Text");
            m_ClearBttn = Button.FindObjectOfType<Button>();
            m_AnchorInputLstnrBhvr = myPlaneFinder.GetComponent<AnchorInputListenerBehaviour>();
            m_ContentPositioningBhvr = myPlaneFinder.GetComponent<ContentPositioningBehaviour>();
            m_LineRenderer = myLineRend.GetComponent<LineRenderer>();
            m_DistanceTextHldr = myTextMesh.GetComponent<TextMesh>();
            m_ContentPositioningBhvr.OnContentPlaced.AddListener(SpawnNewMarker);
            m_ClearBttn.onClick.AddListener(Clear);
            Debug.Log("count=" + count);
        }

        private void SpawnNewMarker(GameObject newMarker)
        {
            //TODO: Implement mini task 2 and part of mini task 3 here.
            if (count == 0)
            {
               markerpos1 = newMarker.transform.position;
            }
            if (count == 1)
            {
                markerpos2 = newMarker.transform.position;
                m_nowState = GameState.AnimatingLineDraw;
                StartTime = Time.time;
            }
            count = count + 1;
            Debug.Log("count =" + count);
            Debug.Log("position1 =" + markerpos1.x + "," + markerpos1.y + "," + markerpos1.z);
            Debug.Log("position2 =" + markerpos2.x + "," + markerpos2.y + "," + markerpos2.z);
            distance = Vector3.Distance(markerpos1, markerpos2);
            Debug.Log("distance =" + distance);
        }

        private void Clear()
        {
            Debug.Log("Trying to Reset Scene");
            SceneManager.LoadScene("Main");
        }
        private void Update()
        {
            if (m_nowState == GameState.AnimatingLineDraw)
            {
                //TODO: Implement mini task 3 and 4 here.
                m_ContentPositioningBhvr.OnContentPlaced.RemoveListener(SpawnNewMarker);
                m_AnchorInputLstnrBhvr.enabled = false;
                movingpos = Vector3.Lerp(markerpos1, markerpos2, (Time.time - StartTime) / distance);
                m_LineRenderer.SetPosition(0, markerpos1);
                m_LineRenderer.SetPosition(1, movingpos);
                if (movingpos == markerpos2)
                {
                    m_DistanceTextHldr.text = "Distance = " + distance + "m";
                    myTextMesh.transform.position = new Vector3((markerpos1.x + markerpos2.x) / 2, (markerpos1.y + markerpos2.y) / 2, (markerpos1.z + markerpos2.z) / 2);
                    m_nowState = GameState.ReadyToSpawnAstronaut;
                }
            }
            else
            {
                //TODO: Implement mini task 5 and 6 here.
                if (m_nowState == GameState.ReadyToSpawnAstronaut)
                {
                    if (Input.touchCount > 0)
                    {
                        if (Astronaut == null)
                        {
                            Astronaut = Instantiate(Resources.Load<GameObject>("Astronaut"));
                            Astronaut.transform.position = new Vector3((markerpos1.x + markerpos2.x) / 2, (markerpos1.y + markerpos2.y) / 2, (markerpos1.z + markerpos2.z) / 2);
                            m_nowState = GameState.ReadyToHitAstronaut;
                        }
                    }
                }
                else
                {
                    if (m_nowState == GameState.ReadyToHitAstronaut)
                    {
                        if (Input.touchCount > 0)
                        {
                            RaycastHit hit;
                            Touch touch = Input.GetTouch(0);
                            Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                            if (Physics.Raycast(ray, out hit))
                            {
                                string TagName = hit.collider.tag;
                                Debug.Log("TagName =" + TagName);
                                if (hit.collider.gameObject == Astronaut)
                                {
                                    if (touch.phase == TouchPhase.Began)
                                    {
                                        Cube = Instantiate(Resources.Load<GameObject>("Cube"));
                                        Cube.transform.position = new Vector3(Astronaut.transform.position.x, Astronaut.transform.position.y + 1.135f, Astronaut.transform.position.z);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}