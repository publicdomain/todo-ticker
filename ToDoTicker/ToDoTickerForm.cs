// <copyright file="ToDoTickerForm.cs" company="PublicDomain">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
namespace PublicDomain
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;

    /// <summary>
    /// Todo list ticker form.
    /// </summary>
    public partial class ToDoTickerForm : Form
    {
        /// <summary>
        /// The name of the module.
        /// </summary>
        private string moduleName = "To-do List ticker";

        /// <summary>
        /// The semantic version.
        /// </summary>
        private string semanticVersion = "0.1.0";

        /// <summary>
        /// To do list ticker data.
        /// </summary>
        private ToDoTickerData toDoTickerData;

        /// <summary>
        /// Gets or sets the associated icon.
        /// </summary>
        /// <value>The associated icon.</value>
        private Icon associatedIcon = null;

        /// <summary>
        /// The data file path.
        /// </summary>
        private string dataFilePath = Path.Combine(Application.StartupPath, "ToDoTickerData.bin");

        /// <summary>
        /// The font converter.
        /// </summary>
        private FontConverter fontConverter = new FontConverter();

        /// <summary>
        /// The ticker form.
        /// </summary>
        private TickerForm tickerForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PublicDomain.ToDoTickerForm"/> class.
        /// </summary>
        public ToDoTickerForm()
        {
            // Required for Windows Forms designer support.
            this.InitializeComponent();

            // Set associated icon from exe file
            this.associatedIcon = Icon.ExtractAssociatedIcon(typeof(ToDoTickerForm).GetTypeInfo().Assembly.Location);

            /* Process data file */

            // Check for a previously-saved data file
            if (File.Exists(this.dataFilePath))
            {
                // Read previous data from binary file
                this.toDoTickerData = this.ReadTickerDataFromFile(this.dataFilePath);

                // Bring data to life in GUI
                this.LoadToDoTickerData();
            }
            else
            {
                // New ticker data instance with initial values set
                this.toDoTickerData = new ToDoTickerData
                {
                    TimerInterval = 10,

                    Separator = "  |  ",

                    TextFont = this.fontConverter.ConvertToInvariantString(this.mainFontDialog.Font),

                    ForegroundColor = this.foregroundColorDialog.Color,

                    BackgroundColor = this.backgroundColorDialog.Color,

                    LeftMargin = 50,

                    RightMargin = 50,
                };
            }
        }

        /// <summary>
        /// Reads the ticker data from file.
        /// </summary>
        /// <returns>The ticker data from file.</returns>
        /// <param name="tickerDataFilePath">Ticker data file path.</param>
        private ToDoTickerData ReadTickerDataFromFile(string tickerDataFilePath)
        {
            // Variable to hold read ticker data
            ToDoTickerData tickerData;

            // Read data from binary file
            IFormatter formatter = new BinaryFormatter();
            Stream binaryFileStream = new FileStream(tickerDataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            tickerData = (ToDoTickerData)formatter.Deserialize(binaryFileStream);
            binaryFileStream.Close();

            // Return read ticker data
            return tickerData;
        }

        /// <summary>
        /// Writes the ticker data to file.
        /// </summary>
        /// <param name="tickerDataFilePath">Ticker data file path.</param>
        private void WriteTickerDataToFile(string tickerDataFilePath)
        {
            // Save ticker data to binary file
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(tickerDataFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this.toDoTickerData);
            stream.Close();
        }

        /// <summary>
        /// Handles the font tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFontToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Show the font dialog
            this.mainFontDialog.ShowDialog();
        }

        /// <summary>
        /// Handles the text speed tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnTextSpeedToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Variable to hold user input
            int parsedInt;

            // Try to parse
            if (int.TryParse(Interaction.InputBox("Set ticker timer interval (milliseconds, less is faster)", "Text speed", this.toDoTickerData.TimerInterval.ToString(), -1, -1), out parsedInt))
            {
                // Set custom value
                this.toDoTickerData.TimerInterval = parsedInt;
            }
        }

        /// <summary>
        /// Handles the always on top tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAlwaysOnTopToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Toggle checked state
            this.alwaysOnTopToolStripMenuItem.Checked = !this.alwaysOnTopToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the remember settings tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRememberSettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Toggle checked state
            this.rememberSettingsToolStripMenuItem.Checked = !this.rememberSettingsToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Check for items, then ask user about clearing them
            if (this.todoListBox.Items.Count > 0 && MessageBox.Show("Proceed to clear list items?", "New list", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Clear To-do list
                this.todoListBox.Items.Clear();
            }
        }

        /// <summary>
        /// Loads the To-do list ticker data.
        /// </summary>
        private void LoadToDoTickerData()
        {
            // Font
            this.mainFontDialog.Font = (Font)this.fontConverter.ConvertFromInvariantString(this.toDoTickerData.TextFont);

            // Always on top
            this.alwaysOnTopToolStripMenuItem.Checked = this.toDoTickerData.AlwaysOnTop;

            // Full width
            this.fullWidthToolStripMenuItem.Checked = this.toDoTickerData.FullWidth;

            // Foreground color
            this.foregroundColorDialog.Color = this.toDoTickerData.ForegroundColor;

            // Background color
            this.backgroundColorDialog.Color = this.toDoTickerData.BackgroundColor;

            // Clear To-do list
            this.todoListBox.Items.Clear();

            // Load items
            this.todoListBox.Items.AddRange(this.toDoTickerData.ListItems.Cast<object>().ToArray());
        }

        /// <summary>
        /// Sets the To-do list ticker data.
        /// </summary>
        private void SetToDoTickerData()
        {
            // Font
            this.toDoTickerData.TextFont = this.fontConverter.ConvertToInvariantString(this.mainFontDialog.Font);

            // Always on top
            this.toDoTickerData.AlwaysOnTop = this.alwaysOnTopToolStripMenuItem.Checked;

            // Full width
            this.toDoTickerData.FullWidth = this.fullWidthToolStripMenuItem.Checked;

            // Foreground color
            this.toDoTickerData.ForegroundColor = this.foregroundColorDialog.Color;

            // Background color
            this.toDoTickerData.BackgroundColor = this.backgroundColorDialog.Color;

            // Set new list (blank slate)
            this.toDoTickerData.ListItems = new List<string>(this.todoListBox.Items.Cast<string>());
        }

        /// <summary>
        /// Handles the open tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Set initial file name
            this.openFileDialog.FileName = Path.GetFileNameWithoutExtension(this.dataFilePath);

            // Show open file dialog
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                /* Populate ticker data */

                try
                {
                    // Deserialize to variable
                    this.toDoTickerData = this.ReadTickerDataFromFile(this.openFileDialog.FileName);

                    // Load data
                    this.LoadToDoTickerData();
                }
                catch (Exception)
                {
                    // Inform user
                    this.mainToolStripStatusLabel.Text = "Open file error";
                }
            }
        }

        /// <summary>
        /// Handles the save tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Set file name
            this.saveFileDialog.FileName = Path.GetFileNameWithoutExtension(this.dataFilePath);

            // Open save file dialog
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK && this.saveFileDialog.FileName.Length > 0)
            {
                /* Save ticker data */

                try
                {
                    // Set data
                    this.SetToDoTickerData();

                    // Write data to file
                    this.WriteTickerDataToFile(this.saveFileDialog.FileName);
                }
                catch (Exception)
                {
                    // Inform user
                    this.mainToolStripStatusLabel.Text = "Save file error";
                }

                // Inform user
                this.mainToolStripStatusLabel.Text = $"Saved to \"{Path.GetFileName(this.saveFileDialog.FileName)}\"";
            }
        }

        /// <summary>
        /// Handles the exit tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Exit module
            this.Close();
        }

        /// <summary>
        /// Handles the add button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAddButtonClick(object sender, EventArgs e)
        {
            // Ask user for new list item
            string listItem = Interaction.InputBox("Set new To-do list item text", "Add item", string.Empty, -1, -1);

            // Check there's something to work with
            if (listItem.Length > 0)
            {
                // Add to To-do list
                this.todoListBox.Items.Add(listItem);
            }
        }

        /// <summary>
        /// Handles the remove button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            // Check for a selected item
            if (this.todoListBox.SelectedIndex > -1)
            {
                // Remove item by index
                this.todoListBox.Items.RemoveAt(this.todoListBox.SelectedIndex);
            }
        }

        /// <summary>
        /// Handles the full width tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFullWidthToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Toggle checked state
            this.fullWidthToolStripMenuItem.Checked = !this.fullWidthToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Handles the separator tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSeparatorToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Ask user for new separator
            string separator = Interaction.InputBox("Set To-do list items separator", "Separator", this.toDoTickerData.Separator, -1, -1);

            // Check there's something to work with
            if (separator.Length > 0)
            {
                // Set new separator
                this.toDoTickerData.Separator = separator;
            }
        }

        /// <summary>
        /// Handles the foreground tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnForegroundToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Show the foreground color dialog
            this.foregroundColorDialog.ShowDialog();
        }

        /// <summary>
        /// Handles the background tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBackgroundToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Show the background color dialog
            this.backgroundColorDialog.ShowDialog();
        }

        /// <summary>
        /// Handles the left tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnLeftToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Variable to hold user input
            int parsedInt;

            // Try to parse
            if (int.TryParse(Interaction.InputBox("Set left margin (in pixels)", "Ticker form margin", this.toDoTickerData.LeftMargin.ToString(), -1, -1), out parsedInt))
            {
                // Set custom value
                this.toDoTickerData.LeftMargin = parsedInt;
            }
        }

        /// <summary>
        /// Handle the right tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRightToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Variable to hold user input
            int parsedInt;

            // Try to parse
            if (int.TryParse(Interaction.InputBox("Set right margin (in pixels)", "Ticker form margin", this.toDoTickerData.RightMargin.ToString(), -1, -1), out parsedInt))
            {
                // Set custom value
                this.toDoTickerData.RightMargin = parsedInt;
            }
        }

        /// <summary>
        /// Handles the bottom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBottomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Variable to hold user input
            int parsedInt;

            // Try to parse
            if (int.TryParse(Interaction.InputBox("Set bottom margin (in pixels)", "Ticker form margin", this.toDoTickerData.BottomMargin.ToString(), -1, -1), out parsedInt))
            {
                // Set custom value
                this.toDoTickerData.BottomMargin = parsedInt;
            }
        }

        /// <summary>
        /// Handles the todo list ticker form form closing event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnToDoTickerFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if must remember settings 
            if (this.rememberSettingsToolStripMenuItem.Checked)
            {
                // Set data
                this.SetToDoTickerData();

                // Write data to disk
                this.WriteTickerDataToFile(this.dataFilePath);
            }
            else
            {
                // Delete data file
                File.Delete(this.dataFilePath);
            }

            // Finally, check for an active ticker form
            if (this.tickerForm != null)
            {
                // Close ticker form
                this.tickerForm.Close();
            }
        }

        /// <summary>
        /// Handles the todo list box mouse down event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnTodoListBoxMouseDown(object sender, MouseEventArgs e)
        {
            // Set item index
            int itemIndex = this.todoListBox.IndexFromPoint(e.Location);

            // Check for an actual item and a right click
            if (itemIndex > -1 && e.Button == MouseButtons.Right)
            {
                // Collect new item text
                string itemText = Interaction.InputBox("Modify To-do list item text", "Edit text", this.todoListBox.Items[itemIndex].ToString(), -1, -1);

                // Check there's something to work with
                if (itemText.Length > 0)
                {
                    // Set new item text
                    this.todoListBox.Items[itemIndex] = itemText;
                }
            }
        }

        /// <summary>
        /// Handles the show ticker button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnShowTickerButtonClick(object sender, EventArgs e)
        {
            // Check for list items
            if (this.todoListBox.Items.Count == 0)
            {
                // Halt flow
                return;
            }

            // Check button text
            if (this.showTickerButton.Text.StartsWith("&C", StringComparison.InvariantCulture))
            {
                // Close ticker form
                this.tickerForm.Close();

                // Set text back
                this.showTickerButton.Text = "&Show ticker";

                // Halt flow
                return;
            }
            else
            {
                // Update text
                this.showTickerButton.Text = "&Close ticker";
            }

            // Set ticker data
            this.SetToDoTickerData();

            // Set working area width
            var workingAreaWidth = Screen.FromControl(this).WorkingArea.Width;

            // Set working area height
            var workingAreaHeight = Screen.FromControl(this).WorkingArea.Height;

            // Set ticker font
            var tickerFont = (Font)this.fontConverter.ConvertFromInvariantString(this.toDoTickerData.TextFont);

            // Declare new ticker form
            this.tickerForm = new TickerForm(string.Join(this.toDoTickerData.Separator, this.todoListBox.Items.Cast<string>()), tickerFont, this.toDoTickerData.TimerInterval, this.toDoTickerData.ForegroundColor, this.toDoTickerData.BackgroundColor)
            {
                // Set ticker height using font's height plus 10 pixels for padding
                Height = tickerFont.Height + 10,

                // Set ticker width
                Width = workingAreaWidth
            };

            // Check for full width
            if (!this.fullWidthToolStripMenuItem.Checked)
            {
                // Subtract from width
                this.tickerForm.Width -= this.toDoTickerData.LeftMargin + this.toDoTickerData.RightMargin;

                // Adjust left
                this.tickerForm.Left = this.toDoTickerData.LeftMargin;
            }

            // Adjust top
            this.tickerForm.Top = workingAreaHeight - this.tickerForm.Height - this.toDoTickerData.BottomMargin;

            // Handle always on top
            this.tickerForm.TopMost = this.alwaysOnTopToolStripMenuItem.Checked;

            // Show ticker
            this.tickerForm.Show();
        }

        /// <summary>
        /// Handles the original thread donation codercom tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOriginalThreadDonationCodercomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open orignal thread
            Process.Start("https://www.donationcoder.com/forum/index.php?topic=52963.0");
        }

        /// <summary>
        /// Handles the source code githubcom tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open GitHub repository
            Process.Start("https://github.com/publicdomain/todo-ticker");
        }

        /// <summary>
        /// Handles the about tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Set license text
            var licenseText = $"CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication{Environment.NewLine}" +
                $"https://creativecommons.org/publicdomain/zero/1.0/legalcode{Environment.NewLine}{Environment.NewLine}" +
                $"Libraries and icons have separate licenses.{Environment.NewLine}{Environment.NewLine}" +
                $"List checkbox icon by OpenClipart-Vectors - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/vectors/list-checkbox-checked-tick-note-147904/{Environment.NewLine}{Environment.NewLine}" +
                $"Patreon icon used according to published brand guidelines{Environment.NewLine}" +
                $"https://www.patreon.com/brand{Environment.NewLine}{Environment.NewLine}" +
                $"GitHub mark icon used according to published logos and usage guidelines{Environment.NewLine}" +
                $"https://github.com/logos{Environment.NewLine}{Environment.NewLine}" +
                $"DonationCoder icon used with permission{Environment.NewLine}" +
                $"https://www.donationcoder.com/forum/index.php?topic=48718{Environment.NewLine}{Environment.NewLine}" +
                $"PublicDomain icon is based on the following source images:{Environment.NewLine}{Environment.NewLine}" +
                $"Bitcoin by GDJ - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/vectors/bitcoin-digital-currency-4130319/{Environment.NewLine}{Environment.NewLine}" +
                $"Letter P by ArtsyBee - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/illustrations/p-glamour-gold-lights-2790632/{Environment.NewLine}{Environment.NewLine}" +
                $"Letter D by ArtsyBee - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/illustrations/d-glamour-gold-lights-2790573/{Environment.NewLine}{Environment.NewLine}";

            // Set title
            string programTitle = typeof(ToDoTickerForm).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;

            // Set version for generating semantic version
            Version version = typeof(ToDoTickerForm).GetTypeInfo().Assembly.GetName().Version;

            // Set about form
            var aboutForm = new AboutForm(
                $"About {programTitle}",
                $"{programTitle} {version.Major}.{version.Minor}.{version.Build}",
                $"Made for: N.A.N.Y. 2023{Environment.NewLine}DonationCoder.com{Environment.NewLine}Day #1, Week #1 @ January 01, 2023",
                licenseText,
                this.Icon.ToBitmap())
            {
                // Set about form icon
                Icon = this.associatedIcon,

                // Set always on top
                TopMost = this.TopMost
            };

            // Show about form
            aboutForm.ShowDialog();
        }
    }
}