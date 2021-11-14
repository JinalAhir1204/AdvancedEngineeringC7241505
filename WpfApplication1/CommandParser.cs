using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace WpfApplication1
{
    public class CommandParser
    {
        String Command = "";
        int penLocationX = 0;
        int penLocationY = 0;
        Color penColor = Colors.Black;
        bool fill = false;
        String invalidcommand = "This is invalid command";
        String invalArg = "This command has invalid parameters";
        /// <summary>
        /// This is command parser set method
        /// </summary>
        /// <param name="command"></param>
        public void setCommandParser(string command)
        {
           if (checkCommand(command) == true)
            {
                this.Command = command;
            }
            else
            {
                this.Command = "";
            }
        }

        /// <summary>
        /// This function  excute the valid commad
        /// </summary>
        public void executeCommand()
        {
            if (this.Command != "")
            {
                string[] commands = this.Command.Split(' ');
                switch (commands[0])
                {
                    case ("moveto"):
                        drawMoveTo(commands);
                        break;
                    case ("drawto"):
                        drawDrawTo(commands);
                        break;
                    case ("clear"):
                        drawClear();
                        break;
                    case ("reset"):
                        drawReset();
                        break;
                    case ("rectangle"):
                        drawRectangle(commands);
                        break;
                    case ("circle"):
                        drawCircle(commands);
                        break;
                    case ("triangle"):
                        drawTriangle(commands);
                        break;
                    case ("pen"):
                        drawPen(commands);
                        break;
                    case ("fill"):
                        drawFill(commands);
                        break;

                }
            }
        }
        /// <summary>
        /// This function draw the rectangle
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void drawRectangle(int width, int height)
        {
            shapeFactory factory = null;
            factory = new shapeRectangleFactory(this.penLocationX, this.penLocationY, this.penColor, this.fill, width, height, this.penColor);
            shapeRectangle rectangleShape = (shapeRectangle)factory.GetShape();
            Rectangle rect;//This is a graphics object. Its parameters will be set from the Rectangle Factory Object
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(rectangleShape.penColor);
            if (rectangleShape.fill)
            {
                rect.Fill = new SolidColorBrush(rectangleShape.penColor);
            }
            rect.Width = rectangleShape.width;
            rect.Height = rectangleShape.height;
            Canvas.SetLeft(rect, rectangleShape.penLocationX);
            Canvas.SetTop(rect, rectangleShape.penLocationY);
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(rect);
                    (window as MainWindow).txtOutput.Text = (window as MainWindow).txtOutput.Text + "\n" + "Rectangle Draw";
                }
            }
        }
        /// <summary>
        /// This function draw the circle
        /// </summary>
        /// <param name="radius"></param>
        public void drawCircle(int radius)
        {
            Ellipse ellp;
            ellp = new Ellipse();
            ellp.Stroke = new SolidColorBrush(this.penColor);
            if (this.fill)
            {
                ellp.Fill = new SolidColorBrush(this.penColor);
            }
            ellp.Width = radius * 2;
            ellp.Height = radius * 2;
            ellp.SetValue(Canvas.LeftProperty, (double)this.penLocationX);
            ellp.SetValue(Canvas.TopProperty, (double)this.penLocationY);
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(ellp);
                }
            }
        }
        /// <summary>
        /// This function draw the triangle
        /// </summary>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        public void drawTriangle(int x2, int y2, int x3, int y3)
        {
            Line l1, l2, l3;
            l1 = new Line();
            l1.Stroke = new SolidColorBrush(this.penColor);
            l1.StrokeThickness = 2;
            l1.X1 = this.penLocationX;
            l1.X2 = x2;
            l1.Y1 = this.penLocationY;
            l1.Y2 = y2;

            l2 = new Line();
            l2.Stroke = new SolidColorBrush(this.penColor);
            l2.StrokeThickness = 2;
            l2.X1 = x2;
            l2.X2 = x3;
            l2.Y1 = y2;
            l2.Y2 = y3;

            l3 = new Line();
            l3.Stroke = new SolidColorBrush(this.penColor);
            l3.StrokeThickness = 2;
            l3.X1 = this.penLocationX;
            l3.X2 = x3;
            l3.Y1 = this.penLocationY;
            l3.Y2 = y3;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(l1);
                    (window as MainWindow).myCanvas.Children.Add(l2);
                    (window as MainWindow).myCanvas.Children.Add(l3);
                }
            }

        }
        /// <summary>
        /// This function draw the line
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public void drawLine(int x1, int y1)
        {
            Line l1;
            l1 = new Line();
            l1.Stroke = new SolidColorBrush(this.penColor);
            l1.StrokeThickness = 2;
            l1.X1 = this.penLocationX;
            l1.X2 = x1;
            l1.Y1 = this.penLocationY;
            l1.Y2 = y1;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(l1); ;
                }
            }
        }
        /// <summary>
        /// This function check the commad is valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkCommand(String command)
        {
            if (commandIsValid(command) == true && commandHasValidArgs(command) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// This boolen function for commands are valid or not 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool commandIsValid(String command)
        {
            //List of valid commands:
            //moveto
            //drawto
            //clear
            //reset
            //rectangle
            //circle
            //triangle
            //pen
            //fill
            if (command.StartsWith("moveto") ||
                command.StartsWith("drawto") ||
                command.StartsWith("clear") ||
                command.StartsWith("reset") ||
                command.StartsWith("rectangle") ||
                command.StartsWith("circle") ||
                command.StartsWith("triangle") ||
                command.StartsWith("pen") ||
                command.StartsWith("fill"))
            {
                return true;
            }
            else
            {
                inValidCommand(invalidcommand);
                return false;
            }

        }
        /// <summary>
        /// This is moveto function for change the position of pointer
        /// </summary>
        /// <param name="commands"></param>
        public void drawMoveTo(string[] commands)
        {
            int x1 = Int32.Parse(commands[1]);
            int y1 = Int32.Parse(commands[2]);
            this.penLocationX = x1;
            this.penLocationY = y1;

        }
        /// <summary>
        /// This function check the moveto command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkmoveto(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("moveto") && (commands.Length == 3))
            {
                bool firstArg = false;
                bool secondArg = false;
                try
                {
                    int m = Int32.Parse(commands[1]);
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[2]);
                    secondArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                if (firstArg && secondArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
;
                return false;
            }
        }
        /// <summary>
        /// If command or params are invalid then this function will execute
        /// </summary>
        public void inValidCommand(String text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(textBlock, 10.00);
            Canvas.SetTop(textBlock, 10.00);
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(textBlock);
                    (window as MainWindow).txtOutput.Text = (window as MainWindow).txtOutput.Text + "\n" + "Invalid";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public void drawDrawTo(string[] commands)
        {
            int x2 = Int32.Parse(commands[1]);
            int y2 = Int32.Parse(commands[2]);
            drawLine(x2, y2);

        }
        /// <summary>
        /// This function check the drawto command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkdrawto(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("drawto") && (commands.Length == 3))
            {
                bool firstArg = false;
                bool secondArg = false;
                try
                {
                    int m = Int32.Parse(commands[1]);
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[2]);
                    secondArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                if (firstArg && secondArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
               
                return false;
            }
        }
        /// <summary>
        /// This function draw the rectangle
        /// </summary>
        /// <param name="commands"></param>
        public void drawRectangle(string[] commands)
        {
            int width = Int32.Parse(commands[1]);
            int height = Int32.Parse(commands[2]);
            drawRectangle(width, height);
        }
        /// <summary>
        /// This function check the rectangle command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkrectangle(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("rectangle") && (commands.Length == 3))
            {
                bool firstArg = false;//width
                bool secondArg = false;//height
                try
                {
                    int m = Int32.Parse(commands[1]);
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[2]);
                    secondArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                if (firstArg && secondArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
              
                return false;
            }
        }
        /// <summary>
        /// This function draw the circle
        /// </summary>
        /// <param name="commands"></param>
        public void drawCircle(string[] commands)
        {
            int m = Int32.Parse(commands[1]);
            drawCircle(m);
        }
        /// <summary>
        /// This function check the circle command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkcircle(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("circle") && (commands.Length == 2))
            {
                bool firstArg = false;
                try
                {
                    int m = Int32.Parse(commands[1]);
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (firstArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
              
                return false;
            }
        }
        /// <summary>
        /// This function for draw triangle
        /// </summary>
        /// <param name="commands"></param>
        public void drawTriangle(string[] commands)
        {
            int x2 = Int32.Parse(commands[1]);
            int y2 = Int32.Parse(commands[2]);
            int x3 = Int32.Parse(commands[3]);
            int y3 = Int32.Parse(commands[4]);
            drawTriangle(x2, y2, x3, y3);
        }
        /// <summary>
        /// This function check the the tranagle shape valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checktriangle(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("triangle") && (commands.Length == 5))
            {
                bool firstArg = false;
                bool secondArg = false;
                bool thirdArg = false;
                bool fourthArg = false;

                try
                {
                    int m = Int32.Parse(commands[1]);
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[2]);
                    secondArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[3]);
                    thirdArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(commands[4]);
                    fourthArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                if (firstArg && secondArg && thirdArg && fourthArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
               
                return false;
            }
        }
        /// <summary>
        /// This function draw the shape by the pen color 
        /// </summary>
        /// <param name="commands">this is the command</param>
        public void drawPen(string[] commands)
        {
            if (commands[1] == "red")
            {
                this.penColor = Colors.Red;
            }
            else if (commands[1] == "blue")
            {
                this.penColor = Colors.Blue;
            }
            else if (commands[1] == "green")
            {
                this.penColor = Colors.Green;
            }
            else
            {
                this.penColor = Colors.Black;
            }
        }
        /// <summary>
        /// This function check the pen command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkpen(String command)
        {
           
            string[] commands = command.Split(' ');
            if (commands[0].Contains("pen") && (commands.Length == 2))
            {
                bool firstArg = false;
                try
                {
                    if (commands[1] == "red" || commands[1] == "blue" || commands[1] == "green")
                    {
                        firstArg = true;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (firstArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
               
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public void drawFill(string[] commands)
        {
            if (commands[1] == "on")
            { this.fill = true; }
            else if (commands[1] == "off")
            { this.fill = false; }
            else { this.fill = false; }

        }
        /// <summary>
        /// This function check the clear command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkfill(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("fill") && (commands.Length == 2))
            {
                bool firstArg = false;
                try
                {
                    if (commands[1] == "on" || commands[1] == "off")
                    {
                        firstArg = true;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (firstArg)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// This function clear the drawing canavs
        /// </summary>
        public void drawClear()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Clear();
                }
            }
        }
        /// <summary>
        /// This function check the clear command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkclear(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("clear") && (commands.Length == 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  This command reset the position 0
        /// </summary>
        public void drawReset()
        {
            this.penLocationX = 0;
            this.penLocationY = 0;
            this.penColor = Colors.Black;

        }

        /// <summary>
        /// This function check the Reset command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkreset(String command)
        {
            string[] commands = command.Split(' ');
            if (commands[0].Contains("reset") && (commands.Length == 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// This function check the valid command's Parameter
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool commandHasValidArgs(String command)
        {
            if (command.StartsWith("moveto"))
            {
                return checkmoveto(command);
            }
            else if (command.StartsWith("drawto"))
            {
                return checkdrawto(command);
            }
            else if (command.StartsWith("clear"))
            {
                return checkclear(command);
            }
            else if (command.StartsWith("reset"))
            {
                return checkreset(command);
            }
            else if (command.StartsWith("rectangle"))
            {
                return checkrectangle(command);
            }
            else if (command.StartsWith("circle"))
            {
                return checkcircle(command);
            }
            else if (command.StartsWith("triangle"))
            {
                return checktriangle(command);
            }
            else if (command.StartsWith("pen"))
            {
                return checkpen(command);
            }
            else if (command.StartsWith("fill"))
            {
                return checkfill(command);
            }
            else
            {
                inValidCommand(invalArg);
                return false;
            }

        }
    }
}
