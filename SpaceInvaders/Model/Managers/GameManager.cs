using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Gaming.Input;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.View.Sprites;
using SpaceInvaders.View.Sprites.ShipSprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Types and Delegates

        public delegate void PlayerDeathHandler();

        public delegate void PlayerHitHandler(int livesRemaining);

        public delegate void ScoreboardUpdateHandler(int score);

        public delegate void PlayerWinHandler();

        #endregion

        #region Data members

        private const double PlayerShipBottomOffset = 30;
        private const int Row1YPosition = 25;
        private const int Row2YPosition = 75;
        private const int Row3YPosition = 125;
        private const int Row4YPosition = 175;

        private const int NumberOfLevel1Enemies = 4;
        private const int NumberOfLevel2Enemies = 4;
        private const int NumberOfLevel3Enemies = 4;

        private readonly EnemyFleetManager enemyFleetManager;

        private DispatcherTimer gameTimer;
        private int tickCounter;
        
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        
        private PlayerShip playerShip;
        private Canvas background;

        #endregion

        #region Properties
        
        /// <summary>
        ///     Gets or sets the user's score
        /// </summary>
        /// <value>The score.</value>
        public int Score { get; set; }
        
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

            this.enemyFleetManager = new EnemyFleetManager();

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
            this.background = background ?? throw new ArgumentNullException(nameof(background));

            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Tick += this.timerTick;
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.gameTimer.Start();

            this.createAndPlacePlayerShip();

            this.placeAllEnemies();
            this.showAllEnemies();
        }

        public event ScoreboardUpdateHandler ScoreboardUpdated;
        public event PlayerDeathHandler PlayerKilled;
        public event PlayerWinHandler PlayerWon;
        public event PlayerHitHandler PlayerHit;

        private void timerTick(object sender, object e)
        {
            this.tickCounter++;
            this.checkIfPlayerWasHit();
            this.enemyFleetManager.MoveEnemies();
            //this.animateMovement();
            this.showEnemyBullets();
            this.moveAllEnemyBullets();
            this.enemyFleetManager.RemoveOffScreenEnemyBulletsBullets(this.backgroundHeight);
            this.MoveAllPlayerBullets();
            this.playerShip.RemoveOffScreenPlayerBullets();
            this.DetectPlayerHitsAndIncreaseScore();
            this.checkIfPlayerWon();
        }

        private void createAndPlacePlayerShip()
        {
            this.playerShip = new PlayerShip();
            this.background.Children.Add(this.playerShip.Sprite);

            this.placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            this.playerShip.X = this.backgroundWidth / 2 - this.playerShip.Width / 2.0;
            this.playerShip.Y = this.backgroundHeight - this.playerShip.Height - PlayerShipBottomOffset;
        }

        private void placeAllEnemies()
        {
            var enemy1Counter = 0;
            var enemy2Counter = 0;
            var enemy3Counter = 0;
            var enemy4Counter = 0;

            foreach (var enemy in this.enemyFleetManager.EnemyShips)
            {
                var level = enemy.EnemyLevel;
                var centerOffset = enemy.Width;

                if (level == EnemyType.Level1)
                {
                    centerOffset = centerOffset * EnemyFleetManager.NumberOfLevel1Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy1Counter - centerOffset;
                    enemy.Y = Row4YPosition;
                    enemy1Counter++;
                }
                else if (level == EnemyType.Level2)
                {
                    centerOffset = centerOffset * EnemyFleetManager.NumberOfLevel2Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy2Counter - centerOffset;
                    enemy.Y = Row3YPosition;
                    enemy2Counter++;
                }
                else if (level == EnemyType.Level3)
                {
                    centerOffset = centerOffset * EnemyFleetManager.NumberOfLevel3Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy3Counter - centerOffset;
                    enemy.Y = Row2YPosition;
                    enemy3Counter++;
                }
                else if (level == EnemyType.Level4)
                {
                    centerOffset = centerOffset * EnemyFleetManager.NumberOfLevel4Enemies / 2;
                    enemy.X = this.backgroundWidth / 2 + enemy.Width * enemy4Counter - centerOffset;
                    enemy.Y = Row1YPosition;
                    enemy4Counter++;
                }
            }
        }

        private void showAllEnemies()
        {
            foreach (var enemy in this.enemyFleetManager.EnemyShips)
            {
                this.background.Children.Add(enemy.Sprite);
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
            else
            {
                this.playerShip.X = 0;
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
            else
            {
                this.playerShip.X = this.backgroundWidth - this.playerShip.Width;
            }
        }

        /// <summary>Fires the bullet if one has not already been fired.</summary>
        /// <param name="background">The background.</param>
        public void FireBullet()
        {
            if (this.playerShip.CanFire() && this.gameTimer.IsEnabled)
            {
                var bullet = this.playerShip.FireBullet();
                this.placePlayerBullet(bullet);
                this.createPlayerBullet(bullet);
            }
        }

        private void placePlayerBullet(PlayerBullet bullet)
        {
            var x = this.playerShip.X + this.playerShip.Width / 2 - bullet.Width / 2;
            var y = this.playerShip.Y - bullet.Height;

            bullet.X = x;
            bullet.Y = y;
        }

        private void createPlayerBullet(PlayerBullet bullet)
        {
            this.background.Children.Add(bullet.Sprite);
        }

        /// <summary>Moves all player bullets.</summary>
        public void MoveAllPlayerBullets()
        {
            foreach (var bullet in this.playerShip.PlayerBullets)
            {
                bullet.MoveUp();
            }
        }

        /// <summary>Detects the player hits and increases the score as necessary.</summary>
        /// <returns> the EnemyShip if the PlayerShip hit one; null otherwise </returns>
        public EnemyShip DetectPlayerHitsAndIncreaseScore()
        {
            foreach (var enemy in this.enemyFleetManager.EnemyShips)
            {
                var bullet = this.playerShip.DetermineIfEnemyWasHit(enemy);
                if (bullet != null)
                {
                    this.background.Children.Remove(bullet.Sprite);
                    this.determineScore(enemy);
                    this.removeDeadEnemy(enemy);
                    return enemy;
                }
            }

            return null;
        }

        private void removeDeadEnemy(EnemyShip enemy)
        {
            this.enemyFleetManager.EnemyShips.Remove(enemy);
            this.background.Children.Remove(enemy.Sprite);
        }

        private void determineScore(EnemyShip enemy)
        {
            this.Score += enemy.PointValue;
            this.ScoreboardUpdated?.Invoke(this.Score);
        }
        
        private void showEnemyBullets()
        {
            var bullet = this.enemyFleetManager.EnemiesShoot();

            if (bullet != null)
            {
                this.background.Children.Add(bullet.Sprite);
            }
        }
        
        /// <summary>Moves all enemy bullets.</summary>
        private void moveAllEnemyBullets()
        {
            if (this.enemyFleetManager.EnemyBullets.Count > 0)
            {
                foreach (var bullet in this.enemyFleetManager.EnemyBullets)
                {
                    bullet.MoveDown();
                }
            }
        }

        private void checkIfPlayerWasHit()
        {
            EnemyBullet bullet = this.enemyFleetManager.DetermineIfPlayerWasHit(this.playerShip);
            if (bullet != null)
            {
                if (this.playerShip.LivesRemaining <= 0)
                {
                    this.PlayerKilled?.Invoke();
                    this.background.Children.Remove(this.playerShip.Sprite);
                    this.gameTimer.Stop();
                }
                else
                {
                    this.playerShip.LivesRemaining--;
                    this.PlayerHit?.Invoke(this.playerShip.LivesRemaining);
                }

                this.background.Children.Remove(bullet.Sprite);
            }
        }

        private void checkIfPlayerWon()
        {
            if (this.enemyFleetManager.EnemyShips.Count == 0)
            {
                this.PlayerWon?.Invoke();
                this.gameTimer.Stop();
            }
        }

        private void animateMovement()
        {
            this.buildSecondFrames();
        }

        private void buildSecondFrames()
        {
            var enemiesToAnimate = from enemy in this.enemyFleetManager.EnemyShips
                                   where enemy.EnemyLevel == EnemyType.Level2 || enemy.EnemyLevel == EnemyType.Level3 || enemy.EnemyLevel == EnemyType.Level4
                                   select enemy;
            foreach (var enemy in enemiesToAnimate.ToList())
            {
                EnemyShip frame2 = new PassiveEnemyShip(0);
                frame2.X = enemy.X;
                frame2.Y = enemy.Y;
                
                this.background.Children.Add(frame2.Sprite);
                this.background.Children.Remove(enemy.Sprite);
                this.background.Children.Add(enemy.Sprite);
            }
        }
        #endregion
    }
}