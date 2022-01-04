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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommandParser cmdParser = new CommandParser();
        List<string> programBlocks = new List<string>();
        public Dictionary<string, string> variables_values = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            txtCommand.Focus();
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

        /// <summary>
        /// This is single line command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCommand_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (txtCommand.Text.Trim() == "run") {
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
        /// This button run the multiline commad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (txtProgram.Text.Contains("While ") || //this program contains code which cannot be simply executed line by line
                txtProgram.Text.Contains("EndWhile\n") || 
                txtProgram.Text.Contains("If ") ||
                txtProgram.Text.Contains("EndIf\n") ||
                txtProgram.Text.Contains("Method ") ||
                txtProgram.Text.Contains("EndMethod\n") || 
                txtProgram.Text.Contains("Var ")
            )
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
                
                for(int i = 0; i< lines.Length;i++)
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
                                programBlocks.Add(codeblock);
                                break;
                            }
                        }
                        continue;
                    }
                    else if (lines[i].StartsWith("If "))//if condition
                    {
                        //code block finishes at EndWhile
                        for (int j = i; j < lines.Length; j++, i++)
                        {
                            codeblock = codeblock + lines[i];
                            if (lines[i].Trim().Equals("EndIf"))
                            {
                                programBlocks.Add(codeblock);
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
                                programBlocks.Add(codeblock);
                                break;
                            }
                        }
                        continue;
                    }
                    else if (lines[i].StartsWith("Var ") ||
                                lines[i].StartsWith("moveto ") ||
                                lines[i].StartsWith("drawto ") ||
                                lines[i].StartsWith("clear") ||
                                lines[i].StartsWith("reset") ||
                                lines[i].StartsWith("rectangle ") ||
                                lines[i].StartsWith("circle ") ||
                                lines[i].StartsWith("triangle ") ||
                                lines[i].StartsWith("pen ") ||
                                lines[i].StartsWith("fill ")                         
                        )
                    {
                        //code block finishes at end of line
                        string command = lines[i].Trim();
                        programBlocks.Add(command);

                        //we also initialize the variables
                        if (command.StartsWith("Var "))
                        {
                            command = command.Replace("Var ", "Var$");
                            string[] commands = command.Split('$');//$ is set as separator to distinguish it from space as a separator
                            if (commands[0].Contains("Var") && (commands.Length == 2))
                            {
                                string[] varVal = commands[1].Split('=');
                                string varName = varVal[0].Trim();
                                string varValue = varVal[1].Trim();
                                try
                                {
                                    variables_values.Add(varName, varValue);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            
                        }
                        continue;
                    }
                }
                //we initialize all the variable values in a dictionary which are not in any of the above code blocks
                //foreach (string command in txtProgram.Text.Split('\n'))//split with newline ensures that conditions like "abcVar" does not occur
                                                                        //it always will be like abc \n Var
                //{
                //    if (command.Trim().Contains("Var "))
                //    {
                //        cmdParser.setCommandParser(command.Trim());
                //    }
                //}

            }
            else{//this program is a script and can be executed line by line. It only contains the following
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

        private void btnCheckSyntax_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = txtOutput.Text + "Program Blocks" + "\n";
            foreach(string obj in programBlocks.ToArray())
            {
                txtOutput.Text = txtOutput.Text + "---------------------" + "\n";
                txtOutput.Text = txtOutput.Text + "\n" + obj;
            }
            txtOutput.Text = txtOutput.Text + "Variables" + "\n";
            foreach (KeyValuePair <string,string> obj in variables_values.ToArray())
            {
                txtOutput.Text = txtOutput.Text + "\n" + obj.Key + ": " + obj.Value;
            }
            
        }
    }
   
}

