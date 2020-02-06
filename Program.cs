using System;
using System.Collections.Generic;

namespace chess
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = (int.Parse(args[0]), int.Parse(args[1]));
            var finish = (int.Parse(args[2]), int.Parse(args[3]));

            var result = new KnightMovesChecker().GetShortestPath(start, finish);
            foreach(var step in result)
            {
                Console.WriteLine(step.Item1 + ", " + step.Item2);
            }

            Console.ReadLine();
        }
    }

    public class KnightMovesChecker
    {
        private KnightMovesPlanner planner = new KnightMovesPlanner();
        public IEnumerable<(int,int)> GetShortestPath((int,int) start, (int,int) finish)
        {
            var currentPosition = new Position { A = start.Item1, B = start.Item2 };
            var queue = new Queue<Position>();
            while (currentPosition.A != finish.Item1 || currentPosition.B != finish.Item2)
            {
                foreach(var pos in planner.GetNextTurns(currentPosition))
                {
                    queue.Enqueue(pos);
                }
                currentPosition = queue.Dequeue();
            }
            var result = new Stack<(int,int)>();
            while(currentPosition!=null)
            {
                result.Push((currentPosition.A, currentPosition.B));
                currentPosition = currentPosition.Previous;
            }
            return result;
        }
    }

    public class Position
    {
        public int A;
        public int B;
        public Position Previous;
    }

    public class KnightMovesPlanner
    {
        private (int,int)[] offsets = new (int,int)[]
        {
            (-2,-1),
            (-2,1),
            (-1,2),
            (-1,-2),
            (1,2),
            (1,-2),
            (2,1),
            (2,-1)
        };
        public IEnumerable<Position> GetNextTurns(Position current)
        {
            foreach(var offset in offsets)
            {
                var result = new Position { A = current.A + offset.Item1, B = current.B + offset.Item2, Previous = current };
                if (result.A > -1 && result.A < 8
                && result.B > -1 && result.B < 8
                && !IsPrevious(current, result))
                    yield return result;
            }
        }

        private static bool IsPrevious(Position current, Position result)
        {
            return current.Previous != null
            && result.A == current.Previous.A 
            && result.B == current.Previous.B;
        }
    }
}
