using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the fields and behaviors of PlayerBullet objects</summary>
    public class PlayerBullet : GameObject
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="PlayerBullet" /> class.</summary>
        public PlayerBullet()
        {
            Sprite = new PlayerBulletSprite();
            SetSpeed(0, 20);
        }

        #endregion
    }
}