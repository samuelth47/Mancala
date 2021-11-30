using System;
using System.Diagnostics;

namespace Mankalah
{
    // rename me
    public class sth5Player : Player // class must be public
    {
        private Position us;

        public sth5Player(Position pos, int maxTimePerMove)
            : base(pos, "Sam", maxTimePerMove)
        {
            us = pos;
        }

        //return the string if I win
        public override string gloat()
        {
            return "I WIN, YOU LOSE!";
        }

        public override String getImage()
        {
            return "pic.jpg";
        }

        //Returns a move that it chose through the minimaxVal function
        public override int chooseMove(Board b)
        {
            //Create a new instance of a stopwatch and start it
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int i = 1;
            moveResult move = new moveResult(0, 0);

            while (watch.ElapsedMilliseconds < getTimePerMove())
            {
                move = minimaxVal(b, i++, Int32.MinValue, Int32.MaxValue);
            }

            return move.GetMove();
        }

        //A minmax function that return the best move and its score
        private moveResult minimaxVal(Board b, int d, int alpha, int beta)
        {
            int best_move = 0;
            int best_score = 0;

            if (b.gameOver() || d == 0)
            {
                return new moveResult(0, evaluate(b));
            }

            //if the Top is max
            if (b.whoseMove() == Position.Top)
            {
                best_score = Int32.MinValue;

                for (int move = 7; move <= 12; move++)
                {
                    if (b.legalMove(move))
                    {
                        //duplicate the board
                        Board b1 = new Board(b);

                        //make the move
                        b1.makeMove(move, false);

                        //Find the value of the move
                        moveResult score = minimaxVal(b1, d - 1, alpha, beta);

                        //remember the value if it the best value
                        if (score.GetScore() > best_score)
                        {
                            best_score = score.GetScore();
                            best_move = move;
                        }
                        if (best_score > alpha)
                            alpha = best_score;
                    }
                }

                return new moveResult(best_move, best_score);
            }

            //if the Bottom is max
            else
            {
                best_score = int.MaxValue;

                for (int move = 0; move <= 5; move++)
                {
                    if (b.legalMove(move))
                    {

                        Board b1 = new Board(b);
                        b1.makeMove(move, false);
                        moveResult score = minimaxVal(b1, d - 1, alpha, beta);
                        if (score.GetScore() < best_score)
                        {
                            best_score = score.GetScore();
                            best_move = move;
                        }
                        if (best_score < beta)
                            beta = best_score;
                    }
                }
            }

            return new moveResult(best_move, best_score);
        }

        //Evaluate fuction that checks the stones, captures and go agains we have
        public override int evaluate(Board b)
        {

            int stones = 0;
            int captures = 0;
            int goAgains = 0;




            //If the position is at the Top
            if (b.whoseMove() == Position.Top)
            {
                for (int i = 7; i <= 12; i++)
                {
                    //Add all the stones at the Top
                    stones += b.stonesAt(i);

                    //Add the go agains you have
                    if (b.stonesAt(i) - (13 - i) == 0)
                    {
                        goAgains += 1;
                    }


                    int capture_target = i + b.stonesAt(i);
                    if (capture_target < 13)
                    {
                        //Get the the number of stones at the capture_target
                        int total_stones = b.stonesAt(capture_target);
                        if (b.whoseMove() == Position.Top && total_stones == 0 && b.stonesAt(13 - capture_target - 1) != 0)
                        {
                            captures += b.stonesAt(13 - capture_target - 1);
                        }
                    }

                }
            }

            //If the position is at the Bottom
            else
            {
                for (int i = 0; i <= 5; i++)
                {
                    //Subtract all the stones at the bottom
                    stones -= b.stonesAt(i);

                    if (b.stonesAt(i) - (6 - i) == 0)
                    {
                        //Subtract the go agains
                        goAgains -= 1;
                    }

                    int capture_target = i + b.stonesAt(i);

                    if (capture_target < 6)
                    {
                        int total_stones = b.stonesAt(capture_target);
                        if (b.whoseMove() == Position.Bottom && total_stones == 0 && b.stonesAt(13 - capture_target - 1) != 0)
                        {
                            captures -= b.stonesAt(13 - capture_target - 1);
                        }
                    }
                }
            }


            if (b.whoseMove() != Position.Top)
            {
                stones = -1 * stones;
                captures = -1 * captures;
                goAgains = -1 * goAgains;
            }


            int score = b.stonesAt(13) - b.stonesAt(6);
            score += stones + captures + goAgains;

            return score;

        }

        //A class to store move results best move and score with additional methods
        class moveResult
        {
            private int bestMove;
            private int bestScore;

            //moveResult function that includes the move and score
            public moveResult(int move, int score)
            {
                bestMove = move;
                bestScore = score;

            }

            //This function sets the score
            public void SetScore(int score)
            {
                bestScore = score;
            }

            //This function sets the move
            public void SetMove(int move)
            {
                bestMove = move;
            }

            //This function returns the score
            public int GetScore()
            {
                return bestScore;
            }

            //This function return the move
            public int GetMove()
            {
                return bestMove;
            }


        }
    }
}