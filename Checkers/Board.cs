using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.PowerPacks;

namespace Checkers
{

    // Note that the board is numbered left-to-right, top-to-bottom starting in the upper-left hand
    // corner at 1 going to the bottom right-hand corner at 64. As can be seen below, the pieces
    // start at ovalShape1, which corresponds to the GUI object with the same Name, and goes to 24;
    // the first 12 are black pieces, and the last 12 are red pieces. The GUI objects' Tag
    // properties are set to "r,n" for "Red, not a king", "b,k" for "Black, king", and so on.
    public class Board
    {
        public string[] PiecesOnBoard { get; set; }

        public Board()
        {
            string[] strArray = {
                                    null, "ovalShape1", null, "ovalShape2", null, "ovalShape3", null, "ovalShape4", null,
                                        null, "ovalShape5", null, "ovalShape6", null, "ovalShape7", null, "ovalShape8",
                                        "ovalShape9", null, "ovalShape10", null, "ovalShape11", null, "ovalShape12", null,
                                        null, null, null, null, null, null, null, null,
                                        null, null, null, null, null, null, null, null,
                                        null, "ovalShape13", null, "ovalShape14", null, "ovalShape15", null, "ovalShape16",
                                        "ovalShape17", null, "ovalShape18", null, "ovalShape19", null, "ovalShape20", null,
                                        null, "ovalShape21", null, "ovalShape22", null, "ovalShape23", null, "ovalShape24"
                                };
            
            this.PiecesOnBoard = strArray;
        }

        /// <summary>
        /// Converts the location to position delivered in a 3-value integer array.
        /// </summary>
        /// <param name="location">this is a value of type System.Drawing.Point</param>
        /// <param name="type">either OvalShape or RectangleShape</param>
        /// <returns>a 3-value integer array where the first value is the board space number,
        /// second value is the x-position (1 to 8), and third value is the y-position</returns>
        public int[] ConvertLocToBoardPos(Point location, string type)
        {
            int[] myPosition = { 0, 0, 0 };
            int[] positions = { 0, 1, 2, 3, 4, 5, 6, 7, 8};     // temporary initialization

            // set the values of positions based upon whether it is a RectangleShape or OvalShape
            if (type == "RectangleShape")
            {
                int[] tempArr = { 0, 0, 71, 142, 213, 284, 355, 426, 497 };
                positions = tempArr;
            }
            else if (type == "OvalShape")
            {
                int[] tempArr = { 0, 8, 79, 150, 221, 292, 363, 434, 505 };
                positions = tempArr;
            }           
            
            // loop through 1 to 8 for both x- and y-values and set myPosition appropriately
            for (int i = 1; i <= 8; i++)
            {
                if (location.X == positions[i])
                {
                    myPosition[1] = i;
                    myPosition[0] += i;
                }
            }
            for (int i = 1; i <= 8; i++)
            {
                if (location.Y == positions[i])
                {
                    myPosition[2] = i;
                    myPosition[0] += (i - 1) * 8;
                }
            }

            return myPosition;
        }

        /// <summary>
        /// This is the reverse of ConvertLocToBoardPos and converts the board position to
        /// location, delivered as a 3-value integer array.
        /// </summary>
        /// <param name="boardPos">this is a 2-digit integer array that contains the x-value in
        /// its first index and the y-value in its second index</param>
        /// <param name="type">either OvalShape or RectangleShape</param>
        /// <returns>a 3-value integer array where the first value is the board space number,
        /// second value is the x location, and third value is the y location</returns>
        public int[] ConvertBoardPosToLoc(int[] boardPos, string type)
        {
            int[] location = { 0, 0, 0 };          // the first integer is the board space number
            int[] positions = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };        // temporary initialization

            // set the values of positions based upon whether it is a RectangleShape or OvalShape
            if (type == "RectangleShape")
            {
                int[] tempArr = { 0, 0, 71, 152, 213, 284, 355, 426, 497 };
                positions = tempArr;
            }
            else if (type == "OvalShape")
            {
                int[] tempArr = { 0, 8, 79, 150, 221, 292, 363, 434, 505 };
                positions = tempArr;
            }

            location[0] = boardPos[0] + (boardPos[1] - 1) * 8;
            location[1] = positions[boardPos[0]];
            location[2] = positions[boardPos[1]];
            
            return location;
        }
    }

    enum BoardSpaces                                                // NOT USED
    {
        A1 = 1, A2, A3, A4, A5, A6, A7, A8,
        B1, B2, B3, B4, B5, B6, B7, B8,
        C1, C2, C3, C4, C5, C6, C7, C8,
        D1, D2, D3, D4, D5, D6, D7, D8,
        E1, E2, E3, E4, E5, E6, E7, E8,
        F1, F2, F3, F4, F5, F6, F7, F8,
        G1, G2, G3, G4, G5, G6, G7, G8,
        H1, H2, H3, H4, H5, H6, H7, H8
    }
}
