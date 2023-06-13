using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class MoveTo : EnemyAction
    {
        [Tooltip("移动的目标位置")]
        public SharedTransform targetPos;

        [Tooltip("移动速度")]
        public float speed;

        public override TaskStatus OnUpdate()
        {
            // 判断转向
            if (Vector3.SqrMagnitude(transform.position - targetPos.Value.position) < 0.1f)
            {
                enemyAnim.SetTrigger("IsTurn");
                transform.Rotate(0, 180, 0);
                enemyController.IsFacingRight = !enemyController.IsFacingRight;
                return TaskStatus.Success;
            }

            // 击退状态不能移动
            if (!enemyController.IsEnemyKnockBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos.Value.position, speed * Time.deltaTime);
            }
            return TaskStatus.Running;
        }
    }
}
