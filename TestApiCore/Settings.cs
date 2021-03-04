using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TestApiCore
{
    public class Settings
    {
        public static string FilePath =>
            (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.ToLower().Replace("testapicore.dll", "");

        static Settings()
        {
        }
    }
}
