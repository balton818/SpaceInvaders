using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the behaviors and fields of EnemyBullet objects</summary>
    public class EnemyBullet : GameObject
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="EnemyBullet" /> class.</summary>
        public EnemyBullet()
        {
            Sprite = new EnemyBulletSprite();
            SetSpeed(0, 10);
        }

        #endregion
    }
}