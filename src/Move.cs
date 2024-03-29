using System;
using System.Collections.Generic;
using System.Linq;

namespace TimHanewich.Chess
{
    public class Move
    {
        public Position FromPosition {get; set;}
        public Position ToPosition {get; set;}

        //Castling store
        public CastlingType? Castling {get; set;}

        //Pawn promotion store
        public PieceType PromotePawnTo {get; set;}

        #region "Constructors"

        public Move()
        {

        }

        public Move(Position from, Position to)
        {
            FromPosition = from;
            ToPosition = to;
        }

        public Move(Position from, Position to, PieceType promote_pawn_to)
        {
            FromPosition = from;
            ToPosition = to;
            PromotePawnTo = promote_pawn_to;
        }

        public Move(CastlingType castling)
        {
            Castling = castling;
        }

        #endregion

        public bool IsPawnPromotion(BoardPosition position)
        {
            Piece p = position.FindOccupyingPiece(FromPosition);
            if (p == null)
            {
                return false;
            }
            if (p.Type != PieceType.Pawn)
            {
                return false;
            }
            
            //Check
            if (p.Color == Color.White)
            {
                if (ToPosition.Rank() == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ToPosition.Rank() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    
        #region "Move sorting"

        public bool IsCapture(BoardPosition position)
        {
            Piece PieceToCapture = position.FindOccupyingPiece(ToPosition);
            if (PieceToCapture != null) //There is a piece on the ToPosition. So therefore, it is a capture. Do not need to test the color because you wouldn't be able to move to a square where one of your pieces is anyway.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Returns the difference in capture material.
        //For example, capturing a queen with a pawn is 10 - 1 = 9. Capturing a Rook with a bishop is 5 - 3 = 2
        //If this is NOT a capture, null is returned.
        public float? CaptureValue(BoardPosition position)
        {
            Piece Captured = position.FindOccupyingPiece(ToPosition);
            if (Captured == null) //If we are not capturing anything, return null
            {
                return null;
            }
            Piece Capturing = position.FindOccupyingPiece(FromPosition);
            return Captured.Value - Captured.Value;
        }

        public static Move[] OrderMovesForEvaluation(BoardPosition position, Move[] moves)
        {

            //Assess the capture value for each move
            List<KeyValuePair<Move, float?>> kvps = new List<KeyValuePair<Move, float?>>();
            foreach (Move m in moves)
            {
                float? ThisMoveCaptureValue = m.CaptureValue(position);
                kvps.Add(new KeyValuePair<Move, float?>(m, ThisMoveCaptureValue));
            }

            //Separate those that have values and those that do not
            List<KeyValuePair<Move, float?>> HaveValues = new List<KeyValuePair<Move, float?>>();
            List<Move> DoNotHaveValues = new List<Move>();
            foreach (KeyValuePair<Move, float?> kvp in kvps)
            {
                if (kvp.Value.HasValue)
                {
                    HaveValues.Add(kvp);
                }
                else
                {
                    DoNotHaveValues.Add(kvp.Key);
                }
            }
    
            //Sort the best by their value
            List<Move> ToReturn = new List<Move>();
            while (HaveValues.Count > 0)
            {
                KeyValuePair<Move, float?> winner = HaveValues[0];
                foreach (KeyValuePair<Move, float?> kvp in HaveValues)
                {
                    if (kvp.Value.Value > winner.Value.Value)
                    {
                        winner = kvp;
                    }
                }
                ToReturn.Add(winner.Key);
                HaveValues.Remove(winner);
            }

            //Add the rest that do not have values
            ToReturn.AddRange(DoNotHaveValues.ToArray());

            return ToReturn.ToArray();

        }


        #endregion

        #region "Algebraic notation"

        public string ToAlgebraicNotation(BoardPosition position)
        {
            //Castling?
            if (Castling.HasValue)
            {
                if (Castling.Value == CastlingType.KingSide)
                {
                    return "O-O";
                }
                else if (Castling.Value == CastlingType.QueenSide)
                {
                    return "O-O-O";
                }
            }

            Piece MovingPiece = position.FindOccupyingPiece(FromPosition);
            if (MovingPiece == null)
            {
                throw new Exception("Move Unable to convert move " + FromPosition.ToString() + " to " + ToPosition.ToString() + " to algebraic notation. Piece not found on from position square.");
            }

            //Get the beginning piece notation
            string PieceNotation = GetPieceNotation(MovingPiece.Type);
            

            //Is this a take? Is it a capture of an enemy piece?
            string CaptureNotation = "";
            Piece CapturingPiece = position.FindOccupyingPiece(ToPosition);
            if (CapturingPiece != null)
            {
                if (MovingPiece.Type == PieceType.Pawn)
                {
                    if (CapturingPiece.Color != MovingPiece.Color)
                    {
                        CaptureNotation = PositionToolkit.File(MovingPiece.Position).ToString().ToLower() + "x";
                    }
                }
                else
                {
                    if (CapturingPiece.Color != MovingPiece.Color)
                    {
                        CaptureNotation = "x";
                    }
                }
            }

            //Disambiguating move?
            string DisambiguatingNotation = "";
            //Check if there are any other pieces of the same type that can also move to the destination square
            if (MovingPiece.Type != PieceType.Pawn) //Pawns do not need disambigurating moves (well, actually this is required for pawns, but I handle the disambiguating move in the take notation section above somewhere)
            {
                foreach (Piece p in position.Pieces)
                {
                    if (p.Position != FromPosition)
                    {
                        if (p.Color == MovingPiece.Color)
                        {
                            if (p.Type == MovingPiece.Type)
                            {

                                //Can this piece also move to the destination?
                                bool ThisPieceCanMoveThereToo = false;
                                foreach (Move ppp in p.AvailableMoves(position, true))
                                {
                                    if (ppp.ToPosition == ToPosition)
                                    {
                                        ThisPieceCanMoveThereToo = true;
                                    }
                                }
                                
                                //If it can move there too, disambiguate
                                if (ThisPieceCanMoveThereToo)
                                {
                                    //What is the difference? Is it rank or is it file?
                                    if (p.Position.Rank() == MovingPiece.Position.Rank())
                                    {
                                        DisambiguatingNotation = MovingPiece.Position.File().ToString().ToLower();
                                    }
                                    else if (p.Position.File() == MovingPiece.Position.File())
                                    {
                                        DisambiguatingNotation = MovingPiece.Position.Rank().ToString();
                                    }
                                    else //For example, moving a queen to G2 in this position: 8/1q4Q1/2k5/1n6/8/8/7Q/4K3 w - - 3 3
                                    {
                                        DisambiguatingNotation = MovingPiece.Position.File().ToString().ToLower();
                                    }
                                }   
                            }
                        }
                    }
                }
            }
            


            //Get the Position part
            string PositionNotation = ToPosition.ToString().ToLower();

            //Pawn promotion?
            string PromotionNotation = "";
            if (IsPawnPromotion(position))
            {
                PromotionNotation = "=" + GetPieceNotation(PromotePawnTo);
            }

            //Is it it a check or check mate?
            string CheckCheckMateNotation = "";
            BoardPosition ResultingPosition = position.Copy();
            ResultingPosition.ExecuteMove(this);
            if (ResultingPosition.IsCheckMate())
            {
                CheckCheckMateNotation = "#";
            }
            else if (ResultingPosition.IsCheck())
            {
                CheckCheckMateNotation = "+";
            }


            //String it all together and return
            string ToReturn = PieceNotation + DisambiguatingNotation + CaptureNotation + PositionNotation + PromotionNotation + CheckCheckMateNotation;
            return ToReturn;
        }
    
        public string ToLongAlgebraicNotation(BoardPosition position)
        {

            //Take care of castling separately
            if (Castling.HasValue)
            {
                if (position.ToMove == Color.White)
                {
                    if (Castling.Value == CastlingType.KingSide)
                    {
                        return "e1g1";
                    }
                    else if (Castling.Value == CastlingType.QueenSide)
                    {
                        return "e1c1";
                    }
                }
                else if (position.ToMove == Color.Black)
                {
                    if (Castling.Value == CastlingType.KingSide)
                    {
                        return "e8g8";
                    }
                    else if (Castling.Value == CastlingType.QueenSide)
                    {
                        return "e8c8";
                    }
                }
            }

            //Preapre normal
            string ToReturn = FromPosition.ToString().ToLower() + ToPosition.ToString().ToLower();

            //Append pawn promotion code?
            if (IsPawnPromotion(position))
            {
                ToReturn = ToReturn + GetPieceNotation(PromotePawnTo).ToLower();
            }

            return ToReturn;

        }

        private string GetPieceNotation(PieceType type)
        {
            string ToReturn = "";
            if (type == PieceType.Rook)
            {
                ToReturn = "R";
            }
            else if (type ==  PieceType.King)
            {
                ToReturn = "K";
            }
            else if (type == PieceType.Queen)
            {
                ToReturn = "Q";
            }
            else if (type == PieceType.Bishop)
            {
                ToReturn = "B";
            }
            else if (type == PieceType.Knight)
            {
                ToReturn = "N";
            }
            return ToReturn;
        }
    
        public Move(string algebraic_notation, BoardPosition position)
        {

            //Check for castling
            //Since this uses "contains", the queen side castling check MUST come first. Because the king-side castling notation is a substring of the queenside, if we put the king side check first, it would deem even queenside castle notation (O-O-O) as king-side.
            if (algebraic_notation.ToLower().Contains("O-O-O".ToLower())) //Must be contains, not ==, because there could be a castling that puts the king in check ("O-O-O+") and that would not trigger this to determine it as castling.
            {
                Castling = CastlingType.QueenSide;
                return;
            }
            else if (algebraic_notation.ToLower().Contains("O-O".ToLower()))  //Must be contains, not ==, because there could be a castling that puts the king in check ("O-O+") and that would not trigger this to determine it as castling.
            {
                Castling = CastlingType.KingSide;
                return;
            }
            
            string disecting = algebraic_notation;

            //Strip out the x
            disecting = disecting.Replace("x", "");

            //Get the to position - may be able to replace this later with the new `FindDestinationPosition` method below
            int? DestinationRankPosition = FindLastNumber(disecting);
            if (!DestinationRankPosition.HasValue)
            {
                throw new InvalidOperationException("Invalid move notation!");
            }
            int DestinationRank = disecting[DestinationRankPosition.Value] - '0';
            string DestinationFile = disecting.Substring(DestinationRankPosition.Value - 1, 1);
            ToPosition = PositionToolkit.Parse(DestinationFile.Trim().ToUpper() + DestinationRank.ToString());

            //What piece is this moving?
            PieceType Moving;
            string PieceNotation = algebraic_notation.Substring(0, 1);
            if (PieceNotation == "K")
            {
                Moving = PieceType.King;
            }
            else if (PieceNotation == "Q")
            {
                Moving = PieceType.Queen;
            }
            else if (PieceNotation == "R")
            {
                Moving = PieceType.Rook;
            }
            else if (PieceNotation == "B")
            {
                Moving = PieceType.Bishop;
            }
            else if (PieceNotation == "N")
            {
                Moving = PieceType.Knight;
            }
            else
            {
                Moving = PieceType.Pawn;
            }

            //Is this a pawn promotion (has "=" in it)
            if (algebraic_notation.Contains("="))
            {
                int loc1 = algebraic_notation.IndexOf("=");
                string promote_pawn_to = algebraic_notation.Substring(loc1 + 1, 1).ToUpper();
                if (promote_pawn_to == "Q")
                {
                    PromotePawnTo = PieceType.Queen;
                }
                else if (promote_pawn_to == "R")
                {
                    PromotePawnTo = PieceType.Rook;
                }
                else if (promote_pawn_to == "B")
                {
                    PromotePawnTo = PieceType.Bishop;
                }
                else if (promote_pawn_to == "N")
                {
                    PromotePawnTo = PieceType.Knight;
                }
            }
            
            
            //Get the from position
            var pieces = position.Pieces.Where(p => p.Color == position.ToMove && p.Type == Moving); //List of pieces that meet this criteria (same color, same type)

            //Compile a list of pieces that have the capability of moving to this position
            List<Piece> PotentialMovingPieces = new List<Piece>();
            foreach (Piece p in pieces)
            {
                Move[] moves = p.AvailableMoves(position, true);
                foreach (Move m in moves)
                {
                    if (m.ToPosition == ToPosition)
                    {
                        if (PotentialMovingPieces.Contains(p) == false)
                        {
                            PotentialMovingPieces.Add(p);
                        }
                    }
                }
            }

            //If there is only a single piece on the board that meets the criteria and is capable of moving to the destination position, that must be it!
            if (PotentialMovingPieces.Count == 0)
            {
                throw new Exception("No pieces of the specified piece type in the notation are able to move to the destination position '" + ToPosition.ToString() + "'");
            }
            else if (PotentialMovingPieces.Count == 1)
            {
                FromPosition = PotentialMovingPieces[0].Position;
            }
            else if (PotentialMovingPieces.Count > 1) //There are multiple pieces of that type and color that can make this move. So we must use the disambiguating move notation to determine which it is.
            {
                string disambiguating = algebraic_notation;
                disambiguating = disambiguating.Replace("x", "");
                disambiguating = disambiguating.Replace("+", "");
                disambiguating = disambiguating.Replace("=Q", "");
                disambiguating = disambiguating.Replace("=R", "");
                disambiguating = disambiguating.Replace("=B", "");
                disambiguating = disambiguating.Replace("=N", "");
                disambiguating = disambiguating.Replace("#", "");
                disambiguating = disambiguating.Replace(ToPosition.ToString().ToLower(), "");
                
                
                //If this is a pawn moving, the entire string above will be lowercase (only one character left at this point, the disambigurating rank or file). And if that is the case, the entire thing would be in lower case because pawn moves to not start their algebraic notation with a capital letter, or a letter at all at that.
                //So, in other words, if the leftover string has a capital in it (checked via the conditional statement below), the first letter must be the notation of the piece being moved (i.e. N, R).
                //And if the first letter is the piece being moved, we don't want that. We have interest in the single character that comes after it. That is the disambiguating rank or file.
                if (disambiguating != disambiguating.ToLower())
                {
                    disambiguating = disambiguating.Substring(1);
                }

                //If the disambiguation is TWO characters - a full position, just parse it from there
                if (disambiguating.Length == 2)
                {
                    Position pos = PositionToolkit.Parse(disambiguating.ToUpper());
                    FromPosition = pos;
                }
                else //It is a single character (either a rank or file alone)
                {
                    //Is this a number (specifies the moving piece's rank) or is it a letter (specifies the moving piece's file)
                    if (char.IsDigit(disambiguating[0])) //If it is a number (rank)
                    {
                        int originating_rank = Convert.ToInt32(disambiguating[0].ToString());
                        foreach (Piece p in PotentialMovingPieces)
                        {
                            if (p.Position.Rank() == originating_rank)
                            {
                                FromPosition = p.Position;
                            }
                        }
                    }
                    else //If it is a letter (file)
                    {
                        foreach (Piece p in PotentialMovingPieces)
                        {
                            if (p.Position.File().ToString().ToLower() == disambiguating[0].ToString().ToLower())
                            {
                                FromPosition = p.Position;
                            }
                        }
                    }
                }
                
                
                
            }


        }

        //Handle eg. Ned4, R8a3, Qa2xb3
        private IEnumerable<Piece> FilterOutAmbiguous(IEnumerable<Piece> pieces, string move, PieceType pieceType)
        {
            var disambFrom = pieceType == PieceType.Pawn ? 0 : 1;
            var disambiguitatingSigns = move.Substring(disambFrom, move.Length - disambFrom - 2).ToArray();
            if (disambiguitatingSigns.Length == 0)
            {
                return pieces;
            }
            return pieces.Where(p => disambiguitatingSigns.All(ds =>
            {
                if (Char.IsDigit(ds))
                {
                    return p.Position.ToString()[1] == ds;
                }
                else
                {
                    return p.Position.ToString()[0] == Char.ToUpper(ds);
                }
            }));
        }

        //Returns the position of the last number in a string, not the number itself. If a rank number (1-8) is not found, returns null.
        private int? FindLastNumber(string inside)
        {
            int? ToReturn = null;
            for (int t = 1; t <= 8; t++)
            {
                int val = inside.LastIndexOf(t.ToString());
                if (val != -1)
                {
                    if (ToReturn == null)
                    {
                        ToReturn = val;
                    }
                    else
                    {
                        if (val > ToReturn.Value)
                        {
                            ToReturn = val;
                        }
                    }
                }
            }
            return ToReturn;
        }
    
        public override string ToString()
        {
            if (Castling.HasValue)
            {
                if (Castling.Value == CastlingType.KingSide)
                {
                    return "O-O";
                }
                else
                {
                    return "O-O-O";
                }
            }
            else
            {
                return FromPosition.ToString() + " --> " + ToPosition.ToString();
            }
        }

        public Position FindDestinationPosition(string algebraic_notation)
        {
            foreach (Position p in Enum.GetValues(typeof(Position)))
            {
                if (algebraic_notation.ToLower().Contains(p.ToString().ToLower()))
                {
                    return p;
                }
            }
            throw new Exception("Unable to find destinaton position (square to move to) in algebraic notation '" + algebraic_notation + "'");
        }

        #endregion
    }
}