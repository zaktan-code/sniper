using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Animator animator;
    private Rigidbody mainRigidbody;
    private Collider mainCollider;
    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    private void Awake() {
        animator = GetComponent<Animator>();
        mainRigidbody = GetComponent<Rigidbody>();
        allRigidbodies = GetComponentsInChildren<Rigidbody>();
        mainCollider = GetComponent<Collider>();
        allColliders = GetComponentsInChildren<Collider>(true);
    }

    private void Start() {
        InitializeRagdollBodyParts();
        ToggleRagdoll(false);
    }

    private void Update() {
        
    }

    private void InitializeRagdollBodyParts() {
        foreach (Rigidbody rigidbody in allRigidbodies) {
            if (rigidbody == mainRigidbody) {
                continue;
            }
            rigidbody.gameObject.AddComponent<EnemyBodyPart>();
        }
    }

    private void ToggleRagdoll(bool active) {
        foreach (Collider collider in allColliders) {
            collider.enabled = active;
        }
        foreach (Rigidbody rigidbody in allRigidbodies) {
            rigidbody.isKinematic = !active;
        }

        mainCollider.enabled = !active;
        mainRigidbody.isKinematic = active;
        animator.enabled = !active;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Bullet") {
            ToggleRagdoll(true);
            if (!FirstBodyPartHit(collision.GetContact(0).point, collision.GetContact(0).normal)) {
                ToggleRagdoll(false);
            }
            else {
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private bool FirstBodyPartHit(Vector3 point, Vector3 normal) {
        Ray ray = new Ray(point, normal);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit)) {
            EnemyBodyPart enemyBodyPart = raycastHit.collider.gameObject.GetComponent<EnemyBodyPart>();
            if (enemyBodyPart != null) {
                enemyBodyPart.Hit(raycastHit.normal, raycastHit.point);
                return true;
            }
        }
        return false;
    }
}
