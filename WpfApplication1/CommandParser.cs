using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Data;

namespace WpfApplication1
{
    [Serializable]
    class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }

        public InvalidCommandException(string command) : base(String.Format("This is my custom Invalid Command message: {0}", command))
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public  class CommandParser
    {
        public List<string> programBlocks = new List<string>();
        public Dictionary<string, string> variables_values = new Dictionary<string, string>();
        public List<string> reservedKeywords = new List<string>();
        String Command;
        int penLocationX;
        int penLocationY;
        Color penColor;
        bool fill;
        String invalidcommand;
        String invalArg;
        public Dictionary<string, string> method_block = new Dictionary<string, string>();//methodname, methodstatements
        public List<string> singleCommandsList = new List<string>();
        public List<string> programBlockKeywords = new List<string>();

        /// <summary>
        /// This is the constructor for the CommandParser Class
        /// </summary>
        public CommandParser()
        {
            Command = "";
            penLocationX = 0;
            penLocationY = 0;
            penColor = Colors.Black;
            fill = false;
            invalidcommand = "This is invalid command";
            invalArg = "This command has invalid parameters";

            singleCommandsList.Add("moveto ");
            singleCommandsList.Add("drawto ");
            singleCommandsList.Add("clear");
            singleCommandsList.Add("reset");
            singleCommandsList.Add("rectangle ");
            singleCommandsList.Add("circle ");
            singleCommandsList.Add("triangle ");
            singleCommandsList.Add("pen ");
            singleCommandsList.Add("fill ");
            singleCommandsList.Add("Var ");

            programBlockKeywords.Add("While ");
            programBlockKeywords.Add("EndWhile\n");
            programBlockKeywords.Add("If ");
            programBlockKeywords.Add("EndIf\n");
            programBlockKeywords.Add("Method ");
            programBlockKeywords.Add("EndMethod\n");
            programBlockKeywords.Add("LoopFor ");
            programBlockKeywords.Add("EndLoopFor\n");
            programBlockKeywords.Add("Var ");

            reservedKeywords.Add("moveto");
            reservedKeywords.Add("drawto");
            reservedKeywords.Add("clear");
            reservedKeywords.Add("reset");
            reservedKeywords.Add("rectangle");
            reservedKeywords.Add("circle");
            reservedKeywords.Add("triangle");
            reservedKeywords.Add("pen");
            reservedKeywords.Add("fill");
            reservedKeywords.Add("While");
            reservedKeywords.Add("EndWhile");
            reservedKeywords.Add("If");
            reservedKeywords.Add("EndIf");
            reservedKeywords.Add("Method");
            reservedKeywords.Add("EndMethod");
            reservedKeywords.Add("LoopFor");
            reservedKeywords.Add("EndLoopFor");
            reservedKeywords.Add("Var");

           
        }
        /// <summary>
        /// This function checks for Program blocks such as If EndIf, Method EndMethod, LoopFor EndLoopFor
        /// </summary>
        /// <param name="text">A string containing (multiple) lines</param>
        /// <returns>True if a programBlock keyword is found. False otherwise.</returns>
        public bool checkforProgramBlocks(string text)
        {
            bool retVal = false;
            foreach (string obj in programBlockKeywords)
            {
                if (text.Contains(obj))
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
        /// <summary>
        /// This function checks for Reserved keywords used in variable names
        /// </summary>
        /// <param name="text">A string containing (multiple) lines</param>
        /// <returns>True if a reserved keyword is found in the variable names</returns>
        public bool checkforReservedKeywords(string text)
        {
            bool retVal = false;
            foreach (string obj in reservedKeywords)
            {
                if (text.Trim().Equals(obj))
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
        /// <summary>
        /// This function takea an input string to evaluate as an expression
        /// </summary>
        /// <param name="eqn">string equation such as "10+5" or "10*2"</param>
        /// <returns>Object which can be parsed to Int, Float, etc.</returns>
        public object evaluateExpression(string eqn)
        {
            DataTable dt = new DataTable();
            var result = dt.Compute(eqn, string.Empty);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool singleLineExecutableCommand(string command)
        {
            bool retVal = false;
            foreach (string obj in singleCommandsList.ToArray())
            {
                if (command.StartsWith(obj))
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkCommandContainsVariableName(string command)
        {
            bool retVal = false;
            foreach (KeyValuePair<string, string> obj in variables_values.ToArray())
            {
                if (command.Contains(obj.Key))
                {
                    retVal = true;
                }
            }
            return retVal;

        }

        /// <summary>
        /// This is command parser set method
        /// </summary>
        /// <param name="command"></param>
        public void setCommandParser(string command)
        {
            //This is a constructor. It is not being used as of now
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
            if (this.Command != "" && this.Command.Trim().Length > 0)
            {
                this.Command=this.Command.Replace("moveto ", "moveto$");
                this.Command = this.Command.Replace("drawto ", "drawto$");
                //this.Command=this.Command.Replace("clear");
                //this.Command=this.Command.Replace("reset");
                this.Command = this.Command.Replace("rectangle ", "rectangle$");
                this.Command = this.Command.Replace("circle ", "circle$");
                this.Command = this.Command.Replace("triangle ", "triangle$");
                this.Command = this.Command.Replace("pen ", "pen$");
                this.Command = this.Command.Replace("fill ", "fill$");
                //this.Command=this.Command.Replace("Var ","Var$");

                string[] commands = this.Command.Split('$');
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
            if (command.Trim().Length > 0)
            {
                if (commandIsValid(command) == true && commandHasValidArgs(command) == true)
                {
                    Console.WriteLine(variables_values.ToArray().Length.ToString());
                    return true;

                }
                else if (commandIsValid(command) == true && commandHasValidArgs(command) == false)
                {
                    try
                    {

                        inValidCommand(invalArg, 10.00, 30.00, command);
                    }
                    catch (InvalidCommandException ex)
                    {
                        MessageBox.Show("Exception: "+ ex);
                    }
                    return false;
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
            if (command.StartsWith("moveto ") ||
                command.StartsWith("drawto ") ||
                command.StartsWith("clear") ||
                command.StartsWith("reset") ||
                command.StartsWith("rectangle ") ||
                command.StartsWith("circle ") ||
                command.StartsWith("triangle ") ||
                command.StartsWith("pen ") ||
                command.StartsWith("fill ") ||
                command.StartsWith("Var "))
            {
                return true;
            }
            else if (command.Contains('='))
            {
                assignVarNameWithItsValueonRHS(command);
                return true;

            }
            else {
               
               try
                    {

                        inValidCommand(invalidcommand, 10.00, 30.00, command);
                    }
                    catch (InvalidCommandException ex)
                    {
                        MessageBox.Show("Exception: "+ ex);
                    }
                return false;
            }

        }
        /// <summary>
        /// This is moveto function for change the position of pointer
        /// </summary>
        /// <param name="commands"></param>
        public void drawMoveTo(string[] commands)
        {
            int x1 = Int32.Parse(evaluateExpression(commands[1]).ToString());
            int y1 = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
                    int m = Int32.Parse(evaluateExpression(commands[1]).ToString());
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
        /// If command or params are invalid then this function will execute
        /// </summary>
        public void inValidCommand(String text, double x, double y, string command)
        {


            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).myCanvas.Children.Add(textBlock);
                    (window as MainWindow).txtOutput.Text = (window as MainWindow).txtOutput.Text + "\n" + command + " Invalid";
                }
            }
            throw new InvalidCommandException(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public void drawDrawTo(string[] commands)
        {
            int x2 = Int32.Parse(evaluateExpression(commands[1]).ToString());
            int y2 = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
                    int m = Int32.Parse(evaluateExpression(commands[1]).ToString());
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
            int width = Int32.Parse(evaluateExpression(commands[1]).ToString());
            int height = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
                    int m = Int32.Parse(evaluateExpression(commands[1]).ToString());
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(commands[2]).ToString());
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
        public string replaceVarNameWithItsValueInCommand(string command)
        {
            if (command.Contains('=') && !command.Contains("=="))
            {
                return assignVarNameWithItsValueonRHS(command);
            }
            else
            {
                foreach (KeyValuePair<string, string> obj in variables_values.ToArray())
                {
                    if (command.Contains(obj.Key))//contains is a bug here because reservedKeywords can be substrings of varNames (obj.Key) 
                    {
                        command = command.Replace(obj.Key, obj.Value);
                    }
                }
                return command;
            }
        }
        public string assignVarNameWithItsValueonRHS(string command)
        {
            string[] left_right = command.Split('=');
            string left = left_right[0].Trim();
            string right = left_right[1].Trim();
            int value = 0;
            foreach (KeyValuePair<string, string> obj in variables_values.ToArray())
            {
                if (right.Contains(obj.Key))//contains is a bug here because reservedKeywords can be substrings of varNames (obj.Key) 
                {
                    string cmd = replaceVarNameWithItsValueInCommand(right);
                    value = (int)evaluateExpression(cmd);
                    variables_values[obj.Key] = value.ToString();
                }
                else {
                    value = (int)evaluateExpression(right);

                }
            }
            return left + "=" + value;

        }
        /// <summary>
        /// This function draw the circle
        /// </summary>
        /// <param name="commands"></param>
        public void drawCircle(string[] commands)
        {
            int m = Int32.Parse(evaluateExpression(commands[1]).ToString());
            drawCircle(m);
        }
        /// <summary>
        /// This function check the circle command valid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkcircle(String command)
        {
            string[] commands = command.Replace("circle ","circle$").Split('$');
            if (commands[0].Equals("circle") && (commands.Length == 2))
            {
                bool firstArg = false;
                try
                {

                    int m = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[1])).ToString());
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
            int x2 = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[1])).ToString());
            int y2 = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[2])).ToString());
            int x3 = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[3])).ToString());
            int y3 = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[4])).ToString());
            drawTriangle(x2, y2, x3, y3);
        }
        /// <summary>
        /// 
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
                    int m = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[1])).ToString());
                    firstArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[2])).ToString());
                    secondArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[3])).ToString());
                    thirdArg = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    int m = Int32.Parse(evaluateExpression(replaceVarNameWithItsValueInCommand(commands[4])).ToString());
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
        /// <param name="commands"></param>
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
                    if (commands[1] == "red" || commands[1] == "blue")
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
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool checkvar(String command)
        {
            command = command.Replace("Var ", "Var$");
            while (command.Contains("  "))//we ensure that command only contain a single space as separator
            {
                command = command.Replace("  ", " ");
            }

            while (command.Contains("\t"))//we ensure that command only contain a single space as separator
            {
                command = command.Replace("\t", " ");
            }
            while (command.Contains(" "))//we ensure that command only contain a single space as separator
            {
                command = command.Replace(" ", "");
            }
            string[] commands = command.Split('$');
            if (commands[0].Contains("Var") && (commands.Length == 2))
            {
                string[] varVal = commands[1].Split('=');
                string varName = varVal[0];
                string varValue = evaluateExpression(replaceVarNameWithItsValueInCommand(varVal[1])).ToString(); 
                try
                {
                    if (variables_values.ContainsKey(varName))
                    {
                        variables_values[varName] = varValue;
                    }
                    else
                    {
                        variables_values.Add(varName, varValue);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
        /// This command reset the position 0
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
            if (command.StartsWith("moveto "))
            {
                return checkmoveto(command);
            }
            else if (command.StartsWith("drawto "))
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
            else if (command.StartsWith("rectangle "))
            {
                return checkrectangle(command);
            }
            else if (command.StartsWith("circle "))
            {
                return checkcircle(command);
            }
            else if (command.StartsWith("triangle "))
            {
                return checktriangle(command);
            }
            else if (command.StartsWith("pen "))
            {
                return checkpen(command);
            }
            else if (command.StartsWith("fill "))
            {
                return checkfill(command);
            }
            else if (command.StartsWith("Var "))
            {
                return checkvar(command);
            }
            else if (command.Contains("="))
            {
                assignVarNameWithItsValueonRHS(command);
                return true;//checkvarAssignment(command);->this function is yet to be implemented
            }
            else
            {
                return false;
            }

        }
    }
}