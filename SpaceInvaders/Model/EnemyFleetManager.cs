using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Model
{
    public class EnemyFleetManager
    {
        public const int NumberOfLevel1Enemies = 4;
        public const int NumberOfLevel2Enemies = 4;
        public const int NumberOfLevel3Enemies = 4;
        private const int numberOfEnemySteps = 20;
        private const int lowerProbabilityBound = 20;
        private const int upperProbabilityBound = 30;

        private bool movingLeft;
        private int enemyMoveCounter;

        public IList<EnemyShip> EnemyShips { get; set; }

        public IList<EnemyBullet> EnemyBullets { get; private set; }

        public EnemyFleetManager()
        {
            this.EnemyShips = new List<EnemyShip>();
            this.EnemyBullets = new List<EnemyBullet>();

            this.createLevel1Enemies();
            this.createLevel2Enemies();
            this.createLevel3Enemies();

            this.movingLeft = true;
        }

        private void createLevel1Enemies()
        {
            for (var index = 0; index < NumberOfLevel1Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip1();
                this.EnemyShips.Add(enemy);
            }
        }

        private void createLevel2Enemies()
        {
            for (var index = 0; index < NumberOfLevel2Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip2();
                this.EnemyShips.Add(enemy);
            }
        }

        private void createLevel3Enemies()
        {
            for (var index = 0; index < NumberOfLevel3Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip3();
                this.EnemyShips.Add(enemy);
            }
        }

        public void MoveEnemies()
        {
            foreach (var enemy in this.EnemyShips)
            {
                if (this.movingLeft)
                {
                    enemy.MoveLeft();
                }
                else
                {
                    enemy.MoveRight();
                }
            }

            this.enemyMoveCounter++;

            if (this.enemyMoveCounter == numberOfEnemySteps)
            {
                this.movingLeft = !this.movingLeft;
                this.enemyMoveCounter = 0;
            }
        }

        public bool DetermineIfPlayerWasHit(PlayerShip player)
        {
            foreach (var bullet in this.EnemyBullets)
            {
                if (CollisionDetector.CollisionHasOccurred(player, bullet))
                {
                    return true;
                }
            }

            return false;
        }
        
        public EnemyBullet EnemiesShoot()
        {
            if (this.randomShotFired())
            {
                var enemy = this.pickRandomEnemy();
                var bullet = enemy.FireBullet();
                this.EnemyBullets.Add(bullet);
                this.setBulletCoordinates(enemy, bullet);

                return bullet;
            }

            return null;
        }

        private void setBulletCoordinates(EnemyShip enemy, EnemyBullet bullet)
        {
            var x = enemy.X + enemy.Width / 2 - bullet.Width / 2;
            var y = enemy.Y + enemy.Height;

            bullet.X = x;
            bullet.Y = y;
        }

        private EnemyShip3 pickRandomEnemy()
        {
            if (this.EnemyShips.Count > 0)
            {
                var random = new Random();
                var level3Enemies = from enemy in this.EnemyShips where enemy.GetType() == typeof(EnemyShip3) select enemy;
                var index = random.Next(level3Enemies.Count());
                var randomEnemy = level3Enemies.ToList()[index];

                return (EnemyShip3)randomEnemy;
            }

            return null;

        }

        private bool randomShotFired()
        {
            if (this.EnemyShips.Count > 0)
            {
                var random = new Random();
                var value = random.Next(100);

                return value > lowerProbabilityBound && value < upperProbabilityBound;
            }

            return false;
        }

        public void RemoveOffScreenEnemyBulletsBullets(double height)
        {
            foreach (var bullet in this.findOffscreenBullets(height))
            {
                this.EnemyBullets.Remove(bullet);
            }
        }

        private IList<EnemyBullet> findOffscreenBullets(double height)
        {
            IList<EnemyBullet> bulletsToRemove = new List<EnemyBullet>();
            foreach (var bullet in this.EnemyBullets)
            {
                if (bullet.Y > height)
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            return bulletsToRemove;
        }
    }
}
