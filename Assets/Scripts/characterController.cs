using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;

public class characterController : MonoBehaviour
{
    [SerializeField]
    GameObject Walter;
    [SerializeField]
    GameObject Gustavo;
    [SerializeField]
    GameObject Mike;
    [SerializeField]
    GameObject Saul;
    [SerializeField]
    GameObject Jesse;

    [SerializeField]
    CinemachineVirtualCamera camera;


    Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();

    Dictionary<string, Vector3> startPositions1 = new Dictionary<string, Vector3>();
    Dictionary<string, Vector3> startPositions2 = new Dictionary<string, Vector3>();
    Dictionary<string, Vector3> startPositions3 = new Dictionary<string, Vector3>();
    Dictionary<string, Vector3> startPositions4 = new Dictionary<string, Vector3>();

    // Added variables to store the from and target objects for movement
    GameObject fromObject;
    GameObject targetObject;

    GameObject lookFromObject;
    GameObject targetFromObject;
    public float transitionDuration = 2.5f;

    
    [SerializeField]
    GameObject Hank;

    int sceneNumber = 0;


    private void Start()
    {

        characters.Add("Walter", Walter);
        characters.Add("Gustavo", Gustavo);
        characters.Add("Mike", Mike);
        characters.Add("Saul", Saul);
        characters.Add("Jesse", Jesse);

        startPositions1.Add("Walter", new Vector3(-136.17f, 67.639f, -24.5f));
        startPositions1.Add("Gustavo", new Vector3(-133.81f, 67.639f, -11.54f));
        startPositions1.Add("Mike", new Vector3(-130.53f, 67.62595f, -21.09f));
        startPositions1.Add("Saul", new Vector3(-140.37f, 67.639f, -19.08f));
        startPositions1.Add("Jesse", new Vector3(-147.5888f, 67.37f, -24.03656f));

        startPositions2.Add("Walter", new Vector3(-175.302f, 67.77f, -0.367f));
        startPositions2.Add("Gustavo", new Vector3(-173.19f, 67.79f, 2.761f));
        startPositions2.Add("Mike", new Vector3(-171.922f, 67.77695f, -0.5200005f));
        startPositions2.Add("Saul", new Vector3(-171.318f, 67.79f, 1.656f));
        startPositions2.Add("Jesse", new Vector3(-170.33f, 67.704f, -3.35f));

        startPositions3.Add("Walter", new Vector3(-25.59401f, 67.14001f, 26.967f));
        startPositions3.Add("Gustavo", new Vector3(-23.48201f, 67.16f, 30.095f));
        startPositions3.Add("Mike", new Vector3(-22.214f, 67.14696f, 26.814f));
        startPositions3.Add("Saul", new Vector3(-21.61f, 67.16f, 28.99f));
        startPositions3.Add("Jesse", new Vector3(-23.62817f, 67.17f, 32.24313f));

        startPositions4.Add("Walter", new Vector3(-78.24f, 4.16f, -186.1032f));
        startPositions4.Add("Gustavo", new Vector3(-77.403f, 4.16f, -188.023f));
        startPositions4.Add("Mike", new Vector3(-76.61f, 4.16f, -182.946f));
        startPositions4.Add("Saul", new Vector3(-71.555f, 4.16f, -185.763f));
        startPositions4.Add("Jesse", new Vector3(-79.79f, 4.32f, -182.16f));

        reset(true);
    }

    private void Update()
    {

        if (fromObject != null && targetObject != null)
        {

            fromObject.GetComponent<Animator>().SetBool("isWalking", true);
            
            Vector3 direction = targetObject.transform.position - fromObject.transform.position;
            float distance = direction.magnitude; // The distance between the from object and the target object
            float stopDistance = 3f; // The distance at which the from object should stop moving
            if (distance > stopDistance) // Check if the from object is close enough to the target object
            {
                //fromObject.GetComponentInChildren<Animator>().SetBool("isWalking", true);
                fromObject.transform.position += direction.normalized * 2 * Time.deltaTime; // Move the from object towards the target object
            

                // Added code to rotate the from object towards the target object over time
                Quaternion targetRotation = Quaternion.LookRotation(direction); // The desired rotation of the from object
                float turnSpeed = 2f; // The speed of rotation in degrees per second
                fromObject.transform.rotation = Quaternion.RotateTowards(fromObject.transform.rotation, targetRotation, turnSpeed * Time.deltaTime); // Rotate the from object gradually towards the target rotation

            }
            else
            {
                fromObject.GetComponentInChildren<Animator>().SetBool("isWalking", false);
            }
        }
        if (lookFromObject != null && targetFromObject != null)
        {
            Vector3 direction = targetFromObject.transform.position - lookFromObject.transform.position;
            lookFromObject.transform.rotation = Quaternion.LookRotation(direction);
        }

    }
    


