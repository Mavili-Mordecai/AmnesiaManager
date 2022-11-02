using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AmnesiaManager.Animations
{
    internal class Slide
    {
        public static void Left(FrameworkElement animationObject, EventHandler? completed = null, float to = 0, float time = 175f)
        {
            var animation = new DoubleAnimation
            {
                From = -animationObject.ActualWidth,
                To = to,
                Duration = TimeSpan.FromMilliseconds(time),
                EasingFunction = new QuarticEase()
            };

            if (completed != null) animation.Completed += completed;
            animationObject.BeginAnimation(Canvas.RightProperty, animation);
        }

        public static void Right(FrameworkElement animationObject, EventHandler? completed = null, float time = 175f)
        {
            var animation = new DoubleAnimation
            {
                From = 0,
                To = -animationObject.ActualWidth,
                Duration = TimeSpan.FromMilliseconds(time),
                EasingFunction = new QuarticEase()
            };

            if (completed != null) animation.Completed += completed;
            animationObject.BeginAnimation(Canvas.RightProperty, animation);
        }
    }
}