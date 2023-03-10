// <copyright file="ToDoTickerData.cs" company="PublicDomain">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>
namespace PublicDomain
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// ToDo-Ticker data.
    /// </summary>
    [Serializable]
    public class ToDoTickerData
    {
        /// <summary>
        /// Gets or sets the timer interval.
        /// </summary>
        /// <value>The timer interval.</value>
        public int TimerInterval { get; set; }

        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        /// <value>The left margin.</value>
        public int LeftMargin { get; set; }

        /// <summary>
        /// Gets or sets the right margin.
        /// </summary>
        /// <value>The right margin.</value>
        public int RightMargin { get; set; }

        /// <summary>
        /// Gets or sets the bottom margin.
        /// </summary>
        /// <value>The bottom margin.</value>
        public int BottomMargin { get; set; }

        /// <summary>
        /// Gets or sets the text font.
        /// </summary>
        /// <value>The text font.</value>
        public string TextFont { get; set; }

        /// <summary>
        /// Gets or sets the separator.
        /// </summary>
        /// <value>The separator.</value>
        public string Separator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:PublicDomain.ToDoTickerData"/> always on top.
        /// </summary>
        /// <value><c>true</c> if always on top; otherwise, <c>false</c>.</value>
        public bool AlwaysOnTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:PublicDomain.ToDoTickerData"/> full width.
        /// </summary>
        /// <value><c>true</c> if full width; otherwise, <c>false</c>.</value>
        public bool FullWidth { get; set; }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>The color of the foreground.</value>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the list items.
        /// </summary>
        /// <value>The list items.</value>
        public List<string> ListItems { get; set; }
    }
}
