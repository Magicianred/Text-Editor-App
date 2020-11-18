using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor_Assign2
{
    public partial class NewUserForm : Form
    {
        // login form to show once a new user is created or process is exited
        private LoginForm loginForm;
        public NewUserForm(LoginForm loginForm)
        {
            InitializeComponent();
            this.loginForm = loginForm;
        }

        /*
         * checks all the the form data when the create button is pressed and creates a user if all is correct,
         * else shows message box with apparent data error 
         */
        private void CreateUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (loginForm.users.ContainsKey(usernameText.Text))
                {
                    throw new UsernameInUseException();
                }
                if (usernameText.Text.CompareTo("") == 0 || passwordText.Text.CompareTo("") == 0 || passwordReentryText.Text.CompareTo("") == 0 ||
                    firstNameText.Text.CompareTo("") == 0 || lastNameText.Text.CompareTo("") == 0 || userTypeCombo.GetItemText(this.userTypeCombo.SelectedItem).CompareTo("") == 0 ||
                    birthDatePicker.Value.ToShortDateString().CompareTo("") == 0)
                {
                    throw new BlankFieldException();
                }
                if (passwordText.Text.CompareTo(passwordReentryText.Text) != 0)
                {
                    throw new PasswordMatchException();
                }
                // save all but username data to an array for the kValue of the dictionary
                string[] newUserData = new[] { passwordText.Text, userTypeCombo.GetItemText(this.userTypeCombo.SelectedItem), firstNameText.Text, lastNameText.Text, birthDatePicker.Value.ToString("dd-MM-yyyy") };
                // add the kKey and kValue to the users dictionary
                this.loginForm.users.Add(usernameText.Text, newUserData);
                //write the new user to the text file
                using (StreamWriter stream = File.AppendText("login.txt"))
                {
                    stream.Write(Environment.NewLine + usernameText.Text + ",");
                    foreach (string a in newUserData)
                    {
                        stream.Write(a + ",");
                    }
                }

                this.loginForm.Show();
                this.Close();
            }
            catch (UsernameInUseException)//if the username is already +
            {
                this.usernameText.Text = string.Empty;
                MessageBox.Show("Sorry, that username is already in user please choose another.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (BlankFieldException)//if a data entry field is left blank
            {
                MessageBox.Show("Please ensure all fields are entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (PasswordMatchException)//if a the password fields do not match
            {
                this.passwordText.Text = string.Empty;
                this.passwordReentryText.Text = string.Empty;
                MessageBox.Show("The entered passwords do not match. Please re-enter them.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         *  if the cancel button pressed, show the login form again 
         */
        private void CancelButton_Click(object sender, EventArgs e)
        {
            loginForm.Show();
            this.Close();
        }
        /*
         * if exit button pressed (X) close the entire application
         */
        private void NewUserForm_Closed(object sender, FormClosedEventArgs e)
        {
            if (!loginForm.Visible)
            {
                Application.Exit();
            }
        }
    }

    [Serializable]
    internal class PasswordMatchException : Exception
    {
        public PasswordMatchException() { }
    }

    [Serializable]
    internal class BlankFieldException : Exception
    {
        public BlankFieldException() { }
    }

    [Serializable]
    internal class UsernameInUseException : Exception
    {
        public UsernameInUseException() { }

    }
}
