using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Piece
    {

        // !!!!!!!!!!!!!!!!!!!!!!
        // THIS IS NO LONGER USED
        // !!!!!!!!!!!!!!!!!!!!!!


        //private string Name { get; set; }       // corresponds to "ovalShapeXX"
        //private string Color { get; set; }      // either red or black
        //private int XPos { get; set; }
        //private int YPos { get; set; }
        //private bool IsKing { get; set; }

        //private static int[] PieceCoords =
        //{
        //    0,      // DO NOT USE THIS LOCATION VALUE
        //    8,      // 1
        //    79,     // 2
        //    150,    // 3
        //    221,    // 4
        //    292,    // 5
        //    363,    // 6
        //    434,    // 7
        //    505     // 8
        //};

        //private static char[] LetterVals =
        //{
        //    '0',    // DO NOT USE THIS LOCATION VALUE
        //    'A',    // 1
        //    'B',    // 2
        //    'C',    // 3
        //    'D',    // 4
        //    'E',    // 5
        //    'F',    // 6
        //    'G',    // 7
        //    'H',    // 8
        //};

        //public Piece(string pieceName, int xPos, int yPos, string color)
        //{
        //    this.Name = pieceName;
        //    this.XPos = xPos;
        //    this.YPos = yPos;
        //    this.IsKing = false;
        //    this.Color = color;
        //}

        //#region methods

        //private string GetPosition()
        //{
        //    string strPos = "";

        //    for (int i = 1; i <= 8; i++)
        //    {
        //        if (this.XPos == PieceCoords[i])
        //        {
        //            strPos += LetterVals[i];
        //        }
        //    }

        //    for (int i = 1; i <= 8; i++)
        //    {
        //        if (this.YPos == PieceCoords[i])
        //        {
        //            strPos += i.ToString();
        //        }
        //    }

        //    return strPos;
        //}

        //private void SetPosition(string position)
        //{
        //    char firstLetter = position[0];
        //    char secondLetter = position[1];
        //    int intSecondLetter = -1;

        //    // Setting the xPos property
        //    for (int i = 1; i <= 8; i++)
        //    {
        //        if (firstLetter == LetterVals[i])
        //        {
        //            intSecondLetter = i;
        //        }
        //    }
        //    if (intSecondLetter < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("intSecondLetter was not properly set");
        //    }
        //    this.XPos = PieceCoords[intSecondLetter];

        //    // Setting the yPos property
        //    this.YPos = PieceCoords[secondLetter];
        //}

        //#endregion


    }
}
