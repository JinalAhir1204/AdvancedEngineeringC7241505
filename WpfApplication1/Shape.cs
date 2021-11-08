using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfApplication1
{
    /// <summary>
    /// The Shape Factory. For shapes such as Point, Line, Polygon, Circle, etc.
    /// </summary>
    public abstract class Shape
    {
        public abstract string ShapeType { get; }
        public abstract int penLocationX { get; set; }
        public abstract int penLocationY { get; set; }
        public abstract Color penColor {get; set;}
        public abstract bool fill { get; set; }
    }
}
