using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MyApp.Utilities
{
    public class ClippedBorder : Border
    {
        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (Child != value)
                {
                    if (Child != null)
                    {
                        // Restore original clipping
                        Child.SetValue(ClipProperty, _oldClip);
                    }

                    if (value != null)
                    {
                        _oldClip = value.ReadLocalValue(ClipProperty);
                    }
                    else
                    {
                        // If we dont set it to null we could leak a Geometry object
                        _oldClip = null;
                    }

                    base.Child = value;
                }
            }
        }

        protected virtual void OnApplyChildClip()
        {
            UIElement child = Child;
            if (child != null)
            {
                _clipRect.RadiusX = _clipRect.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - BorderThickness.Left * 0.5);
                _clipRect.Rect = new Rect(Child.RenderSize);
                child.Clip = _clipRect;
            }
        }

        private RectangleGeometry _clipRect = new RectangleGeometry();
        private object _oldClip;
    }
}
