using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class FacingPlayer : EnemyAction
    {
        public int direction;
        public override TaskStatus OnUpdate()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            var scale = transform.localScale;
            scale.x = direction;
            transform.localScale = scale;

            return TaskStatus.Success;
        }
    }
}
