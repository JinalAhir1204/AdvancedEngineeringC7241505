using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1
{
    class shapeRectangle : Shape
    {
        private readonly string _shapeType;
        private int _penLocationX;
        private int _penLocationY;
        private Color _penColor;
        private bool _fill;
        private int _width;
        private int _height;
        private Color _fillColor;
       
        public override string ShapeType
        {
            get { return _shapeType; }
        }

        public override int penLocationX
        {
            get { return _penLocationX; }
            set { _penLocationX = value; }
        }

        public override int penLocationY
        {
            get { return _penLocationY; }
            set { _penLocationY = value; }
        }
        public override Color penColor
        {
            get { return _penColor; }
            set { _penColor = value; }
        }
        public override bool fill
        {
            get { return _fill; }
            set { _fill = value; }
        }

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int height
        {
            get { return _height; }
            set { _height = value; }
        }
        public Color fillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        public shapeRectangle(int penLocationX, int penLocationY, Color penColor, bool fill, int width, int height, Color fillColor)  
        {
            _shapeType = "Rectangle";
            _penLocationX = penLocationX;
            _penLocationY = penLocationY;
            _penColor = penColor;
            _fill = fill;
            _width = width;
            _height = height;
            _fillColor = fillColor;
  
        }  

    }
}
