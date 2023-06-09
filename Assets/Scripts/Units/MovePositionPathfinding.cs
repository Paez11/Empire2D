using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionPathfinding : MonoBehaviour, IMovePosition
{
    private List<Vector3> pathVectorList;
    private int pathIndex = -1;
    public void SetMovePosition(Vector3 movePosition){
        //pathVectorList = GridPathfindingSystem.GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition).pathVectorList;
        if (pathVectorList.Count > 0)
        {
            pathIndex = 0;
        }
    }

    private void Update(){
        if (pathIndex != -1){
            //Move to next path position
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            float reachedPathPositionDistance = 1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPathPositionDistance){
                pathIndex++;
                if (pathIndex >= pathVectorList.Count){
                    // End of path
                    pathIndex = -1;
                }
            }
        }else{
            // idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }
}
