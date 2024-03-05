using UnityEngine;

public class GyroScopeCuttingScript : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    void Update()
    {
        if (gyroEnabled)
        {
            // Get the orientation of gravity
            Vector3 gravityDirection = -Input.gyro.gravity;

            // Adjust for orientation differences between device and Unity's coordinate system
            Vector3 movementVector = new Vector3(-gravityDirection.x, gravityDirection.y, 0);

            // Apply the movement to the object
            transform.Translate(movementVector * Time.deltaTime * 5f);
        }
    }
}