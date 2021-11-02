using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Model
{
    public class EnemyFleetManager
    {
        #region Data members

        public const int NumberOfLevel1Enemies = 2;
        public const int NumberOfLevel2Enemies = 4;
        public const int NumberOfLevel3Enemies = 6;
        public const int NumberOfLevel4Enemies = 8;

        private const int numberOfEnemySteps = 20;

        private const int lowerProbabilityBound = 20;
        private const int upperProbabilityBound = 30;

        private bool movingLeft;
        private int enemyMoveCounter;

        #endregion

        #region Properties

        public IList<EnemyShip> EnemyShips { get; set; }

        public IList<EnemyBullet> EnemyBullets { get; }

        #endregion

        #region Constructors

        public EnemyFleetManager()
        {
            this.EnemyShips = new List<EnemyShip>();
            this.EnemyBullets = new List<EnemyBullet>();

            this.createPassiveEnemies();
            this.createAggressiveEnemies();

            this.movingLeft = true;
            this.enemyMoveCounter = 0;
        }

        #endregion

        #region Methods

        private void createPassiveEnemies()
        {
            for (var index = 0; index < NumberOfLevel1Enemies; index++)
            {
                var enemy = new PassiveEnemyShip(1);
                this.EnemyShips.Add(enemy);
            }

            for (var index = 0; index < NumberOfLevel2Enemies; index++)
            {
                var enemy = new PassiveEnemyShip(2);
                this.EnemyShips.Add(enemy);
            }
        }

        private void createAggressiveEnemies()
        {
            EnemyShip enemy;
            for (var index = 0; index < NumberOfLevel3Enemies; index++)
            {
                enemy = new AggressiveEnemyShip(1);
                this.EnemyShips.Add(enemy);
            }

            for (var index = 0; index < NumberOfLevel4Enemies; index++)
            {
                enemy = new AggressiveEnemyShip(2);
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
        
        public EnemyBullet DetermineIfPlayerWasHit(PlayerShip player)
        {
            foreach (var bullet in this.EnemyBullets)
            {
                if (CollisionDetector.CollisionHasOccurred(player, bullet))
                {
                    this.EnemyBullets.Remove(bullet);
                    return bullet;
                }
            }

            return null;
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

        private AggressiveEnemyShip pickRandomEnemy()
        {
            var random = new Random();
            List<EnemyShip> aggressiveEnemies = null;
            if (this.EnemyShips.Count > 0)
            {
                var enemyShips = from enemy in this.EnemyShips
                                 where enemy.EnemyLevel == EnemyType.Level3 || enemy.EnemyLevel == EnemyType.Level4
                                 select enemy;
                aggressiveEnemies = enemyShips.ToList();
            }

            if (aggressiveEnemies?.Count > 0)
            {
                var index = random.Next(aggressiveEnemies.Count());
                var randomEnemy = aggressiveEnemies[index];
                return (AggressiveEnemyShip) randomEnemy;
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

        #endregion
    }
}