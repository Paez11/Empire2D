using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePositionDirect : MonoBehaviour, IMoveVelocity, IMovePosition
{
    public Vector3 movePosition;
    private Vector3 previousPosition;
    private Vector3 currentPosition;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;
        SetAgentPosition(this.movePosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (movePosition - transform.position).normalized;

        if (Vector3.Distance(movePosition, transform.position) < 1f)
            moveDir = Vector3.zero; // Stop Moving when near an object

        GetComponent<IMoveVelocity>().SetVelocity(moveDir);

        currentPosition = transform.position; // Get the current position

        Vector3 movementDelta = currentPosition - previousPosition; // Calculate the change in position

        // Determine the direction based on the movement delta
        if (movementDelta.x < 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); 
        }
        else if (movementDelta.x > 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); 
        }

        previousPosition = currentPosition; // Update the previous position for the next frame
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        throw new System.NotImplementedException();
    }

    void SetAgentPosition(Vector3 movePosition)
    {
        agent.SetDestination(new Vector3(movePosition.x, movePosition.y, transform.position.z));
    }
}
