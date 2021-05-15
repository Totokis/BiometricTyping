using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TypingTest
{
    public class RestrictKeysTextBox : TextBox
    {
        public RestrictKeysTextBox() : base()
        {
            this.AddHandler(RestrictKeysTextBox.KeyDownEvent, new RoutedEventHandler(HandleHandledKeyDown), true);
        }

        public void HandleHandledKeyDown(object sender, RoutedEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Key.Space)
            {
                ke.Handled = false;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Space)
            {
                //Prevent spaces (space does not "raise" OnPreviewTextInput)
                e.Handled = true;
            }
        }
    }
}