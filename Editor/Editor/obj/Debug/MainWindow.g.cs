﻿#pragma checksum "..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F2C2FD7C18613AF2DDB7F92E9905713FBDAD59D9"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Editor;
using Editor.Classes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Editor {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 119 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView tree;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Editor;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\MainWindow.xaml"
            ((Editor.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_New);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 17 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Open);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 19 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Save);
            
            #line default
            #line hidden
            
            #line 20 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Save_CanExecute);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 22 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_SaveAs);
            
            #line default
            #line hidden
            
            #line 23 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.SaveAs_CanExecute);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 25 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Cut);
            
            #line default
            #line hidden
            
            #line 26 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Cut_CanExecute);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 28 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Copy);
            
            #line default
            #line hidden
            
            #line 29 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Copy_CanExecute);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 31 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Paste);
            
            #line default
            #line hidden
            
            #line 32 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Paste_CanExecute);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 34 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Rename);
            
            #line default
            #line hidden
            
            #line 35 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Rename_CanExecute);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 37 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Undo);
            
            #line default
            #line hidden
            
            #line 38 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Undo_CanExecute);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Help);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 42 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Close);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 44 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Forward);
            
            #line default
            #line hidden
            
            #line 45 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Forward_CanExecute);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 47 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.CommandBinding_Delete);
            
            #line default
            #line hidden
            
            #line 48 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.Delete_CanExecute);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 58 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_New_Element);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 88 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Import_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 90 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 116 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.About_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.tree = ((System.Windows.Controls.TreeView)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 20:
            
            #line 125 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Rename_DoubleClick);
            
            #line default
            #line hidden
            break;
            case 21:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.TreeViewItem.ExpandedEvent;
            
            #line 130 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.TreeViewItemExpanded);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.TreeViewItem.CollapsedEvent;
            
            #line 131 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.TreeViewItemCollapsed);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.PreviewMouseRightButtonDownEvent;
            
            #line 132 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.TreeViewItemPreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.PreviewMouseLeftButtonDownEvent;
            
            #line 133 "..\..\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.TreeViewItemPreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

