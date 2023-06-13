using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class IsDead : EnemyCondition
    {
        public override TaskStatus OnUpdate()
        {
            return enemyController.CurrentHealth <= 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
