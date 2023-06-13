using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class SearchPlayer : EnemyCondition
    {
        // 如果碰到玩家返回成功否则失败
        public override TaskStatus OnUpdate()
        {
            if (enemyFindPlayer.isTouchingPlayer)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

    }
}
