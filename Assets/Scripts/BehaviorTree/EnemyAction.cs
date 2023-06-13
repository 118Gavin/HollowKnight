using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class EnemyAction : Action
    {
        [HideInInspector]
        public Animator enemyAnim;

        [HideInInspector]
        public EnemyController enemyController;
        
        [HideInInspector]
        public PlayerController playerController;

        [HideInInspector]
        public EnemyFindPlayer enemyFindPlayer;

        public override void OnStart()
        {
            enemyAnim = gameObject.GetComponent<Animator>();
            enemyController = gameObject.GetComponent<EnemyController>();
            playerController = PlayerController.Instance;
            enemyFindPlayer = GetComponent<EnemyFindPlayer>();
        }

    }
}
