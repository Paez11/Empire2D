using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float moveSpeed;
    private Vector3 velocityVector;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetVelocity(Vector3 velocityVector){
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * moveSpeed;
        animator.SetBool("Running", velocityVector.magnitude != 0.0f);    
    }
}
