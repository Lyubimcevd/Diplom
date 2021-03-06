﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Editor.Classes
{
    public class Commands
    {
        public static RoutedCommand Forward { get; set; }
        public static RoutedCommand Rename { get; set; }
        static Commands()
        {
            Forward = new RoutedCommand("Forward", typeof(MainWindow));
            Rename = new RoutedCommand("Rename", typeof(MainWindow));
        }
    }
}
