using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SpaceInvaders.Model.Ships;

namespace SpaceInvaders.Model
{
    /// <summary>Defines the behavior and attributes of EnemyMovementAnimator objects</summary>
    public class EnemyMovementAnimator
    {
        #region Data members

        private const int RotationDisplacement = 5;
        private const int InitialRotationDisplacement = -(RotationDisplacement / 2);
        private readonly Canvas background;
        private int rotationCounter;

        #endregion

        #region Properties

        /// <summary>Gets or sets the enemies to animate.</summary>
        /// <value>The enemies to animate.</value>
        public IEnumerable<EnemyShip> EnemiesToAnimate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyMovementAnimator" /> class.
        ///     Precondition: background != null, enemiesToAnimate != null
        /// </summary>
        /// <param name="background">The background.</param>
        /// <param name="enemiesToAnimate">The enemies to animate.</param>
        /// <exception cref="System.ArgumentException">
        ///     Background cannot be null
        ///     or
        ///     Enemies cannot be null
        /// </exception>
        public EnemyMovementAnimator(Canvas background, IList<EnemyShip> enemiesToAnimate)
        {
            this.background = background ?? throw new ArgumentException("Background cannot be null");
            this.EnemiesToAnimate = enemiesToAnimate ?? throw new ArgumentException("Enemies cannot be null");
        }

        #endregion

        #region Methods

        /// <summary>Animates the enemies.</summary>
        public void AnimateEnemies()
        {
            foreach (var enemyShip in this.selectEnemiesToAnimate())
            {
                if (this.rotationCounter == 0)
                {
                    rotate(enemyShip, InitialRotationDisplacement);
                }
                else if (this.rotationCounter % 2 == 0)
                {
                    rotate(enemyShip, RotationDisplacement);
                }
                else
                {
                    rotate(enemyShip, -RotationDisplacement);
                }
            }

            this.displayObjects();
            this.rotationCounter++;
        }

        private static void rotate(EnemyShip enemy, int angle)
        {
            var rotation = new RotateTransform {Angle = angle};
            enemy.Sprite.RenderTransformOrigin = new Point(.5, .5);
            enemy.Sprite.RenderTransform = rotation;
        }

        private void displayObjects()
        {
            this.removeOldEnemies();
            foreach (var enemy in this.EnemiesToAnimate)
            {
                this.background.Children.Add(enemy.Sprite);
            }
        }

        private void removeOldEnemies()
        {
            foreach (var enemy in this.EnemiesToAnimate)
            {
                this.background.Children.Remove(enemy.Sprite);
            }
        }

        private IList<EnemyShip> selectEnemiesToAnimate()
        {
            var enemiesToAnimate = from enemy in this.EnemiesToAnimate
                                   where enemy.EnemyLevel == EnemyType.Level2 || enemy.EnemyLevel == EnemyType.Level3 ||
                                         enemy.EnemyLevel == EnemyType.Level4
                                   select enemy;
            return enemiesToAnimate.ToList();
        }

        #endregion
    }
}