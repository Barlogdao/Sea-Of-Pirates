using Cysharp.Threading.Tasks;
using Project.Interfaces.Enemies;

namespace Project.Spawner
{
    public class BossSpawner : BaseEnemySpawner
    {
        //[HorizontalLine(3f, EColor.Blue)]
        
       public override void Prepare()
        {
        }

        protected override void OnPrepareForQuest()
        {
            Spawn();
        }

        protected override void OnEnemyDied(IEnemy enemy)
        {
            base.OnEnemyDied(enemy);

            IPoolableEnemy poolEnemy = enemy as IPoolableEnemy;
            poolEnemy.SinkAsync().Forget();
        }
    }
}