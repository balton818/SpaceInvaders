using System;

namespace SpaceInvaders.Model
{
    /// <summary>Provides functionality for determining if two objects have collided</summary>
    public abstract class CollisionDetector
    {
        #region Methods

        /// <summary>Determines whether a collision has occurred between the two objects</summary>
        /// <param name="target">The target.</param>
        /// <param name="bullet">The bullet.</param>
        /// <returns>true if the objects have collided; false otherwise</returns>
        /// <exception cref="System.ArgumentException">
        ///     Target object cannot be null
        ///     or
        ///     Bullet object cannot be null
        /// </exception>
        public static bool CollisionHasOccurred(GameObject target, GameObject bullet)
        {
            if (target == null)
            {
                throw new ArgumentException("Target object cannot be null");
            }

            if (bullet == null)
            {
                throw new ArgumentException("Bullet object cannot be null");
            }

            return target.X < bullet.X + bullet.Width && target.X + target.Width > bullet.X
                                                      && target.Y < bullet.Y + bullet.Height &&
                                                      target.Height + target.Y > bullet.Y;
        }

        #endregion
    }
}