    public void lookAtWithMove(string from, string target)
    {

        if (characters.ContainsKey(from) && characters.ContainsKey(target))
        {
            fromObject = characters[from]; // Assign the from object to the global variable
            targetObject = characters[target]; // Assign the target object to the global variable


            Vector3 direction = targetObject.transform.position - fromObject.transform.position;


            fromObject.transform.rotation = Quaternion.LookRotation(direction);
            
        }
        else
        {
            Debug.Log("Invalid names: " + from + " or " + target);
        }
    }

    public void lookAt(string from, string target)
    {
        if (characters.ContainsKey(from) && characters.ContainsKey(target))
        {
            lookFromObject = characters[from]; // Assign the from object to the global variable
            targetFromObject = characters[target]; // Assign the target object to the global variable
        }
        else
        {
            Debug.Log("Invalid names: " + from + " or " + target);
        }
    }
    
    public void summonHank(string donorName)
    {
        GameObject HankClone = Instantiate(Hank, new Vector3(-133.68f + Random.Range(-5, 5), 67.374f, -14.29f), Quaternion.Euler(0, 180, 0));

        if (sceneNumber == 1)
        {
            HankClone.transform.position = new Vector3(-175.516f + Random.Range(-5, 5), 67.803f, 3.141f);
        }

        else if(sceneNumber == 2)
        {
            HankClone.transform.position = new Vector3(-25.59401f + Random.Range(-5, 5), 67.14001f, 26.967f);
        }
        else if (sceneNumber == 3)
        {
            HankClone.transform.position = new Vector3(-79.592f + Random.Range(-5, 5), 4.16f, -186.1032f);
        }
        
        HankClone.GetComponentInChildren<TextMeshPro>().text = donorName;
    }

    public void stopMoving(string character)
    {
        RandomWalk walkScript = characters[character].GetComponent<RandomWalk>();
        walkScript.enabled = false;
        characters[character].GetComponent<Animator>().SetBool("isWalking", false);
        characters[character].GetComponent<Rigidbody>().velocity = Vector3.zero;
        characters[character].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    public void startMoving(string character)
    {
        RandomWalk walkScript = characters[character].GetComponent<RandomWalk>();

        characters[character].GetComponent<Animator>().SetBool("isWalking", true);

        walkScript.enabled = true;
    }

    public void switchCamera(string target)
    {
        camera.LookAt = characters[target].transform;   

        if(sceneNumber == 0)
        {
            camera.Follow = characters[target].transform;

            foreach (string character in characters.Keys)
            {
                characters[character].GetComponent<RandomWalk>().speed = 0.8f;
            }

        }
        else if (sceneNumber == 1)
        {
            camera.Follow = null;
            camera.transform.position = new Vector3(-167.79f, 71.536f, 4.297f);

            foreach (string character in characters.Keys)
            {
                characters[character].GetComponent<RandomWalk>().speed = 1.2f;
            }

        }

        else if (sceneNumber == 2)
        {
            camera.Follow = null;
            camera.transform.position = new Vector3(-29.8f, 71.28f, 31.81f);

            foreach (string character in characters.Keys)
            {
                characters[character].GetComponent<RandomWalk>().speed = 0.8f;
            }


        }
        else if (sceneNumber == 3)
        {
            camera.Follow = null;
            camera.transform.position = new Vector3(-78.258f, 9.85f, -179.438f);

            foreach (string character in characters.Keys)
            {
                characters[character].GetComponent<RandomWalk>().speed = 0.8f;
            }

        }

    }



    public void reset(bool resPos)
    {

        sceneNumber = Random.Range(0, 4);
        if(sceneNumber == 0)
        {
            foreach (string character in characters.Keys)
            {
                if (resPos)
                {
                    characters[character].transform.position = startPositions1[character];
                }

                characters[character].GetComponent<Rigidbody>().velocity = Vector3.zero;
                characters[character].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        else if(sceneNumber == 1)
        {
            foreach (string character in characters.Keys)
            {
                if (resPos)
                {
                    characters[character].transform.position = startPositions2[character];
                }

                characters[character].GetComponent<Rigidbody>().velocity = Vector3.zero;
                characters[character].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        else if (sceneNumber == 2)
        {
            foreach (string character in characters.Keys)
            {
                if (resPos)
                {
                    characters[character].transform.position = startPositions3[character];
                }

                characters[character].GetComponent<Rigidbody>().velocity = Vector3.zero;
                characters[character].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        else if (sceneNumber == 3)
        {
            foreach (string character in characters.Keys)
            {
                if (resPos)
                {
                    characters[character].transform.position = startPositions4[character];
                }

                characters[character].GetComponent<Rigidbody>().velocity = Vector3.zero;
                characters[character].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        

        // Added code to reset the from and target objects to null  
        fromObject = null;
        targetObject = null;
    }

}
