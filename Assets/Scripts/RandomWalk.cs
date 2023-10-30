    using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    

    

    public float speed = 1.2f;

    
    private Vector3 direction;

    
    private Quaternion targetRotation;

    
    public float rotateSpeed = 5f;

    
    void Start()
    {
        
        direction = RandomDirection();
        
        targetRotation = Quaternion.LookRotation(direction);
    }

    
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    


    private Vector3 RandomDirection()
    {
        
        float angle = Random.Range(0f, 2f * Mathf.PI);

        
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);

       
        return new Vector3(x, 0, z);
    }

    // A collision detection method that is called when the gameobject collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a wall or another obstacle
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "character" || collision.gameObject.tag == "paidCharacter")
        {
            // If so, bounce back and pick a new direction
            direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            direction = RandomDirection();
            // Set the target rotation to face the new direction
            targetRotation = Quaternion.LookRotation(direction);
        }
    }



}
