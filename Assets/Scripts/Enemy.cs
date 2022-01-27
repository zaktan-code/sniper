using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private GameObject hitEffect;

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
            EnemyBodyPart enemyBodyPart = rigidbody.gameObject.AddComponent<EnemyBodyPart>();
            enemyBodyPart.SetHitEffect(hitEffect);
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
            if (!FirstBodyPartHit(collision.GetContact(0).point, collision.GetContact(0).normal, collision.gameObject.GetComponent<Bullet>().Direction)) {
                ToggleRagdoll(false);
            }
            else {
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private bool FirstBodyPartHit(Vector3 point, Vector3 collisionNormal, Vector3 hitNormal) {
        Ray ray = new Ray(point, collisionNormal);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit)) {
            EnemyBodyPart enemyBodyPart = raycastHit.collider.gameObject.GetComponent<EnemyBodyPart>();
            if (enemyBodyPart != null) {
                enemyBodyPart.Hit(hitNormal, raycastHit.point);
                return true;
            }
        }
        return false;
    }
}
