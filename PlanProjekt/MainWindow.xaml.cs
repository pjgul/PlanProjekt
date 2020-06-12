using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SQLite;

namespace PlanProjekt
{
    /*****************************************************************************************************************
    * Plan project is a program which grades users work on day to day basis. 
    * After each day the software puts users grade in a SQLite database.
    * At the end of the month the total amount of grades is added up and devided by the amount of days in the month.
    * Afterwards the final grade for the month is displayed.
    *****************************************************************************************************************/
    public partial class MainWindow : Window
    {
        /**grades array holds all possible grades that the user may recieve when graditing their day's work*/
        double[] grades = new double[] { 0, 2, 3, 3.5, 4, 4.5, 5 };
        /**counter variable is effected by the raiseCounter button. It's used in ADDButton_Click function as the task selector*/
        int counter = 1;
        
        /**grade represents the current grade. When at 0, the grade is 0, when at 1, the grade is 2, at 2 it's 3, at 3 it's 3.5, 4 is 4, 5 is 4.5 and 6 is 5.*/
        /**Additionally grade is raised by checking tasks, and lowered by unchecking them.*/
        int grade = 0;
        /**the tryTask boolean makes sure that any task was completed. when it's completed it will allow the task variable to raise the grade*/
        bool tryTask = false;
        /**the task variable works with grades[] array to set minimal amount of tasks required*/
        int task = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        /**The day variable reads the current day. The variable is used to determine whether or not its the end of the month.*/
        int day = DateTime.Now.Day;
        /**The month variable is used to determine which month it is, to help with determining the amount of days a grade needs to be devided by.*/
        int month = DateTime.Now.Month;

        /*********************************************************************************************
         * @fn GradeMonth()
         * The grade month function takes an SQLite querry that adds up all the grades from the month
        *********************************************************************************************/
        double GradeMonth() 
        {
            using (var connection = new SQLiteConnection(App.databasePath))
            {
                var gradeAVG = connection.Table<GradesDatabase>()
                    .Select(s => s.Grades)
                    .ToList()
                    .Average();
                return gradeAVG;
            }
        }

