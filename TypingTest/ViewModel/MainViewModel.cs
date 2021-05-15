using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TypingTest.ViewModel.BaseClass;

namespace TypingTest.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        DateTime? TimeOfLastKeyPressed = null;
        public MainViewModel()
        {
            _displayedItems.Add(new String("dupa"));
            TypedText = "jsdiof";
        }

        private ObservableCollection<object> _displayedItems = new ObservableCollection<object>() { new String("dfdffd") };
        public ObservableCollection<object> DisplayedItems
        {
            get { return _displayedItems; }
            set { _displayedItems = value; OnPropertyChanged(nameof(DisplayedItems)); }
        }

        private String typedText;
        public String TypedText { 
            get { return typedText; } 
            set { typedText = value;
                DateTime now = DateTime.Now;

                TimeSpan? durationOfType;
                if (TimeOfLastKeyPressed == null)
                    durationOfType = null;
                else
                    durationOfType = now - TimeOfLastKeyPressed;
                TimeOfLastKeyPressed = now;
                if(durationOfType!=null)
                    _displayedItems.Add($"'{TypedText[TypedText.Length - 1]}': {((TimeSpan)durationOfType).TotalSeconds}s");
            } 
        }

        public bool CanExecuteUpdateTextBoxBindingOnEnterCommand(object parameter)
        {
            return true;
        }

        public void ExecuteUpdateTextBoxBindingOnEnterCommand(object parameter)
        {
            TextBox tBox = parameter as TextBox;
            if (tBox != null)
            {
                DependencyProperty prop = TextBox.TextProperty;
                BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null)
                    binding.UpdateSource();
            }
        }
        public void dupa(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tBox = (TextBox)sender;
                DependencyProperty prop = TextBox.TextProperty;

                BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null) { binding.UpdateSource(); }
            }
        }

        public void add(object sender, KeyEventArgs e)
        {
            DisplayedItems.Add("dupa");
        }
    }
}
