using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class EnemyCondition : Conditional
    {
        [HideInInspector]
        public EnemyController enemyController;

        [HideInInspector]
        public EnemyFindPlayer enemyFindPlayer;

        public override void OnStart()
        {
            enemyController = GetComponent<EnemyController>();
            enemyFindPlayer = GetComponent<EnemyFindPlayer>();
        }
    }
}
