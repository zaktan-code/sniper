using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform bulletsParent;

    [SerializeField]
    private Transform bulletStartFrom;

    private Animator animator;
    private Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;

    private PlayerState state;
    private bool setAimingWhenStop = false;
    private bool readyToShoot = false;
    private Vector3 aimTargetPosition = Vector3.forward;

    private bool IsMoving {
        get {
            return navMeshAgent.velocity != Vector3.zero;
        }
    }

    private bool IsEnemyVisible {
        get {
            return enemy.GetComponentInChildren<SkinnedMeshRenderer>().isVisible;
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        state = PlayerState.Walking;
        Physics.queriesHitTriggers = false;
    }

    private void Update() {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        switch (state) {
            case PlayerState.Walking:
                if (setAimingWhenStop) {
                    if (!IsMoving) {
                        if (IsEnemyVisible) {
                            PrepareToAiming();
                        }
                        else {
                            setAimingWhenStop = false;
                        }
                    }
                    break;
                }
                if (Input.GetMouseButtonDown(0)) {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit)) {
                        navMeshAgent.SetDestination(raycastHit.point);
                    }
                }
                if (IsMoving) {
                    navMeshAgent.speed = GameManager.instance.settings.playerSpeed;
                }
                break;
            case PlayerState.PrepareToAiming:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming")) {
                    state = PlayerState.Aiming;
                    readyToShoot = true;
                }
                break;
            case PlayerState.Aiming:
                setAimingWhenStop = false;
                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
                    if (readyToShoot) {
                        Shoot();
                    }
                }
                break;
        }
    }

    private void Shoot() {
        readyToShoot = false;
        StartCoroutine(Recharge());
        GameObject newBullet = Instantiate(bullet, bulletStartFrom.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(aimTargetPosition - bulletStartFrom.position) * GameManager.instance.settings.bulletSpeed;
        Destroy(newBullet, GameManager.instance.settings.bulletMaxDistance / GameManager.instance.settings.bulletSpeed);
    }

    private IEnumerator Recharge() {
        yield return new WaitForSeconds(GameManager.instance.settings.shootingTimeout);
        readyToShoot = true;
    }

    private void PrepareToAiming() {
        state = PlayerState.PrepareToAiming;
        animator.SetTrigger("StartAiming");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "ShootingPlace") {
            setAimingWhenStop = true;
        }
    }

    private void OnAnimatorIK(int layerIndex) {
        if (state != PlayerState.Aiming) {
            return;
        }
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit)) {
            aimTargetPosition = raycastHit.point;
        }

        animator.SetIKPosition(AvatarIKGoal.RightHand, aimTargetPosition);
    }

}
