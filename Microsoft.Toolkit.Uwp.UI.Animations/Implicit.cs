// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Attached Properties to enable Implicit Animations through XAML
    /// </summary>
    public class Implicit
    {
        /// <summary>
        /// Identifies the Implicit.ShowAnimations XAML attached property
        /// </summary>
        public static readonly DependencyProperty ShowAnimationsProperty =
            DependencyProperty.RegisterAttached("ShowAnimations", typeof(AnimationCollection), typeof(Implicit), new PropertyMetadata(null, ShowAnimationsChanged));

        /// <summary>
        /// Identifies the Implicit.HideAnimations XAML attached property
        /// </summary>
        public static readonly DependencyProperty HideAnimationsProperty =
            DependencyProperty.RegisterAttached("HideAnimations", typeof(AnimationCollection), typeof(Implicit), new PropertyMetadata(null, HideAnimationsChanged));

        /// <summary>
        /// Identifies the Implicit.Animations XAML attached property
        /// </summary>
        public static readonly DependencyProperty AnimationsProperty =
            DependencyProperty.RegisterAttached("Animations", typeof(AnimationCollection), typeof(Implicit), new PropertyMetadata(null, AnimationsChanged));

        /// <summary>
        /// Gets the value of the Implicit.ShowAnimations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to get the value from</param>
        /// <returns><see cref="AnimationCollection"/></returns>
        public static AnimationCollection GetShowAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(ShowAnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(ShowAnimationsProperty, collection);
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the Implicit.ShowAnimations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to set the value</param>
        /// <param name="value">The <see cref="AnimationCollection"/> to set</param>
        public static void SetShowAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(ShowAnimationsProperty, value);
        }

        /// <summary>
        /// Gets the value of the Implicit.HideAnimations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to get the value from</param>
        /// <returns><see cref="AnimationCollection"/></returns>
        public static AnimationCollection GetHideAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(HideAnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(HideAnimationsProperty, collection);
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the Implicit.HideAnimations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to set the value</param>
        /// <param name="value">The <see cref="AnimationCollection"/> to set</param>
        public static void SetHideAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(HideAnimationsProperty, value);
        }

        /// <summary>
        /// Gets the value of the Implicit.Animations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to get the value from</param>
        /// <returns><see cref="AnimationCollection"/></returns>
        public static AnimationCollection GetAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(AnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(AnimationsProperty, collection);
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the Implicit.Animations XAML attached property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to set the value</param>
        /// <param name="value">The <see cref="AnimationCollection"/> to set</param>
        public static void SetAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(AnimationsProperty, value);
        }

        private static void ShowAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!AnimationBase.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= ShowCollectionChanged;
            }

            if (e.NewValue is AnimationCollection animationCollection && d is UIElement element)
            {
                animationCollection.Parent = element;
                animationCollection.AnimationCollectionChanged -= ShowCollectionChanged;
                animationCollection.AnimationCollectionChanged += ShowCollectionChanged;
                ElementCompositionPreview.SetImplicitShowAnimation(element, GetCompositionAnimationGroup(animationCollection, element));
            }
        }

        private static void HideAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!AnimationBase.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= HideCollectionChanged;
            }

            if (e.NewValue is AnimationCollection animationCollection && d is UIElement element)
            {
                animationCollection.Parent = element;
                animationCollection.AnimationCollectionChanged -= HideCollectionChanged;
                animationCollection.AnimationCollectionChanged += HideCollectionChanged;
                ElementCompositionPreview.SetImplicitHideAnimation(element, GetCompositionAnimationGroup(animationCollection, element));
            }
        }

        private static void AnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= AnimationsCollectionChanged;
            }

            if (e.NewValue is AnimationCollection animationCollection && d is UIElement element)
            {
                animationCollection.Parent = element;
                animationCollection.AnimationCollectionChanged -= AnimationsCollectionChanged;
                animationCollection.AnimationCollectionChanged += AnimationsCollectionChanged;
                ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = GetImplicitAnimationCollection(animationCollection, element);
            }
        }

        private static void ShowCollectionChanged(object sender, EventArgs e)
        {
            if (!AnimationBase.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var collection = sender as AnimationCollection;
            if (collection.Parent == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitShowAnimation(collection.Parent, GetCompositionAnimationGroup(collection, collection.Parent));
        }

        private static void HideCollectionChanged(object sender, EventArgs e)
        {
            if (!AnimationBase.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var collection = sender as AnimationCollection;
            if (collection.Parent == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitHideAnimation(collection.Parent, GetCompositionAnimationGroup(collection, collection.Parent));
        }

        private static void AnimationsCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as AnimationCollection;
            if (collection.Parent == null)
            {
                return;
            }

            ElementCompositionPreview.GetElementVisual(collection.Parent).ImplicitAnimations =
                                            GetImplicitAnimationCollection(collection, collection.Parent);
        }

        private static CompositionAnimationGroup GetCompositionAnimationGroup(AnimationCollection collection, UIElement element)
        {
            if (AnimationBase.IsCreatorsUpdateOrAbove && collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetCompositionAnimationGroup(element);
        }

        private static ImplicitAnimationCollection GetImplicitAnimationCollection(AnimationCollection collection, UIElement element)
        {
            if (AnimationBase.IsCreatorsUpdateOrAbove && collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetImplicitAnimationCollection(element);
        }
    }
}
