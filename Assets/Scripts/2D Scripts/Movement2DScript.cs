using UnityEngine;

public class Movement2DScript : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField, Tooltip("Set the movement speed in pixel per second")]
    private float movementSpeed = 1;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            transform.position += new Vector3(0, movementSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit the finish line!!!");
    }
}