        /***************************************************
         * @fn Deleter()
         * Deleter function deletes contents of the database
        ***************************************************/
       public void Deleter()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.DeleteAll<GradesDatabase>();
            }
        }

         /****************************************************************************************************************************
          * @fn FinalFunction()
          * The final function changes the grade screen to say that it grades the month, as well as to put out the grade for the month. 
          * Finally the function deletetes the contents of database
         ****************************************************************************************************************************/
        public void FinalFunction()
        {
            GradeWindow gradeWindow = new GradeWindow();
            gradeWindow.gradeMessage.Content = "Your grade for this month is";
            gradeWindow.gradeTest.Content = "" + GradeMonth();
            gradeWindow.Show();
            Deleter();
        }

        /***********************************************************************************************************************
         * @fn ADDButton_Click(object sender, RoutedEventArgs e)
         * ADDButton function inserts text from InputBox TextBox into tasks depening on the current value of the counter variable
        ***********************************************************************************************************************/
        public void ADDButton_Click(object sender, RoutedEventArgs e)
        {
                switch (counter)
                {
                    case 1:
                        {
                            task1.Content = inputBox.Text;
                            break;
                        }
                    case 2:
                        {
                            task2.Content = inputBox.Text;
                            break;
                        }
                    case 3:
                        {
                            task3.Content = inputBox.Text;
                            break;
                        }
                    case 4:
                        {
                            task4.Content = inputBox.Text;
                            break;
                        }
                    case 5:
                        {
                            task5.Content = inputBox.Text;
                            break;
                        }
                    case 6:
                        {
                            task6.Content = inputBox.Text;
                            break;
                        }
                }

        }

        /**********************************************************************************************************************************
         * @fn raiseCounter_Click(object sender, RoutedEventArgs e)
         * raise conter raises the counter until the last task, then it resets the counter and inputs current date into the inputBox TextBox
         * as well as sets the currentTask TextBox as "/" where it represents that no task is selected
        **********************************************************************************************************************************/
        public void raiseCounter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = DateTime.Today;
                if (counter < 6)
                {
                    counter++;
                    currentTask.Text = "" + counter;
                }
                else
                {
                    inputBox.Text = dateTime.ToString("dd/MM/yyyy");
                    counter = 0;
                    currentTask.Text = "/";
                }
            }
            catch (Exception e1)
            {
                inputBox.Text = e1.Message;
            }
        }

        /************************************************************************************************************************************
         * @fn lowerCounter_Click(object sender, RoutedEventArgs e)
         * lower conter lowers the counter until the first task, then it resets the counter and inputs current date into the inputBox TextBox
         * as well as sets the currentTask TextBox as "/" where it represents that no task is selected
        ************************************************************************************************************************************/
        public void lowerCounter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = DateTime.Today;
                if (counter > 1)
                {
                    counter--;
                    currentTask.Text = "" + counter;
                }
                else
                {
                    inputBox.Text = dateTime.ToString("dd/MM/yyyy");
                    counter = 7;
                    currentTask.Text = "/";
                }
            }
            catch (Exception e1)
            {
                inputBox.Text = e1.Message;
            }
        }

        /*********************************************************
         * @fn Checked(object sender, RoutedEventArgs e)
         * The function raises the grade when the task is checked.
         * Additionally it allows the grade button to work.
        *********************************************************/
        public void Checked(object sender, RoutedEventArgs e)
        {
            grade++;
                tryTask = true;
        }

        /*********************************************************
         * @fn Unchecked(object sender, RoutedEventArgs e)
         * The function lowers the grade when the task is unchecked.
         * Additionally it allows the grade button to work.
        *********************************************************/
        public void Unchecked(object sender, RoutedEventArgs e)
        {
            grade--;
                tryTask = false;
        }



        /*************************************************************************************************************************************************
         * @fn gradeButton_Click(object sender, RoutedEventArgs e)
         * This function finishes the whole work of the program by displaying the final grade.
         * Firstly, it checks whether or not the added index of task and grade variable goes out of scope of the grades array.
         * After the check is completed and the orrect grade is assigned, the function creates and new window to display the grade.
         * Afterwards it tests which month is it, and then checks whether or not its the final ay of said month.
         * Finally if it's the case the FinalFunction activates calculating the grade for the month, after which said grade is displayed in a new window.
        *************************************************************************************************************************************************/
        public void gradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (tryTask)
            {
                if ((grade + task) > 6)
                {
                    inputBox.Text = "" + grades[6];
                }
                else
                {
                    inputBox.Text = "" + grades[grade + task];
                }
                    GradeWindow gradeWindow = new GradeWindow();
                    /**the variable below corresponds to the grade window, and is supplied with the grade from the main window*/
                    gradeWindow.gradeTest.Content = inputBox.Text;
                    gradeWindow.Show();
                    GradesDatabase GradesDB = new GradesDatabase()
                    {
                        /**The database is updated with the current grade*/
                        Grades = grades[grade]
                    };

                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<GradesDatabase>();
                    connection.Insert(GradesDB);


                    /**********************************************************************************************************************************
                     * the month switch tests which month it currently is, then checks whether it's the end of the month for said month,
                     * until finally it exceutes a query that sums up the grades from all the dates of the month and devides them by the number of days,
                     * ending with the final grade for the entire month
                    **********************************************************************************************************************************/
                    switch (month)
                    {
                        case 1:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 2:
                            {
                                if (day == 29)
                                {
                                    FinalFunction();
                                }
                                else if (day == 28)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 3:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 4:
                            {
                                if (day == 30)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 5:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 6:
                            {
                                if (day == 30)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 7:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 8:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 9:
                            {
                                if (day == 30)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 10:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 11:
                            {
                                if (day == 30)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                        case 12:
                            {
                                if (day == 31)
                                {
                                    FinalFunction();
                                }
                                break;
                            }
                    }

                    Close();

                }
                
            }
        }

        /*************************************************************************************
         * @fn bRaise_Click(object sender, RoutedEventArgs e)
         * The bRaise function raises the amount of tasks needed to be completed for the day.
         * Said function will only raise up to a maximum of 6.
        *************************************************************************************/
        public void bRaise_Click(object sender, RoutedEventArgs e)
        {
            if (task > 0)
            {
                task--;
                yourTasks.Text = "Number of tasks to do: " + (6 - task);
            }

        }

        /**************************************************************************************
         * @fn bLower_Click(object sender, RoutedEventArgs e)
         * The bLower function lowers the amount of tasks needed to be completed for the day.
         * Said function will only lower down to a minimum of 1 task.
        **************************************************************************************/
        public void bLower_Click(object sender, RoutedEventArgs e)
        {
            if (task < 5) 
            {
                yourTasks.Text = "Number of tasks to do: " + (5 - task);
                task++;
            }
        }
    }
}
