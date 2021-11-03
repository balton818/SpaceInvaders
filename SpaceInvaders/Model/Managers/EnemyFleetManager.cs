using System;
using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.Model.Ships;

namespace SpaceInvaders.Model.Managers
{

    /// <summary>Defines the behavior and attributes of EnemyFleetManager class objects</summary>
    public class EnemyFleetManager
    {
        #region Data members

        /// <summary>The number of level 1 enemies in the game</summary>
        public const int NumberOfLevel1Enemies = 2;

        /// <summary>The number of level 2 enemies in the game</summary>
        public const int NumberOfLevel2Enemies = 4;

        /// <summary>The number of level 3 enemies in the game</summary>
        public const int NumberOfLevel3Enemies = 6;

        /// <summary>The number of level 4 enemies in the game</summary>
        public const int NumberOfLevel4Enemies = 8;

        private const int NumberOfEnemySteps = 20;

        private const int LowerProbabilityBound = 20;
        private const int UpperProbabilityBound = 30;

        private bool movingLeft;
        private int enemyMoveCounter;

        #endregion

        #region Properties

        /// <summary>Gets or sets the enemy ships.</summary>
        /// <value>The enemy ships.</value>
        public IList<EnemyShip> EnemyShips { get; set; }

        /// <summary>Gets the enemy bullets.</summary>
        /// <value>The enemy bullets.</value>
        public IList<EnemyBullet> EnemyBullets { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="EnemyFleetManager" /> class.</summary>
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

        /// <summary>Moves the enemies.</summary>
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

            if (this.enemyMoveCounter == NumberOfEnemySteps)
            {
                this.movingLeft = !this.movingLeft;
                this.enemyMoveCounter = 0;
            }
        }

        /// <summary>Determines if player was hit.</summary>
        /// <param name="player">The player.</param>
        /// <returns>true if the player was hit; false otherwise</returns>
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

        /// <summary>Gives the fire command to the enemies in the game</summary>
        /// <returns>returns the EnemyBullet object if one was fired; null otherwise</returns>
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

        /// <summary>Removes the offscreen enemy bullets.</summary>
        /// <param name="height">The height.</param>
        public void RemoveOffScreenEnemyBullets(double height)
        {
            foreach (var bullet in this.findOffscreenBullets(height))
            {
                this.EnemyBullets.Remove(bullet);
            }
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
                var index = random.Next(aggressiveEnemies.Count);
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

                return value > LowerProbabilityBound && value < UpperProbabilityBound;
            }

            return false;
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