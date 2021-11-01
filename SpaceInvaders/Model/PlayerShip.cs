using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class PlayerShip : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 20;
        private const int SpeedYDirection = 0;

        #endregion

        #region Properties

        /// <summary>Gets or sets the player bullets.</summary>
        /// <value>The player bullets.</value>
        public IList<PlayerBullet> PlayerBullets { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {
            Sprite = new PlayerShipSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
            this.PlayerBullets = new List<PlayerBullet>(1);
        }

        #endregion

        #region Methods

        /// <summary>Determines whether the PlayerShip can move left.</summary>
        /// <returns>
        ///     <c>true</c> if this instance can move left; otherwise, <c>false</c>.
        /// </returns>
        public bool CanMoveLeft()
        {
            return X - SpeedX >= 0;
        }

        /// <summary>Determines whether this PlayerShip can move right</summary>
        /// <param name="windowWidth">Width of the window.</param>
        /// <returns>
        ///     <c>true</c> if this instance can move right; otherwise, <c>false</c>.
        /// </returns>
        public bool CanMoveRight(double windowWidth)
        {
            return X + Width + SpeedX <= windowWidth;
        }

        /// <summary>Determines whether the player hit the enemy</summary>
        /// <param name="enemy">The enemy.</param>
        /// <returns>true if the enemy was hit; false otherwise</returns>
        /// <exception cref="System.ArgumentException">Enemy object cannot be null</exception>
        public PlayerBullet DetermineIfEnemyWasHit(EnemyShip enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentException("Enemy object cannot be null");
            }

            foreach (var bullet in this.PlayerBullets)
            {
                if (CollisionDetector.CollisionHasOccurred(enemy, bullet))
                {
                    this.PlayerBullets.Remove(bullet);
                    return bullet;
                }
            }

            return null;
        }

        /// <summary>Determines whether the PlayerShip can fire.</summary>
        /// <returns>
        ///     <c>true</c> if the PlayerShip can fire; otherwise, <c>false</c>.
        /// </returns>
        public bool CanFire()
        {
            return this.PlayerBullets.Count <= 3 && this.checkProperBulletSpacing();
        }

        private bool checkProperBulletSpacing()
        {
            if (this.PlayerBullets.Count > 0)
            {
                var lastBulletPosition = this.PlayerBullets[this.PlayerBullets.Count - 1].Y;
                return this.Y - lastBulletPosition > this.PlayerBullets[this.PlayerBullets.Count - 1].Height;
            }

            return true;
        }

        /// <summary>Fires the bullet.</summary>
        /// <returns>the bullet that was fired</returns>
        public PlayerBullet FireBullet()
        {
            var bullet = new PlayerBullet();
            this.PlayerBullets.Add(bullet);
            return bullet;
        }

        public void RemoveOffScreenPlayerBullets()
        {
            foreach (var bullet in this.findOffscreenBullets())
            {
                this.PlayerBullets.Remove(bullet);
            }
        }

        private IList<PlayerBullet> findOffscreenBullets()
        {
            var bulletsToRemove = new List<PlayerBullet>();

            foreach (var bullet in this.PlayerBullets)
            {
                if (bullet.Y <= 0)
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            return bulletsToRemove;
        }

        #endregion
    }
}