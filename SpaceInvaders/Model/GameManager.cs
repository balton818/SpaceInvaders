using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const double PlayerShipBottomOffset = 30;
        private const int EnemyShipTopOffset = 50;

        private const int NumberOfLevel1Enemies = 4;
        private const int NumberOfLevel2Enemies = 4;
        private const int NumberOfLevel3Enemies = 4;

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private bool movingLeft;
        private int enemyMoveCounter;

        private PlayerShip playerShip;

        #endregion

        #region Properties

        /// <summary>Gets the enemies currently in play.</summary>
        /// <value>The enemies.</value>
        public IList<EnemyShip> Enemies { get; }

        /// <summary>Gets the player bullets currently in play.</summary>
        /// <value>The player bullets.</value>
        public PlayerBullet PlayerBullet { get; set; }

        /// <summary>Gets or sets the enemy bullets.</summary>
        /// <value>The enemy bullets.</value>
        public IList<EnemyBullet> EnemyBullets { get; set; }

        /// <summary>
        ///     Gets or sets the user's score
        /// </summary>
        /// <value>The score.</value>
        public int Score { get; set; }

        public delegate void PlayerDeathHandler(bool gameOver);

        public event PlayerDeathHandler GameOver;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="backgroundHeight">The backgroundHeight of the game play window.</param>
        /// <param name="backgroundWidth">The backgroundWidth of the game play window.</param>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.Enemies = new List<EnemyShip>();
            this.EnemyBullets = new List<EnemyBullet>();

            this.movingLeft = true;
            this.enemyMoveCounter = 0;
            this.Score = 0;

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the game placing player ship and enemy ship in the game.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }

            this.createAndPlacePlayerShip(background);

            this.createLevel1Enemies(background);
            this.createLevel2Enemies(background);
            this.createLevel3Enemies(background);

            this.placeAllEnemies();
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            this.playerShip = new PlayerShip();
            background.Children.Add(this.playerShip.Sprite);

            this.placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            this.playerShip.X = this.backgroundWidth / 2 - this.playerShip.Width / 2.0;
            this.playerShip.Y = this.backgroundHeight - this.playerShip.Height - PlayerShipBottomOffset;
        }

        private void createLevel1Enemies(Canvas background)
        {
            for (var index = 0; index < NumberOfLevel1Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip1();
                background.Children.Add(enemy.Sprite);
                this.Enemies.Add(enemy);
            }
        }

        private void createLevel2Enemies(Canvas background)
        {
            for (var index = 0; index < NumberOfLevel2Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip2();
                background.Children.Add(enemy.Sprite);
                this.Enemies.Add(enemy);
            }
        }

        private void createLevel3Enemies(Canvas background)
        {
            for (var index = 0; index < NumberOfLevel3Enemies; index++)
            {
                EnemyShip enemy = new EnemyShip3();
                background.Children.Add(enemy.Sprite);
                this.Enemies.Add(enemy);
            }
        }

        private void placeAllEnemies()
        {
            var enemy1Counter = 0;
            var enemy2Counter = 0;
            var enemy3Counter = 0;

            foreach (var enemy in this.Enemies)
            {
                var type = enemy.GetType();
                var centerOffset = enemy.Width;

                if (type == typeof(EnemyShip1))
                {
                    centerOffset = centerOffset * NumberOfLevel1Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy1Counter - centerOffset;
                    enemy.Y = EnemyShipTopOffset * 3;
                    enemy1Counter++;
                }
                else if (type == typeof(EnemyShip2))
                {
                    centerOffset = centerOffset * NumberOfLevel2Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy2Counter - centerOffset;
                    enemy.Y = EnemyShipTopOffset * 2;
                    enemy2Counter++;
                }
                else if (type == typeof(EnemyShip3))
                {
                    centerOffset = centerOffset * NumberOfLevel3Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy3Counter - centerOffset;
                    enemy.Y = EnemyShipTopOffset;
                    enemy3Counter++;
                }
            }
        }

        /// <summary>
        ///     Moves all enemies and changes movement direction if necessary.
        ///     Precondition: none
        ///     Postcondition: all enemies moved to the left or right
        /// </summary>
        public void MoveAllEnemies()
        {
            foreach (var enemy in this.Enemies)
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

            if (this.enemyMoveCounter == 20)
            {
                this.movingLeft = !this.movingLeft;
                this.enemyMoveCounter = 0;
            }
        }

        /// <summary>
        ///     Moves the player ship to the left.
        ///     Precondition: none
        ///     Postcondition: The player ship has moved left.
        /// </summary>
        public void MovePlayerShipLeft()
        {
            if (this.playerShip.CanMoveLeft())
            {
                this.playerShip.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player ship to the right.
        ///     Precondition: none
        ///     Postcondition: The player ship has moved right.
        /// </summary>
        public void MovePlayerShipRight()
        {
            if (this.playerShip.CanMoveRight(this.backgroundWidth))
            {
                this.playerShip.MoveRight();
            }
        }

        /// <summary>Fires the bullet if one has not already been fired.</summary>
        /// <param name="background">The background.</param>
        public void FireBullet(Canvas background)
        {
            if (this.playerShip.CanFire())
            {
                var bullet = this.playerShip.FireBullet();
                this.createPlayerBullet(background, bullet);
                this.PlayerBullet = bullet;
                this.placePlayerBullet(bullet);
            }
            else
            {
                this.removePlayerBullet();
            }
        }

        private void placePlayerBullet(PlayerBullet bullet)
        {
            var x = this.playerShip.X + this.playerShip.Width / 2 - bullet.Width / 2;
            var y = this.playerShip.Y - bullet.Height;

            bullet.X = x;
            bullet.Y = y;
        }

        private void createPlayerBullet(Canvas background, PlayerBullet bullet)
        {
            background.Children.Add(bullet.Sprite);
        }

        /// <summary>Moves all player bullets.</summary>
        public void MoveAllPlayerBullets()
        {
            foreach (var bullet in this.playerShip.PlayerBullets)
            {
                bullet.MoveUp();
            }
        }

        private void removePlayerBullet()
        {
            var bullet = this.playerShip.PlayerBullets[0];
            if (this.playerShip.PlayerBullets[0].Y <= 0)
            {
                this.playerShip.PlayerBullets.Remove(bullet);
            }
        }

        /// <summary>Detects the player hits and increases the score as necessary.</summary>
        /// <returns> the EnemyShip if the PlayerShip hit one; null otherwise </returns>
        public EnemyShip DetectPlayerHitsAndIncreaseScore()
        {
            foreach (var enemy in this.Enemies)
            {
                if (this.playerShip.HitEnemy(enemy))
                {
                    this.Enemies.Remove(enemy);
                    this.removePlayerBullet();
                    this.determineScore(enemy);
                    return enemy;
                }
            }

            return null;
        }

        private void determineScore(EnemyShip enemy)
        {
            this.Score += enemy.PointValue;
        }

        /// <summary>Determines if player was hit by an enemy ship</summary>
        /// <returns>true if the player was hit; false otherwise</returns>
        public bool DetermineIfPlayerWasHit()
        {
            foreach (var enemy in this.Enemies)
            {
                var type = enemy.GetType();
                if (type == typeof(EnemyShip3) && ((EnemyShip3) enemy).HitPlayerShip(this.playerShip))
                {
                    this.onGameOver();
                    return true;
                }
            }

            return false;
        }

        /// <summary>Enables the enemies to fire randomly</summary>
        /// <param name="background">The background.</param>
        public void EnemiesShoot(Canvas background)
        {
            if (this.randomShotFired())
            {
                var enemy = this.pickRandomEnemy();
                var bullet = enemy.FireBullet();
                this.EnemyBullets.Add(bullet);
                this.placeEnemyBullet(bullet, enemy);
                this.showEnemyBullet(bullet, background);
            }
        }

        private void placeEnemyBullet(EnemyBullet bullet, EnemyShip3 enemy)
        {
            var x = enemy.X + enemy.Width / 2 - bullet.Width / 2;
            var y = enemy.Y + enemy.Height;

            bullet.X = x;
            bullet.Y = y;
        }

        private EnemyShip3 pickRandomEnemy()
        {
            var random = new Random();
            var level3Enemies = from enemy in this.Enemies where enemy.GetType() == typeof(EnemyShip3) select enemy;
            var index = random.Next(level3Enemies.Count());
            var randomEnemy = level3Enemies.ToList()[index];
            
            return (EnemyShip3) randomEnemy;
        }

        private void showEnemyBullet(EnemyBullet bullet, Canvas background)
        {
            background.Children.Add(bullet.Sprite);
        }

        private bool randomShotFired()
        {
            var random = new Random();
            var value = random.Next(200);

            return value > 20 && value < 35;
        }

        /// <summary>Moves all enemy bullets.</summary>
        public void MoveAllEnemyBullets()
        {
            if (this.EnemyBullets.Count > 0)
            {
                foreach (var bullet in this.EnemyBullets)
                {
                    bullet.MoveDown();
                }
            }
        }

        private void onGameOver()
        {
            if (this.GameOver != null)
            {
                this.GameOver(true);
            }
        }

        #endregion
    }
}