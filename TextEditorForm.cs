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
    public partial class TextEditorForm : Form
    {
        LoginForm loginForm;// login form to show when logging out
        string currentFileName;// name of the file being currently editied
        /*
         * constructor sets window title, usertype, username, enabled buttons based on usertype
         */
        public TextEditorForm(string userKey, string userType, LoginForm loginForm)
        {
            InitializeComponent();
            this.loginForm = loginForm;
            this.usernameLabel.Text = String.Format("Username: {0}", userKey);
            this.userTypeLabel.Text = String.Format("User Role: {0}", userType);

            this.Text = "New file";
            if (userType.Equals("View"))//disables uneeded buttons
            {
                this.textBox.ReadOnly = true;
                this.pasteToolStripMenuItem.Enabled = false;
                this.pasteButton.Enabled = false;
                this.newButton.Enabled = false;
                this.newToolStripMenuItem.Enabled = false;
                this.boldButton.Enabled = false;
                this.boldToolStripMenuItem.Enabled = false;
                this.italicsButton.Enabled = false;
                this.italicToolStripMenuItem.Enabled = false;
                this.underlineButton.Enabled = false;
                this.underlineToolStripMenuItem.Enabled = false;
                this.fontSizePicker.Enabled = false;
                this.cutButton.Enabled = false;
                this.cutToolStripMenuItem.Enabled = false;
            }
        }
        /*
         * check to save changes when form is closing
         */
        private void TextEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible == true)
            {
                e.Cancel = CheckSavedChanges() == DialogResult.Cancel;
            }
        }
        /*
         * if form is closed by escape button (X) exit the application
         */
        private void TextEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.loginForm.Visible)
            {
                Application.Exit();
            }
        }

        /*
         * Check for saved changes if logging out and then show login screen again
         */
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            if (CheckSavedChanges() != DialogResult.Cancel)
            {
                this.loginForm.Show();
                this.Hide();
            }
        }
        /*
         *saves all text to the current file
         */
        private void SaveFile()
        {
            File.WriteAllText(currentFileName, this.textBox.Rtf);
        }

        /*
         * When save as button clicked call save as function
         */
        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveAs();

        }

        /*
         * when save button clicked call either function if file exists already or save as if it is the first save
         */
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFileName))
            {
                SaveFile();
            }
            else
            {
                SaveAs();
            }
        }

        /*
         * creates a new save file fialog and saves prompts user to save file
         */
        private void SaveAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = "Save As",
                Filter = "Rich Text Format file (*.rtf)|*.rtf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                currentFileName = saveDialog.FileName;
                Text = currentFileName;
                SaveFile();
            }
        }

        /*
         * prompts for saving file then
         * opens new open file dialog and prompts user to open a file
         */
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (CheckSavedChanges() != DialogResult.Cancel)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Open File",
                    Filter = "Rich Text Format file (*.rtf)|*.rtf"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.currentFileName = openFileDialog.FileName;
                    this.Text = currentFileName;

                    textBox.LoadFile(currentFileName, RichTextBoxStreamType.RichText);
                }
            }
        }

        /*
         * prompts to save changes then creates a new blank file to work on
         */
        private void NewButton_Click(object sender, EventArgs e)
        {
            if (CheckSavedChanges() != DialogResult.Cancel)
            {
                currentFileName = string.Empty;
                Text = "New file";
                textBox.Clear();

            }
        }

        /*
         * shows the about form
         */
        private void AboutButton_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        /*
         * used to check when exiting the file screen
         * checks to see if the file has been modified since it's last save and prompts user to save or
         * prompts user to save if file has not been saved before 
         */
        private DialogResult CheckSavedChanges()
        {
            DialogResult changes = DialogResult.None;
            try
            {
                if (File.ReadAllText(currentFileName) != this.textBox.Rtf && !string.IsNullOrEmpty(currentFileName))
                {
                    changes = MessageBox.Show($"Do you want to save changes to\n{currentFileName}?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (changes == DialogResult.Yes)
                    {
                        SaveFile();
                    }
                }
            }
            catch (Exception)//catches multiple exceptions due to currentFileName being null and not having a file of the same already saved and prompts for saving
            {
                if (!string.IsNullOrEmpty(this.textBox.Text))
                {
                    changes = MessageBox.Show($"Do you want to save this file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (changes == DialogResult.Yes)
                    {
                        SaveAs();
                    }
                }
            }

            return changes;
        }

        /*
         * When the cut command is pressed or Ctrl+X
         */
        private void CutButton_Click(object sender, EventArgs e)
        {
            if (textBox.SelectionLength > 0)
            {
                textBox.Cut();
            }
        }

        /*
         * When the Copy command is pressed or Ctrl+C
         */
        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (textBox.SelectionLength > 0)
            {
                textBox.Copy();
            }
        }

        /*
         * When the Paste command is pressed or Ctrl+V
         */
        private void PasteButton_Click(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        /*
         * When the Bold command is pressed or Ctrl+B
         * toggles text for bold or not as well as menu icon highlighting
         */
        private void BoldButton_Click(object sender, EventArgs e)
        {
            try
            {
                Font currentFont = textBox.SelectionFont;

                if (currentFont.Bold)
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Bold);
                    boldButton.Checked = false;
                }
                else
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Bold);
                    boldButton.Checked = true;
                }
            }
            catch (NullReferenceException)
            {
                boldButton.Checked = !boldButton.Checked;
            }

        }

        /*
         * When the Italic command is pressed or Ctrl+I
         * toggles text for Italic or not as well as menu icon highlighting
         */
        private void ItalicButton_Click(object sender, EventArgs e)
        {
            try
            {
                Font currentFont = textBox.SelectionFont;

                if (currentFont.Italic)
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Italic);
                    italicsButton.Checked = false;
                }
                else
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Italic);
                    italicsButton.Checked = true;
                }
            }
            catch (NullReferenceException)
            {
                italicsButton.Checked = !italicsButton.Checked;
            }
        }
        
        /*
         * When the Underline command is pressed or Ctrl+U
         * toggles text for underline or not as well as menu icon highlighting
         */
        private void UnderlineButton_Click(object sender, EventArgs e)
        {
            try
            {
                Font currentFont = textBox.SelectionFont;

                if (currentFont.Underline)
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Underline);
                    underlineButton.Checked = false;
                }
                else
                {
                    textBox.SelectionFont = new Font(textBox.SelectionFont, textBox.SelectionFont.Style ^ FontStyle.Underline);
                    underlineButton.Checked = true;
                }
            }
            catch (NullReferenceException)
            {
                underlineButton.Checked = !underlineButton.Checked;
            }
        }

        /*
         * when typing at a new spot updates menu icons to match font styling and size
         */
        private void UpdateFontStyleToolStripButtons()
        {
            Font currentFont = textBox.SelectionFont;
            if (currentFont != null)
            {
                boldButton.Checked = currentFont.Bold;
                italicsButton.Checked = currentFont.Italic;
                underlineButton.Checked = currentFont.Underline;
                fontSizePicker.Text = Convert.ToString(currentFont.Size);
            }
        }

        /*
         * when text is input call the menu icon updater
         */
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateFontStyleToolStripButtons();
        }

        /*
         * Changes font size of selected font to number selected
         */
        private void FontSizePicker_IndexChange(object sender, EventArgs e)
        {
            float fontSize = Convert.ToSingle(this.fontSizePicker.SelectedItem);
            if (fontSize > 0)
            {
                Font currentFont = textBox.SelectionFont;
                if (currentFont != null)
                {
                    textBox.SelectionFont = new Font(currentFont.FontFamily, fontSize, currentFont.Style);
                }
            }

        }


    }
}
