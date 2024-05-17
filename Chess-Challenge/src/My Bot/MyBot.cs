using ChessChallenge.API;
using System;

public class MyBot : IChessBot
{
    bool playingAsWhite;
    Move chosenMove;
    public Move Think(Board board, Timer timer)
    {
        

        Move[] moves = board.GetLegalMoves();
        
        playingAsWhite = board.IsWhiteToMove;

        minMax(board, 3,int.MinValue,int.MaxValue,playingAsWhite);
        
        return chosenMove;
    }
    int Evaluate(Board board)
    {
        int matrial = 0;
        for (int i = 0 ; i < 64 ; i++)
        {
            Piece piece = board.GetPiece(new Square(i));
            int colorMultiplier=(piece.IsWhite) ? 1 : -1;
            if (piece.PieceType== PieceType.Pawn)
                matrial+=10*colorMultiplier;
            else if (piece.PieceType == PieceType.Knight)
                matrial += 30 * colorMultiplier;
            else if (piece.PieceType == PieceType.Bishop)
                matrial += 30 * colorMultiplier;
            else if (piece.PieceType == PieceType.Rook)
                matrial += 50 * colorMultiplier;
            else if (piece.PieceType == PieceType.Queen)
                matrial += 90 * colorMultiplier;
            else if (piece.PieceType == PieceType.King)
                matrial += 900 * colorMultiplier;
            if (piece.IsPawn)
            {
                if (piece.Square.Rank >= 3 && piece.Square.Rank <= 4 && piece.Square.File >= 3 && piece.Square.File <= 4)
                    matrial += 2 * colorMultiplier;

            }else if (piece.IsKnight)
            {
                if(piece.Square.Rank >= 2 && piece.Square.Rank <= 5 && piece.Square.File >= 2 && piece.Square.File <= 5)
                    matrial += 2 * colorMultiplier;
            }
        }

        return matrial;
    }

    int minMax(Board board, int depth,int alpha, int beta, bool isMaxmizing)
    {
        if(depth==0 || board.GetLegalMoves().Length == 0)
        {
            return Evaluate(board);
        }

        if (isMaxmizing)
        {
            int bestEvaluation = int.MinValue;
            Move bestMove = board.GetLegalMoves()[0];

            foreach (Move move in board.GetLegalMoves())
            {
                board.MakeMove(move);
                int evaluation = minMax(board, depth - 1,alpha, beta, false);
                board.UndoMove(move);
                if (evaluation > bestEvaluation) 
                {
                    bestEvaluation = evaluation;
                    bestMove = move;    
                }
                alpha = Math.Max(alpha, evaluation);
                if (beta <= alpha)
                    break;
            }

            chosenMove = bestMove;
            return bestEvaluation;
        }
        else if (!isMaxmizing)
        {
            int bestEvaluation = int.MaxValue;
            Move bestMove = board.GetLegalMoves()[0];

            foreach (Move move in board.GetLegalMoves())
            {
                board.MakeMove(move);
                int evaluation = minMax(board, depth - 1, alpha, beta, true);
                board.UndoMove(move);
                if (evaluation < bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    bestMove = move;
                }
                beta = Math.Min(beta, evaluation);
                if (beta <= alpha)
                    break;
            }
            chosenMove = bestMove;
            return bestEvaluation;
        }

        return Evaluate(board);
    }
}