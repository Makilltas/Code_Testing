using UnityEngine;

public class TrapMover : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(0, 2f, 0); 
    public float moveSpeed = 2f;                       
    public float pauseTime = 1f;                       

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp = true;
    private bool isPausing = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveOffset;
        StartCoroutine(MoveTrap());
    }

    System.Collections.IEnumerator MoveTrap()
    {
        while (true)
        {
            Vector3 destination = movingUp ? targetPos : startPos;

            while (Vector3.Distance(transform.position, destination) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = destination;

            yield return new WaitForSeconds(pauseTime);

            movingUp = !movingUp;
        }
    }
}
