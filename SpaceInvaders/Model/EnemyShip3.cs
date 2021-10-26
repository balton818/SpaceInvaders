using System;
using System.Collections.Generic;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the behavior and fields of EnemyShip3 objects</summary>
    public class EnemyShip3 : EnemyShip
    {
        #region Data members

        /// <summary>The enemy bullets</summary>
        public IList<EnemyBullet> EnemyBullets;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="EnemyShip3" /> class.</summary>
        public EnemyShip3()
        {
            Sprite = new Level3EnemyShipSprite();
            SetSpeed(5, 5);
            PointValue = 10;
            this.EnemyBullets = new List<EnemyBullet>();
        }

        #endregion

        #region Methods

        /// <summary>Fires the bullet.</summary>
        /// <returns>the bullet that was fired</returns>
        public EnemyBullet FireBullet()
        {
            var bullet = new EnemyBullet();
            this.EnemyBullets.Add(bullet);
            return bullet;
        }

        /// <summary>Determines whether the enemy ship has hit the player ship</summary>
        /// <param name="player">The player.</param>
        /// <returns>true if the player ship was hit; false otherwise</returns>
        /// <exception cref="System.ArgumentException">Player object cannot be null</exception>
        public bool HitPlayerShip(PlayerShip player)
        {
            if (player == null)
            {
                throw new ArgumentException("Player object cannot be null");
            }

            if (this.EnemyBullets.Count > 0)
            {
                foreach (var bullet in this.EnemyBullets)
                {
                    if (CollisionDetector.CollisionHasOccurred(player, bullet))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}