using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMoveVelocity, IMovePosition
{
    private Vector3 movePosition;

    private void Awake() {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 movePosition){
        this.movePosition = movePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (movePosition - transform.position).normalized;
        if(Vector3.Distance(movePosition, transform.position) < 1f)
            moveDir = Vector3.zero; //Stop Moving when near and object
        GetComponent<IMoveVelocity>().SetVelocity(moveDir);
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        throw new System.NotImplementedException();
    }
}
