using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class FollowPlayer : EnemyAction
    {
        public float speed;
        public override TaskStatus OnUpdate()
        {
            if ((transform.position.x > playerController.transform.position.x) && enemyController.IsFacingRight || (transform.position.x < playerController.transform.position.x) && !enemyController.IsFacingRight)
            {
                var scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
            else
            {
                var scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }

            // 判断转向
            if (Vector3.SqrMagnitude(transform.position - playerController.transform.position) < 0.1f)
            {
                return TaskStatus.Success;
            }

            // 击退状态不能移动
            if (!enemyController.IsEnemyKnockBack)
            {
                enemyAnim.SetTrigger("Attack");
                transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, speed * Time.deltaTime);
            }


            return TaskStatus.Running;
        }
    }
}
