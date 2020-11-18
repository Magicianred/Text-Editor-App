using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor_Assign2
{
    public partial class LoginForm : Form
    {
        /*
         * users is a dictionary so that all users have a unique username
         * TKey = username
         * TValue = array of [password, userType, firstName, lastName, DOB]
         */
        public Dictionary<string, string[]> users;


        public LoginForm()
        {
            InitializeComponent();
            
        }


        /*
         *when the login button is clicked checks if username and passowrd are correct and creates new text editor if they are,
         *if not emptiess password and creates a message box to show error 
         */
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (users.ContainsKey(usernameText.Text) && users[usernameText.Text][0].CompareTo(passwordText.Text)==0)
            {
                this.Hide();
                new TextEditorForm(usernameText.Text, users[usernameText.Text][1], this).Show();
                this.usernameText.Text = string.Empty;
                this.passwordText.Text = string.Empty;
                
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.passwordText.Text = string.Empty;
            }
        }

        /*
         * if new user button is clicked create a new user form and show it whilst hiding this form
         */
        private void NewUserButton_Click(object sender, EventArgs e)
        {
            this.usernameText.Text = string.Empty;
            this.passwordText.Text = string.Empty;
            this.Hide();
            new NewUserForm(this).Show();
        }

        /*
         * exit program if exit button clicked
         */
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*
         *on initial loginform load initialize all the user logins from text file into dictionary 
         */
        private void LoginForm_Load(object sender, EventArgs e)
        {
            
            LoadUsers();
        }

        /*
         * get users from text file and put their data into the users dictionary
         */
        private void LoadUsers()
        {
            users = new Dictionary<string, string[]>();
            try
            {
                //read all lines into an array
                String[] rawFileLines = File.ReadAllLines("login.txt");
                int lineCount = rawFileLines.Length;

                for (int i = 0; i < lineCount; i++)
                {
                    String[] line = rawFileLines[i].Split(',').Select(a => a.Trim()).ToArray();
                    users.Add(line[0], line.Skip(1).ToArray());
                }

            }
            //if the file is not found display an error message
            catch (FileNotFoundException)
            {
                MessageBox.Show("Cannot find the login file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * when this form is made visible put user curser on username textbox 
         */
        private void LoginForm_Show(object sender, EventArgs e)
        {
            this.ActiveControl = usernameText;
        }
    }
}
