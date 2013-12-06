using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace Checkers
{
    // Note that the board is numbered left-to-right, top-to-bottom starting in the upper-left hand
    // corner at 1 going to the bottom right-hand corner at 64. As can be seen in the Board.cs
    // file, the pieces start at ovalShape1, which corresponds to the GUI object with the same
    // Name, and goes to 24; the first 12 are black pieces, and the last 12 are red pieces. The GUI
    // objects' Tag properties are set to "r,n" for "Red, not a king", "b,k" for "Black, king", and
    // so on. In this way, when a black piece is kinged, it's Tag is changed from "b,n" to "b,k".
    public partial class CheckersForm : Form
    {
        // holds data about where pieces are located and provides methods to correspond locations
        // to positions
        private Board MyBoard {get; set;}

        // these are set while the potential move is evaluated; if the move is valid, they will be
        // updated appropriately so that the underlying referenced pieces are updated
        private OvalShape PieceToMove { get; set; }
        private OvalShape PieceToTake { get; set; }

        // this is a collection of the pieces to enable Linq queries
        private List<OvalShape> Ovals { get; set; }

        // when either of these hits 0, the game is over
        private int PiecesRemainingRed { get; set; }
        private int PiecesRemainingBlack { get; set; }

        // stores whose turn it is
        private bool IsRedTurn { get; set; }

        public CheckersForm()
        {
            InitializeComponent();
            this.MyBoard = new Board();
            this.IsRedTurn = true;
            this.PieceToMove = null;
            this.PieceToTake = null;
            txtMessageBox.Text = "";
            this.PiecesRemainingRed = 12;
            this.PiecesRemainingBlack = 12;

            this.Ovals = new List<OvalShape>();
            this.Ovals.Add(ovalShape1);
            this.Ovals.Add(ovalShape2);
            this.Ovals.Add(ovalShape3);
            this.Ovals.Add(ovalShape4);
            this.Ovals.Add(ovalShape5);
            this.Ovals.Add(ovalShape6);
            this.Ovals.Add(ovalShape7);
            this.Ovals.Add(ovalShape8);
            this.Ovals.Add(ovalShape9);
            this.Ovals.Add(ovalShape10);
            this.Ovals.Add(ovalShape11);
            this.Ovals.Add(ovalShape12);
            this.Ovals.Add(ovalShape13);
            this.Ovals.Add(ovalShape14);
            this.Ovals.Add(ovalShape15);
            this.Ovals.Add(ovalShape16);
            this.Ovals.Add(ovalShape17);
            this.Ovals.Add(ovalShape18);
            this.Ovals.Add(ovalShape19);
            this.Ovals.Add(ovalShape20);
            this.Ovals.Add(ovalShape21);
            this.Ovals.Add(ovalShape22);
            this.Ovals.Add(ovalShape23);
            this.Ovals.Add(ovalShape24);
        }

        /// <summary>
        /// This is the method called whenever an OvalShape is clicked on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ovalShape_Click(object sender, EventArgs e)
        {
            // check to make sure there are both black and red pieces left
            if (this.PiecesRemainingBlack == 0 || this.PiecesRemainingRed == 0)
            {
                txtMessageBox.Text = "GAME IS OVER!!! Please close and reopen to start again.";
                return;
            }

            OvalShape ovalSender = (OvalShape)sender;
            txtMessageBox.Text = "";

            // if user clicks on another piece while one is already selected, give error message
            if (this.PieceToMove != null)
            {
                txtMessageBox.Text = "There is a piece there already!";
                this.PieceToMove = null;
                return;
            }

            // Check whose turn it is and set error message if not your turn
            if (!isMyTurnCheck(ovalSender))
            {
                return;
            }

            // everything is good, set this.PieceToMove
            this.PieceToMove = ovalSender;
        }
 
        /// <summary>
        /// This is the method called whenever a RectangleShape that is a black square is clicked
        /// on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rectangleShape_Click(object sender, EventArgs e)
        {
            // check to make sure there are both black and red pieces left
            if (this.PiecesRemainingBlack == 0 || this.PiecesRemainingRed == 0)
            {
                txtMessageBox.Text = "GAME IS OVER!!! Please close and reopen to start again.";
                return;
            }

            RectangleShape boardSpot = (RectangleShape)sender;

            if (txtMessageBox.Text == "It's red's turn!" ||
                txtMessageBox.Text == "It's black's turn!")
            {
                return;
            }

            txtMessageBox.Text = "";

            // Check if piece was selected:
            if (this.PieceToMove == null)
            {
                txtMessageBox.Text = "No piece to move!";
                return;
            }

            // Check if space is empty:
            bool isSpaceEmpty = checkEmptySpace(boardSpot);
            if (!isSpaceEmpty)
            {
                txtMessageBox.Text = "Board space not empty!";
                this.PieceToMove = null;
                return;
            }

            // Check if distance to move is valid:
            string checkMoveText = checkMove(boardSpot);
            if (checkMoveText != "")
            {
                txtMessageBox.Text = checkMoveText;
                this.PieceToMove = null;
                return;
            }

            // Check to see if the piece landed on the far squares, and if so make it a king
            makeKing(boardSpot);

            // Move the piece:
            movePiece(boardSpot);
        }

        #region Helper Methods

        /// <summary>
        /// This method determines whether a piece should be made king. If so, it makes that piece
        /// a king by changing it's tag from "b,n" to "b,k" or "r,n" to "r,k" and by changing its
        /// color.
        /// </summary>
        /// <param name="boardSpot">the RectangleShape the user clicked on</param>
        private void makeKing(RectangleShape boardSpot)
        {
            // check that there is a piece to move
            if (this.PieceToMove == null)
            {
                return;
            }

            // get position of the square clicked on
            int[] myPosition = this.MyBoard.ConvertLocToBoardPos(boardSpot.Location,
                "RectangleShape");

            if (this.PieceToMove.Tag.ToString() == "r,n" &&
                myPosition[0] < 9)                                  // red piece is ready to king
            {
                this.PieceToMove.Tag = "r,k";
                this.PieceToMove.BorderColor = Color.Red;
            }
            else if (this.PieceToMove.Tag.ToString() == "b,n" &&
                myPosition[0] > 55)                                 // black piece is ready to king
            {
                this.PieceToMove.Tag = "b,k";
                this.PieceToMove.BorderColor = Color.Gray;
            }
        }

        //// set this.PieceToMove, return false if there was a piece there already
        //private string setPieceToMove(OvalShape ovalSender)
        //{
        //    if (this.PieceToMove == null)       // piece is not already selected
        //    {
        //        this.PieceToMove = ovalSender;
        //        return "";
        //    }
        //    // clicked on one piece then another:
        //    else
        //    {
        //        return "There is a piece already there!";
        //    }
        //}

        // Check whose turn it is and return error if not your turn

        /// <summary>
        /// Check whose turn it is and return false bool if not your turn
        /// </summary>
        /// <param name="ovalSender">This is the OvalShape that the user clicked on</param>
        /// <returns>Returns bool true if it is your turn and false otherwise</returns>
        private bool isMyTurnCheck(OvalShape ovalSender)
        {
            if (IsRedTurn)                                                  // it's red's turn
            {
                if (ovalSender.Tag.ToString() == "b,n" ||
                    ovalSender.Tag.ToString() == "b,k")                     // piece is black
                {
                    txtMessageBox.Text = "It's red's turn!";
                    return false;
                }
            }
            else if (!IsRedTurn)                                            // it's black's turn
            {
                if (ovalSender.Tag.ToString() == "r,n" ||
                    ovalSender.Tag.ToString() == "r,k")                     // piece is red
                {
                    txtMessageBox.Text = "It's black's turn!";
                    return false;
                }
            }

            return true;                                                    // it is your turn
        }

        /// <summary>
        /// Check that the move is valid. Return a string error message if it is not, and the empty
        /// string "" if it is.
        /// </summary>
        /// <param name="boardSpot">the RectangleShape the user clicked on</param>
        /// <returns>string with either the error message or "" indicating no error</returns>
        private string checkMove(RectangleShape boardSpot)
        {
            //bool isGoodDistance = false;
            bool isJump = false;

            // convert the boardSpot and the piece that is moving locations to positions
            int[] boardPos = this.MyBoard.ConvertLocToBoardPos(boardSpot.Location, "RectangleShape");
            int[] movePiecePos = this.MyBoard.ConvertLocToBoardPos(
                this.PieceToMove.Location, "OvalShape");

            int deltaPosX = boardPos[1] - movePiecePos[1];
            int deltaPosY = boardPos[2] - movePiecePos[2];

            // if 2 spaces being moved in x- and y-directions, it is a jump (note that double-
            //   jumping is not currently supported
            if (Math.Abs(deltaPosX) == 2 && Math.Abs(deltaPosY) == 2)
            {
                isJump = true;
            }

            if (isJump)                                                             // is a jump
            {
                // find the position that would be jumped by taking the average of current location
                //   and the desired location
                int possibleJumpPiecePosX = (boardPos[1] + movePiecePos[1]) / 2;
                int possibleJumpPiecePosY = (boardPos[2] + movePiecePos[2]) / 2;
                int[] possibleJumpPiecePos = { possibleJumpPiecePosX, possibleJumpPiecePosY};

                int[] possibleJumpLoc = this.MyBoard.ConvertBoardPosToLoc(
                    possibleJumpPiecePos, "OvalShape");

                // if no piece to jump in the location, return error message
                if (this.MyBoard.PiecesOnBoard[possibleJumpLoc[0]] == null)
                {
                    return "No piece to jump!";
                }
                else
                {
                    // set this.PieceToTake appropriately
                    this.PieceToTake = Ovals.Where(o => o.Location.X == possibleJumpLoc[1]).Where(
                        o2 => o2.Location.Y == possibleJumpLoc[2]).First();
                }

                // Check that you are not jumping a piece of the same color
                if ((this.PieceToTake.Tag.ToString() == "b,n" ||
                    this.PieceToTake.Tag.ToString() == "b,k") &&
                    (this.PieceToMove.Tag.ToString() == "b,n" ||
                    this.PieceToMove.Tag.ToString() == "b,k"))      // black jumping black
                {
                    this.PieceToTake = null;
                    return "Cannot jump black piece!!!";
                }
                else if ((this.PieceToTake.Tag.ToString() == "r,n" ||
                    this.PieceToTake.Tag.ToString() == "r,k") &&
                    (this.PieceToMove.Tag.ToString() == "r,n" ||
                    this.PieceToMove.Tag.ToString() == "r,k"))      // red jumping red
                {
                    this.PieceToTake = null;
                    return "Cannot jump red piece!!!";
                }

                // Check black non-king pieces for good move distance
                if (this.PieceToMove.Tag.ToString() == "r,n")                   // piece is red
                {
                    if (deltaPosY == -2 && (deltaPosX == 2 || deltaPosX == -2))
                    {
                        return "";
                    }
                    else
                    {
                        return "Invalid black jump!";
                    }
                }

                // Check red non-king pieces for good move distance
                if (this.PieceToMove.Tag.ToString() == "b,n")                   // piece is red
                {
                    if (deltaPosY == 2 && (deltaPosX == 2 || deltaPosX == -2))
                    {
                        return "";
                    }
                    else
                    {
                        return "Invalid red jump!";
                    }
                }

                //// Check kings for good move distance
                //if (this.PieceToMove.Tag.ToString() == "r,k" ||
                //        this.PieceToMove.Tag.ToString() == "b,k")               // piece is a king
                //{
                //    if (Math.Abs(deltaPosY) == 2 && Math.Abs(deltaPosX) == 2)
                //    {
                //        return "";
                //    }
                //    else
                //    {
                //        return "Invalid king jump!!!";
                //    }
                //}

            }
            else                                                                    // not a jump
            {
                // Check red non-king pieces for good move distance
                if (this.PieceToMove.Tag.ToString() == "r,n")                       // piece is red
                {
                    if (deltaPosY == -1 && (deltaPosX == 1 || deltaPosX == -1))
                    {
                        return "";
                    }
                    else
                    {
                        return "Invalid red move!";
                    }
                }

                // Check black non-king pieces for good move distance
                if (this.PieceToMove.Tag.ToString() == "b,n")                   // piece is black
                {
                    if (deltaPosY == 1 && (deltaPosX == 1 || deltaPosX == -1))
                    {
                        return "";
                    }
                    else
                    {
                        return "Invalid black move!";
                    }
                }

                // Check kings for good move distance
                if (this.PieceToMove.Tag.ToString() == "r,k" ||
                        this.PieceToMove.Tag.ToString() == "b,k")               // piece is a king
                {
                    if (Math.Abs(deltaPosY) == 1 && Math.Abs(deltaPosX) == 1)
                    {
                        return "";
                    }
                    else
                    {
                        return "Invalid king move!";
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// This is called after checkMove validates the move is legal, and it updates all the
        /// appropriate values to signify a successful move.
        /// </summary>
        /// <param name="boardSpot">the RectangleShape the user clicked on</param>
        private void movePiece(RectangleShape boardSpot)
        {
            int oldSpot = 0;

            // loop through PiecesOnBoard to find the old spot number
            for (int i = 1; i < 65; i++)
            {
                if (this.MyBoard.PiecesOnBoard[i] == this.PieceToMove.Name)
                {
                    oldSpot = i;
                    break;
                }
            }

            // set this.MyBoard.PiecesOnBoard to the new values to reflect the move
            string spotNumber = boardSpot.Name.ToString().Split('e')[3];
            int intSpot = Convert.ToInt16(spotNumber);
            this.MyBoard.PiecesOnBoard[intSpot] = this.PieceToMove.Name;
            this.MyBoard.PiecesOnBoard[oldSpot] = null;     // remove old location

            // add 8 to the boardSpot location values to account for piece offset
            Point newLoc = new Point(8 + boardSpot.Location.X,
                8 + boardSpot.Location.Y);

            this.PieceToMove.Location = newLoc;             // move the piece in the GUI
            this.PieceToMove = null;

            // if there is a piece to take:
            if (this.PieceToTake != null)
            {
                // get the position of the piece to take
                int[] pieceToTakePos = this.MyBoard.ConvertLocToBoardPos(this.PieceToTake.Location,
                    "OvalShape");

                // move the OvalShape that was taken to (-100,-100)
                OvalShape ovalToRemove = new OvalShape();
                ovalToRemove = Ovals.Where(o => o.Name == this.PieceToTake.Name).First();
                ovalToRemove.Location = new Point(-100, -100);

                // remove the piece from this.MyBoard.PiecesOnBoard
                this.MyBoard.PiecesOnBoard[pieceToTakePos[0]] = null;

                // subtract one from pieces remaining every time a piece is captured
                if (this.PieceToTake.Tag.ToString() == "r,n" || this.PieceToTake.Tag.ToString() == "r,k")
                {
                    this.PiecesRemainingRed -= 1;
                }
                else
                {
                    this.PiecesRemainingBlack -= 1;
                }

                // print who wins to the message box if pieces remaining for the opponent is 0
                if (this.PiecesRemainingBlack == 0)
                {
                    txtMessageBox.Text = "RED WINS!!!";
                }
                else if (this.PiecesRemainingRed == 0)
                {
                    txtMessageBox.Text = "BLACK WINS!!!";
                }

                this.PieceToTake = null;
            }

            // Move succeeded, change whose turn it it:
            if (this.IsRedTurn)
                this.IsRedTurn = false;
            else
                this.IsRedTurn = true;
        }

        /// <summary>
        /// This function checks if the space is empty.
        /// </summary>
        /// <param name="boardSpot">the RectangleShape the user clicked on</param>
        /// <returns>returns bool false if the board space is not empty</returns>
        private bool checkEmptySpace(RectangleShape boardSpot)
        {
            string spotNumber = boardSpot.Name.ToString().Split('e')[3];
            int intSpot = Convert.ToInt16(spotNumber);

            if (this.MyBoard.PiecesOnBoard[intSpot] != null)
                return false;

            return true;
        }

        #endregion
    }
}
