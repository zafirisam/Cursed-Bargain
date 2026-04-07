using UnityEngine;

//this is a script that make the camera follow a target smoothly instead of snapping instantly
public class CameraFollow2D : MonoBehaviour
{
    public Transform target; // The object the camera soulf follow
    public float smoothSpeed = 5f; // how fast the camera move to the target
    public Vector3 offset; // this will set the camera slightly above the target

    private void LateUpdate()
    {
        if (target == null) return; //if no target is assigned stop running this code

        // hear i created the desired camera offset/position
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;
        // Lerp means "move smoothly from current position to offset desiredPosition
        //Time.deltaTime makes it frame-rate independent
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
