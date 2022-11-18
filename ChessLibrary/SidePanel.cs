﻿using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessLibrary
{
    public partial class SidePanel : UserControl
    {
        public Board ParentBoard { get; set; }
        public string Side { get; }
        #region Constructor
        public SidePanel(Board board, string side = "right")
        {
            InitializeComponent();
            ParentBoard = board;
            Side = side;

            InitSidePanel();
            InitUserProfiles();
            UpdateText(null);

            // delegate events
            ParentBoard.PieceMoved += ParentBoard_PieceMoved;
            Tile.OnSelected += Tile_OnSelected;
        }
        #endregion
        #region Delegate events and Update text method
        private void Tile_OnSelected(Tile tile) => UpdateText(tile);
        private void ParentBoard_PieceMoved() => UpdateText(null);
        private void UpdateText(Tile? tile)
        {
            lbSelected.Text = $"Selected: {tile}";
            //lbSelected.ForeColor = Color.White;

            lbTurn.Text = "Turn:" + " " + ParentBoard.Turn;

            // display history of all moves
            lstMoves.Items.Clear();

            foreach (Tuple<Piece, Tile, Tile> move in ParentBoard.MoveStack)
                lstMoves.Items.Add($"{move.Item1}: x{move.Item2.CoordinateX}, y{move.Item2.CoordinateY} -> " +
                    $"x{move.Item3.CoordinateX}, y{move.Item3.CoordinateY}");
        }
        #endregion
        #region Initialize sidepanel
        private void InitSidePanel()
        {
            // set location to left or right
            this.Location = Side == "left" ? 
                new Point(ParentBoard.Left - this.Width, ParentBoard.Top) : 
                new Point(ParentBoard.Right, ParentBoard.Top);

            this.Width = ParentBoard.Width / 2;
            this.Height = ParentBoard.Height;
            this.BackColor = Color.FromArgb(80, 80, 80);

            ParentBoard.ParentForm.Controls.Add(this);
        }
        private void InitUserProfiles()
        {
            User pOne = ParentBoard.CurrentRoom.PlayerOne;
            User pTwo = ParentBoard.CurrentRoom.PlayerTwo;

            // profile pics
            pbP1Pic.Image = pOne.ProfilePic;
            pbP2Pic.Image = pTwo.ProfilePic;
            // usernames
            lbP1Username.Text = pOne.Username;
            lbP2Username.Text = pTwo.Username;
            // ratings
            lbP1Rating.Text = Convert.ToString(pOne.ChessRating);
            lbP2Rating.Text = Convert.ToString(pTwo.ChessRating);  
        }
        #endregion
        #region Button click events
        private void btnReset_Click(object sender, EventArgs e)
        {
            ParentBoard.DeleteBoard();
            if (ParentBoard.IsDisposed)
                GC.Collect();
            GC.SuppressFinalize(ParentBoard);
        }
        #endregion
        #region Responsive operations
        public void ResponsiveLayout()
        {
            // set location to left or right
            this.Location = Side == "left" ?
                new Point(ParentBoard.Left - this.Width, ParentBoard.Top) :
                new Point(ParentBoard.Right, ParentBoard.Top); this.Height = ParentBoard.Height;

            this.Width = ParentBoard.Width / 2;

            lstMoves.Width = this.Width;
            lstMoves.Height = this.Height / 3;
            lstMoves.BackColor = Color.Black;
            lstMoves.ForeColor = Color.White;
            lstMoves.Location = new Point(0, this.Height - lstMoves.Height);
        }
        #endregion
    }
}