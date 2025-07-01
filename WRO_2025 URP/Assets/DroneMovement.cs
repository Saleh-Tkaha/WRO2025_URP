using UnityEngine;
using System.Collections;

public class DroneMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public float strafeAmount = 0.5f;
    public float smoothTime = 0.5f;
    public float tiltAmount = 15f;
    public float waitTime = 2f; 
    public float idleShakeAmount = 0.1f;
    public float idleShakeSpeed = 5f;

    private int currentWaypointIndex = 0;
    private Vector3 velocity = Vector3.zero;
    private bool isWaiting = false;

    void Update()
    {
        if (waypoints.Length == 0) return;

        if (isWaiting)
        {
           
            transform.position += new Vector3(
                Mathf.Sin(Time.time * idleShakeSpeed) * idleShakeAmount,
                Mathf.Cos(Time.time * idleShakeSpeed) * idleShakeAmount,
                0f
            ) * Time.deltaTime;
            return;
        }

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPosition - transform.position).normalized;

       
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);

        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        
        float strafe = Mathf.Sin(Time.time * speed) * strafeAmount;
        transform.position += transform.right * strafe * Time.deltaTime;

        float tiltAngle = Mathf.Clamp(Vector3.Dot(direction, transform.forward), -1f, 1f) * -tiltAmount;
        transform.rotation *= Quaternion.Euler(tiltAngle, 0f, 0f);

        
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaiting = false;
    }
}
