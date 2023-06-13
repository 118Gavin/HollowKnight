using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BugMoveTo : Action
    {
        [Tooltip("移动的目标位置")]
        public SharedTransform targetPos;

        [Tooltip("移动速度")]
        public float speed;

        public override TaskStatus OnUpdate()
        {
            // 如果俩个点的距离无限接近>0.1f，表示还没到该点
            while (Vector3.SqrMagnitude(transform.position - targetPos.Value.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos.Value.position, speed * Time.deltaTime);
                return TaskStatus.Running;
            }

            return TaskStatus.Success;
        }
    }
}
