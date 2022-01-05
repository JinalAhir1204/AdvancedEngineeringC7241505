using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Data;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommandParser cmdParser = new CommandParser();
        //public List<string> programBlocks = new List<string>();
        public Dictionary<string, string> method_block = new Dictionary<string, string>();//methodname, methodstatements
        public List<string> singleCommandsList = new List<string>();
        public List<string> reservedKeywords = new List<string>();
        public List<string> programBlockKeywords = new List<string>();
        public string whileCondition = "";
        public MainWindow()
        {
            InitializeComponent();
            txtCommand.Focus();
            singleCommandsList.Add("moveto ");
            singleCommandsList.Add("drawto ");
            singleCommandsList.Add("clear");
            singleCommandsList.Add("reset");
            singleCommandsList.Add("rectangle ");
            singleCommandsList.Add("circle ");
            singleCommandsList.Add("triangle ");
            singleCommandsList.Add("pen ");
            singleCommandsList.Add("fill ");

            programBlockKeywords.Add("While ");
            programBlockKeywords.Add("EndWhile\n");
            programBlockKeywords.Add("If ");
            programBlockKeywords.Add("EndIf\n");
            programBlockKeywords.Add("Method ");
            programBlockKeywords.Add("EndMethod\n");
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
            reservedKeywords.Add("Var");
        }
        /// <summary>
        /// This Button Execute the Single line command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (txtCommand.Text.Trim() == "run")
            {
                foreach (string command in txtProgram.Text.Split('\n'))
                {
                    cmdParser.setCommandParser(command.Trim());
                    cmdParser.executeCommand();
                    txtOutput.Text = txtOutput.Text + "\n" + command.Trim();
                }
                txtCommand.Text = "";
            }
            else
            {
                //now we execute the above command
                cmdParser.setCommandParser(txtCommand.Text);
                cmdParser.executeCommand();
                txtOutput.Text = txtOutput.Text + "\n" + txtCommand.Text;
                txtCommand.Text = "";
                e.Handled = true;
            }
        }
        /// <summary>
        /// This is single line command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCommand_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (txtCommand.Text.Trim() == "run")
                {
                    foreach (string command in txtProgram.Text.Split('\n'))
                    {
                        cmdParser.setCommandParser(command.Trim());
                        cmdParser.executeCommand();
                        txtOutput.Text = txtOutput.Text + "\n" + command.Trim();
                    }
                    txtCommand.Text = "";
                }
                else
                {
                    //now we execute the above command
                    cmdParser.setCommandParser(txtCommand.Text);
                    cmdParser.executeCommand();
                    txtOutput.Text = txtOutput.Text + "\n" + txtCommand.Text;
                    txtCommand.Text = "";
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// This Button redirect to the saved commad file and select the pervious saved file and 
        /// load the previous command 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtProgram.Text = File.ReadAllText(openFileDialog.FileName);
        }

        /// <summary>
        /// This Button Save the Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            String filepath = ".\\" + "MyProgram_" + DateTime.Now.Ticks.ToString() + ".txt";
            File.WriteAllText(filepath, txtProgram.Text);
        }

        private bool checkforProgramBlocks(string text)
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

        private bool checkforReservedKeywords(string text)
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
        /// This button run the multiline commad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (checkforProgramBlocks(txtProgram.Text))
            {
                //from the program we first divide the program into appropriate code blocks... if endif, while endwhile, method endmethod
                //moveto
                //drawto
                //clear
                //reset
                //rectangle
                //circle
                //triangle
                //pen
                //fill
                //Var 
                string[] lines = txtProgram.Text.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    string codeblock = "";//we declare this here as everyline can be a code block

                    while (lines[i].Contains("  "))//we ensure that command only contain a single space as separator
                    {
                        lines[i] = lines[i].Replace("  ", " ");
                    }

                    while (lines[i].Contains("\t"))//we ensure that command only contain a single space as separator
                    {
                        lines[i] = lines[i].Replace("\t", " ");
                    }


                    if (lines[i].StartsWith("While "))//while condition
                    {
                        //code block finishes at EndWhile
                        for (int j = i; j < lines.Length; j++, i++)
                        {
                            codeblock = codeblock + lines[i];
                            if (lines[i].Trim().Equals("EndWhile"))
                            {
                                cmdParser.programBlocks.Add(codeblock);
                                break;
                            }
                        }
                        continue;
                    }
                    else if (lines[i].StartsWith("If "))//if condition
                    {
                        //code block finishes at EndIf
                        for (int j = i; j < lines.Length; j++, i++)
                        {
                            codeblock = codeblock + lines[i];
                            if (lines[i].Trim().Equals("EndIf"))
                            {
                                cmdParser.programBlocks.Add(codeblock);
                                break;
                            }
                        }
                        continue;
                    }
                    else if (lines[i].StartsWith("Method "))//method name
                    {
                        //code block finishes at EndWhile
                        for (int j = i; j < lines.Length; j++, i++)
                        {
                            codeblock = codeblock + lines[i];
                            if (lines[i].Trim().Equals("EndMethod"))
                            {
                                cmdParser.programBlocks.Add(codeblock.Trim());
                                addMethodBlock(codeblock.Trim());
                                break;
                            }
                        }
                        continue;
                    }
                    else if (lines[i].StartsWith("Var ") || singleLineExecutableCommand(lines[i]))
                    {
                        //code block finishes at end of line
                        string command = lines[i].Trim();
                        cmdParser.programBlocks.Add(command);

                        //we also initialize the variables
                        if (command.StartsWith("Var "))
                        {
                            command = command.Replace("Var ", "Var$");
                            string[] commands = command.Split('$');//$ is set as separator to distinguish it from space as a separator
                            if (commands[0].Contains("Var") && (commands.Length == 2))
                            {
                                string[] varVal = commands[1].Split('=');
                                string varName = varVal[0].Trim();
                                //variable name should always start with an alphabet
                                if (varName.Length < 10)
                                {
                                    txtOutput.Text = txtOutput.Text + "\n" + "Variable name should be >= 10 characters in length." + "\n";
                                }
                                else if (checkContainsOnlyAlphabets(varName) && varName.Length >= 10)
                                {
                                    //check that varName is not one of the reserved keywords
                                    if (!checkforReservedKeywords(varName))//Not a reserved keyword//This is now useless as varName.Length >=10
                                    {
                                        string varValue = varVal[1].Trim();
                                        try
                                        {
                                            cmdParser.variables_values.Add(varName, varValue);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else //the varName is a reserved keyword
                                    {
                                        txtOutput.Text = txtOutput.Text + "\n" + "Reserved Keyword '" + varName + "'Used as a Variable Name" + "\n";
                                    }
                                }

                            }
                        }
                        continue;
                    }
                    else
                    {
                        //check if this statement is a Method call
                        foreach (KeyValuePair<string, string> method in method_block.ToArray())
                        {
                            if (lines[i].Trim().Equals(method.Key + "()"))
                            {
                                foreach (string statement in method.Value.Split('\n'))
                                {
                                    cmdParser.setCommandParser(statement.Trim());
                                    cmdParser.executeCommand();
                                    txtOutput.Text = txtOutput.Text + "\n" + lines[i].Trim();
                                }
                            }
                        }
                    }
                }//End of for loop
                //Here the whole program has been read into programblocks and all the variables in the variable_values, so we can now start executing them

                foreach (string obj in cmdParser.programBlocks.ToArray())
                {
                    string command = obj.Trim();
                    if (singleLineExecutableCommand(command))
                    {
                        //we check if this single line command contains argument, if so the argName will be replaced by its value
                        command = replaceVarNameWithItsValueInCommand(command);
                        cmdParser.setCommandParser(command);
                        cmdParser.executeCommand();
                    }
                    else//this is either programBlock (or a variable)
                    {
                        if (command.StartsWith("Var "))
                        {//this is a variable and we have already added variables to variable_values
                        }
                        else
                        {
                            if (command.StartsWith("If "))
                            {
                                executeIfBlock(command);
                            }
                            else if (command.StartsWith("While "))
                            {
                                whileCondition = command.Split('\r')[0].Trim().Replace("While ", "");

                                executeWhileBlock(command);
                            }
                            else if (command.StartsWith("Method "))
                            {

                            }
                            else
                            {

                            }

                        }
                    }

                }

            }
            else
            {//this program is a script and can be executed line by line. It only contains the following
                //moveto
                //drawto
                //clear
                //reset
                //rectangle
                //circle
                //triangle
                //pen
                //fill
                foreach (string command in txtProgram.Text.Split('\n'))
                {
                    cmdParser.setCommandParser(command.Trim());
                    cmdParser.executeCommand();
                    txtOutput.Text = txtOutput.Text + "\n" + command.Trim();
                }
            }
        }

        private object evaluateExpression(string eqn)
        {
            DataTable dt = new DataTable();
            var result = dt.Compute(eqn, string.Empty);
            return result;
        }

        public void executeIfBlock(string block)
        {
            //if has a condition after the keyword
            string[] lines = block.Split('\r');

            //we extract and test the condition
            string condition = lines[0].Replace("If ", "").Trim();
            condition = replaceVarNameWithItsValueInCommand(condition).Trim();

            //we take into consideration the following conditions

            if (condition.Contains("=="))//==
            {
                string[] left_right = condition.Replace("==", "=").Split('=');
                var left = evaluateExpression(left_right[0].Trim());
                var right = evaluateExpression(left_right[1].Trim());
                if (left.Equals(right))
                {
                    for (int i = 1; i < lines.Length; i++)
                    {

                        string command = replaceVarNameWithItsValueInCommand(lines[i].Trim());
                        if (command.Equals("EndIf"))
                        {
                            break;
                        }
                        else
                        {
                            cmdParser.setCommandParser(command);
                            cmdParser.executeCommand();
                            txtOutput.Text = txtOutput.Text + "\n" + command;
                        }
                    }
                }
            }
            else if (condition.Contains("!="))//!=
            {
                string[] left_right = condition.Replace("!=", "!").Split('!');
                var left = evaluateExpression(left_right[0].Trim());
                var right = evaluateExpression(left_right[1].Trim());
                if (!left.Equals(right))
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string command = replaceVarNameWithItsValueInCommand(lines[i].Trim());
                        cmdParser.setCommandParser(command);
                        cmdParser.executeCommand();
                        txtOutput.Text = txtOutput.Text + "\n" + command;
                    }
                }
            }
            else//no more conditions
            {
                txtOutput.Text = txtOutput.Text + "\n" + "only == and != are supported." + "\n";
            }
        }
        public void executeWhileBlock(string block)
        {
            //while has a condition after the keyword
            string[] lines = block.Split('\r');

            //we extract and test the condition
            string condition = lines[0].Replace("While ", "").Trim();
            condition = replaceVarNameWithItsValueInCommand(condition).Trim();
            //we take into consideration the following conditions
            if (condition.Contains(">"))//==
            {
                string[] left_right = condition.Split('>');
                int left = (int)evaluateExpression(left_right[0].Trim());
                int right = (int)evaluateExpression(left_right[1].Trim());
                while (left > right)
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string command = replaceVarNameWithItsValueInCommand(lines[i].Trim());

                        if (command.Equals("EndWhile"))
                        {
                            break;
                        }
                        else
                        {
                            cmdParser.setCommandParser(command);
                            cmdParser.executeCommand();
                            txtOutput.Text = txtOutput.Text + "\n" + command;
                        }
                    }
                }


            }
            else if (condition.Contains("<"))//==
            {

            }
            else //no more conditions
            {

            }
        }
        public void addMethodBlock(string block)
        {
            string[] lines = block.Split('\r');
            string statements = "";
            //we extract the method name
            string methodName = lines[0].Replace("Method ", "").Trim();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                if (lines[i].Trim().Equals("EndMethod"))
                { break; }
                statements = statements + lines[i] + "\r\n";
            }
            method_block.Add(methodName, statements);


        }

        private static bool checkContainsOnlyAlphabets(string str)
        {

            string a = str.Substring(0, 1);
            bool isAlphaBet = Regex.IsMatch(a.ToString(), "^[a-zA-Z]*$", RegexOptions.Singleline);
            if (isAlphaBet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string replaceVarNameWithItsValueInCommand(string command)
        {
            if (command.Contains('=') && !command.Contains("=="))
            {
                return assignVarNameWithItsValueonRHS(command);
            }
            else
            {
                foreach (KeyValuePair<string, string> obj in cmdParser.variables_values.ToArray())
                {
                    if (command.Contains(obj.Key))//contains is a bug here because reservedKeywords can be substrings of varNames (obj.Key) 
                    {
                        command = command.Replace(obj.Key, obj.Value);
                    }
                }
                return command;
            }
        }
        private string assignVarNameWithItsValueonRHS(string command)
        {
            string[] left_right = command.Split('=');
            string left = left_right[0].Trim();
            string right = left_right[1].Trim();
            int value = 0;
            foreach (KeyValuePair<string, string> obj in cmdParser.variables_values.ToArray())
            {
                if (right.Contains(obj.Key))//contains is a bug here because reservedKeywords can be substrings of varNames (obj.Key) 
                {
                    string cmd = replaceVarNameWithItsValueInCommand(right);
                    value = (int)evaluateExpression(cmd);
                    cmdParser.variables_values[obj.Key] = value.ToString();
                }
            }
            return left + "=" + value;

        }
        private bool singleLineExecutableCommand(string command)
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
        private bool checkCommandContainsVariableName(string command)
        {
            bool retVal = false;
            foreach (KeyValuePair<string, string> obj in cmdParser.variables_values.ToArray())
            {
                if (command.Contains(obj.Key))
                {
                    retVal = true;
                }
            }
            return retVal;

        }
        private void btnCheckSyntax_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = txtOutput.Text + "No of Program Blocks = " + cmdParser.programBlocks.Count + ". Program Blocks Started" + "\n";
            foreach (string obj in cmdParser.programBlocks.ToArray())
            {
                txtOutput.Text = txtOutput.Text + "---------------------" + "\n";
                txtOutput.Text = txtOutput.Text + obj + "\n";
            }
            txtOutput.Text = txtOutput.Text + "Program Blocks Finished" + "\n";
            txtOutput.Text = txtOutput.Text + "No of Variables = " + cmdParser.variables_values.Count + ". Variables Started" + "\n";
            foreach (KeyValuePair<string, string> obj in cmdParser.variables_values.ToArray())
            {
                txtOutput.Text = txtOutput.Text + obj.Key + ": " + obj.Value + "\n";
            }
            txtOutput.Text = txtOutput.Text + "Variables Finished" + "\n";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SingletonRectangle rectangle = SingletonRectangle.GetInstance;//No instance for this class has to be created.
            //A single instance will automatically be created and maintained throuhout the lifecycle of the the application.

            Rectangle rect;//This is a graphics object. Its parameters will be set from the Rectangle Factory Object
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(rectangle.penColor);
            if (rectangle.fill)
            {
                rect.Fill = new SolidColorBrush(rectangle.penColor);
            }
            rect.Width = rectangle.width;
            rect.Height = rectangle.height;
            Canvas.SetLeft(rect, rectangle.penLocationX);
            Canvas.SetTop(rect, rectangle.penLocationY);
            this.myCanvas.Children.Add(rect);

        }
    }

}

