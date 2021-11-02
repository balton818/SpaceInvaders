
using System.Collections.Generic;
using System.Drawing;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SpaceInvaders.Model
{
    
    public class MovementAnimator
    {
        private Canvas background;
        private int rotationCounter;

        private const int rotationDisplacement = 5;
        private const int intialRotationDisplacement = rotationDisplacement / 2;

        public IList<EnemyShip> objectsToAnimate { get; set; }

        public MovementAnimator(Canvas background, IList<EnemyShip> objectsToAnimate)
        {
            if (background != null)
            {
                this.background = background;
            }

            if (objectsToAnimate != null)
            {
                this.objectsToAnimate = objectsToAnimate;
            }
        }

        public void animateObjects()
        {
            foreach (var item in this.objectsToAnimate)
            {
                if (this.rotationCounter == 0)
                {
                    this.rotateLeftFromCenter(item);
                } else if (this.rotationCounter % 2 == 0)
                {
                    this.rotateRight(item);
                }
                else
                {
                     this.rotateLeft(item);
                }

            }
            this.displayObjects();
            this.rotationCounter++;
        }

        private void rotateLeftFromCenter(EnemyShip enemy)
        {
            RotateTransform rotation = new RotateTransform {Angle = -intialRotationDisplacement};
            enemy.Sprite.RenderTransformOrigin = new Windows.Foundation.Point(.5, .5);
            enemy.Sprite.RenderTransform = rotation;
        }

        private void rotateRight(EnemyShip enemy)
        {
            RotateTransform rotation = new RotateTransform { Angle = rotationDisplacement };
            enemy.Sprite.RenderTransformOrigin = new Windows.Foundation.Point(.5, .5);
            enemy.Sprite.RenderTransform = rotation;
        }

        private void rotateLeft(EnemyShip enemy)
        {
            RotateTransform rotation = new RotateTransform { Angle = -rotationDisplacement };
            enemy.Sprite.RenderTransformOrigin = new Windows.Foundation.Point(.5, .5);
            enemy.Sprite.RenderTransform = rotation;
        }

        private void displayObjects()
        {
            this.removeOldEnemies();
            foreach (var enemy in this.objectsToAnimate)
            {
                this.background.Children.Add(enemy.Sprite);   
            }
        }

        private void removeOldEnemies()
        {
            foreach (var enemy in this.objectsToAnimate)
            {
                this.background.Children.Remove(enemy.Sprite);
            }
        }
    }
}
