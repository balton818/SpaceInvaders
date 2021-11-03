using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpaceInvaders.Model.Ships;

namespace SpaceInvaders.Model.Managers
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Types and Delegates

        /// <summary>the PlayerDeathHandler</summary>
        public delegate void PlayerDeathHandler();

        /// <summary>the PlayerHitHandler</summary>
        /// <param name="livesRemaining">The lives remaining.</param>
        public delegate void PlayerHitHandler(int livesRemaining);

        /// <summary>the PlayerWinHandler</summary>
        public delegate void PlayerWinHandler();

        /// <summary>the ScoreboardUpdateHandler</summary>
        /// <param name="score">The score.</param>
        public delegate void ScoreboardUpdateHandler(int score);

        #endregion

        #region Data members

        private const double PlayerShipBottomOffset = 30;
        private const int Row1YPosition = 25;
        private const int Row2YPosition = 75;
        private const int Row3YPosition = 125;
        private const int Row4YPosition = 175;

        private readonly EnemyFleetManager enemyFleetManager;
        private EnemyMovementAnimator animator;

        private DispatcherTimer gameTimer;

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
        ///     Precondition: gameBackground != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gameBackground">The gameBackground canvas.</param>
        public void InitializeGame(Canvas gameBackground)
        {
            this.background = gameBackground ?? throw new ArgumentNullException(nameof(gameBackground));

            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Tick += this.timerTick;
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.gameTimer.Start();

            this.createNewAnimator();

            this.createAndPlacePlayerShip();

            this.placeAllEnemies();
        }

        private void createNewAnimator()
        {
            this.animator = new EnemyMovementAnimator(this.background, this.enemyFleetManager.EnemyShips);
        }

        /// <summary>Occurs when the score is updated</summary>
        public event ScoreboardUpdateHandler ScoreboardUpdated;

        /// <summary>Occurs when the player is killed.</summary>
        public event PlayerDeathHandler PlayerKilled;

        /// <summary>Occurs when the player has won.</summary>
        public event PlayerWinHandler PlayerWon;

        /// <summary>Occurs when the player is hit by an enemy ship.</summary>
        public event PlayerHitHandler PlayerHit;

        private void timerTick(object sender, object e)
        {
            this.checkIfPlayerWasHit();
            this.enemyFleetManager.MoveEnemies();
            this.animateEnemyMovement();
            this.showEnemyBullets();
            this.moveAllEnemyBullets();
            this.enemyFleetManager.RemoveOffScreenEnemyBullets(this.backgroundHeight);
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

        /// <summary>Fires the bullet if the player ship is able to</summary>
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
            var bullet = this.enemyFleetManager.DetermineIfPlayerWasHit(this.playerShip);
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

        private void animateEnemyMovement()
        {
            this.animator.AnimateEnemies();
        }

        #endregion
    }
}