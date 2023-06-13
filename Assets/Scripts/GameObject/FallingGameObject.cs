using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class FallingGameObject : MonoBehaviour
{
    [SerializeField]
    private float checkDistance;

    [SerializeField]
    private float speed;

    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private ParticleSystem smokeEffect;

    [SerializeField]
    private GameObject destoryEffect;

    private PolygonCollider2D collider2D;

    private bool canDamage;
    private bool isPlayerComing;
    private bool canMove;
    private bool isFirst;

    private void Start()
    {
        collider2D = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        CheckPlayerAndGround();
        MoveFunc();
    }

    // 射线检测玩家经过
    private void CheckPlayerAndGround()
    {
        isPlayerComing = Physics2D.Raycast(transform.position, -transform.up, checkDistance, whatIsPlayer);

        if (isPlayerComing && !isFirst) //只执行一次
        {
            isFirst = !isFirst;
            smokeEffect.Play();
            StartCoroutine(waitSeconds());
        }
    }

    private void MoveFunc()
    {
        if (canMove)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            canDamage = false;
            PlayerData.Instance.ReduceHealth();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            smokeEffect.Stop();
            StartCoroutine(OnGround());
            return;
        }


    }

    public void OnAttack()
    {
        this.gameObject.SetActive(false);
        collider2D.enabled = false;
        if (PlayerController.Instance.IsFackingLeft)
        {
            GameObject effect = Instantiate(destoryEffect, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            Destroy(effect, 3.5f);
        }
        else
        {
            GameObject effect = Instantiate(destoryEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3.5f);
        }
    }

    private IEnumerator waitSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDamage = true;
    }

    private IEnumerator OnGround()
    {
        yield return new WaitForSeconds(0.01f);
        canMove = false;
        canDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, new Vector3(transform.position.x, transform.position.y + (-transform.up.y * checkDistance), transform.position.z));
    }

}
