using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotgame : MonoBehaviour
{
    public Material orbMat;
    public Material chosenMat; //color of selected orb
    public Material lineMat;

    public int difficulty;
    public float spacing;
    public float orbSize; 

    private LineRenderer lr;

    private List<GameObject> sphereObjects = new List<GameObject>();
    
    private List<Vector3> userPoints = new List<Vector3>();
    private List<GameObject> points = new List<GameObject>();
    private Vector3[] isoverts;


    private Vector3 firstPoint;
    private Vector3 secondPoint;
    
    private int check = 0;
    private GameObject shield;

   
    // int space = 2;

    private Vector3[] IcosahedronVertices()
    {
            float t = (1f + Mathf.Sqrt(spacing)) / 2f;
        
            return new Vector3[]
            {
                new Vector3(-1,  t,  0),
                new Vector3( 1,  t,  0),
                new Vector3(-1, -t,  0),
                new Vector3( 1, -t,  0),
                new Vector3( 0, -1,  t),
                new Vector3( 0,  1,  t),
                new Vector3( 0, -1, -t),
                new Vector3( 0,  1, -t),
                new Vector3( t,  0, -1),
                new Vector3( t,  0,  1),
                new Vector3(-t,  0, -1),
                new Vector3(-t,  0,  1)
            };
    }
    

    void Awake()
    {   
        //make orbs
        isoverts = IcosahedronVertices();
        for (int i = 0; i < isoverts.Length; i++)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.localScale = new Vector3(orbSize, orbSize, orbSize);
            sphereObjects.Add(obj);
            obj.name = "Sphere" + "_" + i;
            obj.transform.position = isoverts[i];
            obj.GetComponent<Renderer>().material = orbMat;
        }

        //make shield
        shield = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        float radius = spacing /2f - orbSize;
        shield.transform.localScale = new Vector3(radius, radius, radius);
        shield.name = "Shield";
        shield.GetComponent<Renderer>().material = orbMat;

        //pick list of chosen orbs
        for (int i = 0; i < difficulty; i++)
        {
            var chosen = sphereObjects[Random.Range(0,sphereObjects.Count - 1)];
            points.Add(chosen);
        }


    }



    void DrawLine(Vector3 start, Vector3 end)
    {
        lr = (new GameObject("line")).AddComponent<LineRenderer>();
        
        lr.material = lineMat;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 1f;
        lr.endWidth = 1f;

        userPoints.Add(firstPoint);

        firstPoint = secondPoint;

        if(userPoints.Count == (points.Count - 1)){
            
            CheckWin();
        }
    }

    // // Change Color of the orbs
    IEnumerator ChangeColor()
    {   
        for (int i = 0; i < difficulty; i++)
        {
            var chosen = points[i];

            //Get the Renderer component for sphere
            var renderer = chosen.GetComponent<Renderer>();
            renderer.material = chosenMat;
        
            yield return new WaitForSeconds(1f);

            renderer.material= orbMat;
        }
    }

    void CheckWin()
    {   
        //add final user selected point
        userPoints.Add(firstPoint);
        if (userPoints.Count != points.Count)
        {
            FindObjectOfType<gamemanager>().Lose();
            return;
        }
        
        for (int i = 0; i < points.Count; i++)
        {
            if (userPoints[i] != points[i].transform.position)
            {
                Debug.Log("Loss");
                FindObjectOfType<gamemanager>().Lose();
                shield.GetComponent<Renderer>().material = chosenMat;
                return;
            }   
        }
        Debug.Log("Win");
        FindObjectOfType<gamemanager>().Win();
        Destroy(shield);

        return;
        
    }

    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine (ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown (0))
        { 
                RaycastHit hit; 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
                if ( Physics.Raycast (ray,out hit,100.0f)) 
                {
                    // Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                    if(hit.collider.gameObject.name != "Shield" && hit.collider.gameObject.name != "spore"){
                        if(check == 0){
                            firstPoint = hit.transform.position;
                            check = 1;
                        }else{
                            secondPoint = hit.transform.position;
                            DrawLine(firstPoint, secondPoint);
                        }
                    }
                    
                }
         }
    }
}


