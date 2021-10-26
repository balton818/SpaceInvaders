namespace SpaceInvaders.Model
{
    /// <summary> Defines the data members and behavior for EnemyShip objects </summary>
    public abstract class EnemyShip : GameObject
    {
        #region Properties

        /// <summary>Gets or sets the point value for destroying the ship.</summary>
        /// <value>The point value.</value>
        public int PointValue { get; protected set; }

        #endregion

        #region Methods

        /// <summary>Sets the point value for the EnemyShip.</summary>
        /// <param name="points">The points.</param>
        public void SetPoints(int points)
        {
            this.PointValue = points;
        }

        #endregion
    }
}