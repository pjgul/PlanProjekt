using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlanProjekt
{
    /************************************************************************************
     * Besides its regular use the APP class also manages to connection to the database.
    ************************************************************************************/
    public partial class App : Application
    {
        static string gradesDB = "GradesDatabase.db";
        static string folderPath = Directory.GetCurrentDirectory();
        public static string databasePath = Path.Combine(folderPath, gradesDB);
    }
}
