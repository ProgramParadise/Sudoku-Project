using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Internal;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("'generate' or 'solve'");
            string s = Console.ReadLine();
            int[][,] puzzles = new int[][,]
            {   
                // Easy level sudoku
                new int[,]
                {
                    {0,0,0,0,1,0,0,0,0},
                    {0,0,6,4,8,5,0,0,3},
                    {0,0,8,0,0,0,4,0,6},
                    {0,1,0,0,4,8,0,2,0},
                    {2,0,0,7,3,0,5,0,4},
                    {0,7,0,0,6,0,0,1,0},
                    {0,0,0,0,0,0,0,0,0},
                    {4,0,0,0,0,0,2,7,8},
                    {8,0,0,0,0,0,0,0,0} 
                },

                // Mediumn level sudoku
                new int[,]
                {
                    {0,0,3,0,1,0,0,0,8},
                    {6,8,0,0,0,0,0,0,0},
                    {0,0,5,0,9,8,0,0,2},
                    {0,0,1,0,0,0,0,9,7},
                    {8,0,0,9,4,7,0,0,1},
                    {5,9,0,0,0,0,3,0,0},
                    {7,0,0,8,5,0,9,0,0},
                    {0,0,0,0,0,0,0,1,6},
                    {1,0,0,0,3,0,7,0,0} 
                },

                //Hard level sudoku
                new int[,]
                {
                    {0,0,9,1,0,0,0,0,0},
                    {0,4,0,0,5,0,0,8,0},
                    {3,0,5,0,0,7,0,0,0},
                    {0,0,0,0,0,8,0,4,0},
                    {6,2,0,0,0,0,0,1,5},
                    {0,1,0,3,0,0,0,0,0},
                    {0,0,0,4,0,0,1,0,8},
                    {0,8,0,0,9,0,0,3,0},
                    {0,0,0,0,0,3,7,0,0} 
                },

                // World's hardest sudoku
                new int[,]
                {
                    {8,0,0,0,0,0,0,0,0},
                    {0,0,3,6,0,0,0,0,0},
                    {0,7,0,0,9,0,2,0,0},
                    {0,5,0,0,0,7,0,0,0},
                    {0,0,0,0,4,5,7,0,0},
                    {0,0,0,1,0,0,0,3,0},
                    {0,0,1,0,0,0,0,6,8},
                    {0,0,8,5,0,0,0,1,0},
                    {0,9,0,0,0,0,4,0,0} 
                },

                // //User input sudoku (0 = spaces)
                // new int[,]
                // {
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0},
                //     {0,0,0,0,0,0,0,0,0} 
                // },
            };

            if (s.ToLowerInvariant() == "generate")
            {
                // for (int i = 1; i <= 9; i++)
                // {
                //     for (int j = 1; j <= 9; j++)
                //     {
                //         Console.Write(j + ",");
                //     }
                // }
                Console.WriteLine("input 'all at once' or 'one at a time'");
                //assign values
                List<List<string>> items = new List<List<string>>();

                if (Console.ReadLine().ToLowerInvariant() == "all at once")
                {
                    Console.WriteLine("Output the matrix with each digit separated by a comma.");
                    string num = Console.ReadLine();
                    List<string> tokens = new List<string>(num.Split(","));
                    for (int i = 0; i != 9; i++)
                    {
                        items.Add(tokens.GetRange(i * 9, 9));
                    }
                }
                if(Console.ReadLine().ToLowerInvariant() == "one at a time")
                {
                    List<string> tokens = new List<string>();
                    int width = 9;
                    int height = 9;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            string input;
                            int inputValue;
                            do
                            {
                                Console.WriteLine($"Input value for ({x},{y}): ");
                            }
                            while (!int.TryParse(input = Console.ReadLine(), out inputValue));
                            tokens.Add("" + inputValue);
                        }
                        items.Add(tokens.GetRange(x * 9, 9));
                    }
                }
                //write values
                Console.WriteLine("'code' or 'matrix'\n");
                Generator.Print2DList(items, Console.ReadLine());
            }

            if (s.ToLowerInvariant() == "solve")
            {
                StreamWriter writer = new StreamWriter("puzzles.txt");
                int idx = 1;
                foreach (int[,] puzzle in puzzles)
                {
                    writer.WriteLine("PUZZLE {0}: STEP-BY-STEP SOLVE", idx++);
                    writer.WriteLine("{----------------------------------------------------}");
                    SudokuSolver solver = new SudokuSolver(puzzle);
                    solver.writer = writer;
                    solver.Solve();
                    writer.WriteLine("{----------------------------------------------------}");
                    writer.WriteLine("+OK  SOLVED -> {0} steps", solver.steps);
                    solver.WritePuzzle(writer);
                    writer.Flush();
                    writer.WriteLine();
                    writer.WriteLine();
                    Console.WriteLine("\nPUZZLE {0} SOLVED\n", idx - 1);
                }
                Console.Write("See puzzles.txt for the solutions.\n");

                // if you uncomment this, you may want to comment the step output
                // in SolveRecurse. If you don't your file will be extremely large
                // ~20MB
                // TestSolver(new StreamWriter("userInput_solved.txt"));
            }
        }
        private static void TestSolver(StreamWriter writer)
        {
            Console.WriteLine("\nSOLVING USER INPUT\n");

            SudokuSolver solver;
            List<int[,]> puzzles;

            string[] lines = File.ReadLines("userInput.txt").Skip(4).ToArray();

            puzzles = new List<int[,]>();
            foreach (string line in lines)
            {
                int[,] puzzle = new int[9, 9];
                int i = 0;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        char c = line[i++];
                        if (c == '.')
                            puzzle[row, col] = 0;
                        else
                            puzzle[row, col] = (int)(c - '0');
                    }
                }
                puzzles.Add(puzzle);
            }

            writer.AutoFlush = false;

            int idx = 1;
            long max = 0;
            double avg = 0.0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1; i++)
            {
                foreach (int[,] puzzle in puzzles)
                {
                    long start = watch.ElapsedMilliseconds;
                    solver = new SudokuSolver(puzzle);
                    solver.writer = writer;
                    Console.WriteLine("Solving Puzzle {0}", idx);
                    writer.WriteLine("Solving Puzzle {0}", idx++);
                    if (solver.Solve())
                    {
                        long end = watch.ElapsedMilliseconds;
                        Console.WriteLine(" +OK Total: {0}ms (+{1}ms) -> {2} steps", end, end - start, solver.steps);
                        writer.WriteLine(" +OK Total: {0}ms (+{1}ms) -> {2} steps", end, end - start, solver.steps);
                        writer.WriteLine();
                        solver.WritePuzzle(writer);
                        writer.WriteLine();
                        max = ((end - start) > max) ? (end - start) : max;
                        avg = end;
                    }
                    else
                        writer.WriteLine(" +FAIL");
                }
            }

            writer.WriteLine("Max: {0}ms", max);
            writer.WriteLine("Avg: {0}ms", avg / 95);

            writer.Flush();
            writer.Close();
            Console.WriteLine("\nSee userInput_solved.txt for the solutions.");
        }
    }
    
    class Generator
    {
        
        public static void Print2DList<T>(List<List<T>> matrix, string s)
        {
            int h = matrix.Count;
            int w = matrix.Max(l => l.Count);

            if (s.ToLowerInvariant() == "code")
            {
                Console.WriteLine("new int[,]\n{ ");
                for (int i = 0; i < h; i++)
                {
                    Console.Write("\t{");
                    for (int j = 0; j < w; j++)
                    {
                        if (j < matrix[i].Count)
                            Console.Write(matrix[i][j]);
                        if (j < (w - 1) && i < h) Console.Write(",");
                    }
                    if (i != h - 1) Console.Write("},");
                    else Console.Write("} \n}//,");
                    Console.WriteLine();
                }
            }
            if (s.ToLowerInvariant() == "matrix")
            {
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (j < matrix[i].Count)
                        {
                            Console.Write(matrix[i][j]);
                            Console.Write(" ");
                        }   
                    }
                    Console.WriteLine();
                }
            }
        }
    }
    class SudokuSolver
    {
        // Convenience class for tracking candidates
        class Candidate : IEnumerable
        {
            bool[] m_values;
            int m_count;
            int m_numCandidates;

            public int Count { get { return m_count; } }

            public Candidate(int numCandidates, bool initialValue)
            {
                m_values = new bool[numCandidates];
                m_count = 0;
                m_numCandidates = numCandidates;

                for (int i = 1; i <= numCandidates; i++)
                    this[i] = initialValue;
            }

            public bool this[int key]
            {
                // Allows candidates to be referenced by actual value (i.e. 1-9, rather than 0 - 8)
                get { return m_values[key - 1]; }

                // Automatically tracks the number of candidates
                set
                {
                    m_count += (m_values[key - 1] == value) ? 0 : (value == true) ? 1 : -1;
                    m_values[key - 1] = value;
                }
            }

            public void SetAll(bool value)
            {
                for (int i = 1; i <= m_numCandidates; i++)
                    this[i] = value;
            }

            public override string ToString()
            {
                StringBuilder values = new StringBuilder();
                foreach (int candidate in this)
                    values.Append(candidate);
                return values.ToString();
            }

            public IEnumerator GetEnumerator()
            {
                return new CandidateEnumerator(this);
            }

            // Enumerator simplifies iterating over candidates
            private class CandidateEnumerator : IEnumerator
            {
                private int m_position;
                private Candidate m_c;

                public CandidateEnumerator(Candidate c)
                {
                    m_c = c;
                    m_position = 0;
                }

                // only iterates over valid candidates
                public bool MoveNext()
                {
                    ++m_position;
                    if (m_position <= m_c.m_numCandidates)
                    {
                        if (m_c[m_position] == true)
                            return true;
                        else
                            return MoveNext();
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    m_position = 0;
                }

                public object Current
                {
                    get { return m_position; }
                }
            }
        }



        // True values for row, grid, and region constraint matrices
        // mean that they contain that candidate, inversely,
        // True values in the candidate constraint matrix means that it
        // is a possible value for that cell.
        Candidate[,] m_cellConstraintMatrix;
        Candidate[] m_rowConstraintMatrix;
        Candidate[] m_colConstraintMatrix;
        Candidate[,] m_regionConstraintMatrix;

        // Actual puzzle grid (uses 0s for unsolved squares)
        int[,] m_grid;

        // Another convenience structure. Easy and expressive way
        // of passing around row, column information.
        struct Cell
        {
            public int row, col;
            public Cell(int r, int c) { row = r; col = c; }
        }

        // helps avoid iterating over solved squares
        HashSet<Cell> solved;
        HashSet<Cell> unsolved;

        // Tracks the cells changed due to propagation (i.e. the rippled cells)
        Stack<HashSet<Cell>> changed;

        // keeps cell counts by keeping them in buckets
        // this allows the cell with the least candidates
        // (minimum count) to be selected in O(1)
        HashSet<Cell>[] bucketList;

        // Tracks the number of steps a solution takes
        public int steps;

        public StreamWriter writer;

        private void InitializeMatrices()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    // if the square is solved update the candidate list
                    // for the row, column, and region
                    if (m_grid[row, col] > 0)
                    {
                        int candidate = m_grid[row, col];
                        m_rowConstraintMatrix[row][candidate] = true;
                        m_colConstraintMatrix[col][candidate] = true;
                        m_regionConstraintMatrix[row / 3, col / 3][candidate] = true;
                    }
                }
            }
        }

        private void PopulateCandidates()
        {
            //Add possible candidates by checking
            //the rows, columns and grid
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    //if solved, then there are no possible candidates
                    if (m_grid[row, col] > 0)
                    {
                        m_cellConstraintMatrix[row, col].SetAll(false);
                        solved.Add(new Cell(row, col));
                    }
                    else
                    {
                        // populate each cell with possible candidates
                        // by checking the row, col, and grid associated 
                        // with that cell
                        foreach (int candidate in m_rowConstraintMatrix[row])
                            m_cellConstraintMatrix[row, col][candidate] = false;
                        foreach (int candidate in m_colConstraintMatrix[col])
                            m_cellConstraintMatrix[row, col][candidate] = false;
                        foreach (int candidate in m_regionConstraintMatrix[row / 3, col / 3])
                            m_cellConstraintMatrix[row, col][candidate] = false;

                        Cell c = new Cell(row, col);
                        bucketList[m_cellConstraintMatrix[row, col].Count].Add(c);
                        unsolved.Add(c);
                    }
                }
            }
        }

        private Cell NextCell()
        {
            if (unsolved.Count == 0)
                return new Cell(-1, -1);    // easy way to signal a solved puzzle

            for (int i = 0; i < bucketList.Length; i++)
                if (bucketList[i].Count > 0)
                    return bucketList[i].First();

            // should never execute
            return new Cell(99, 99);

        }

        // Backtracking method. Undoes the specified selection
        private void UnselectCandidate(Cell aCell, int candidate)
        {
            // 1) Remove selected candidate from grid
            m_grid[aCell.row, aCell.col] = 0;

            // 2) Add that candidate back to the cell constraint matrix.
            //    Since it wasn't selected, it can still be selected in the 
            //    future
            m_cellConstraintMatrix[aCell.row, aCell.col][candidate] = true;

            // 3) Put cell back in the bucket list
            bucketList[m_cellConstraintMatrix[aCell.row, aCell.col].Count].Add(aCell);

            // 4) Remove the candidate from the row, col, and region constraint matrices
            m_rowConstraintMatrix[aCell.row][candidate] = false;
            m_colConstraintMatrix[aCell.col][candidate] = false;
            m_regionConstraintMatrix[aCell.row / 3, aCell.col / 3][candidate] = false;

            // 5) Add the candidate back to any cells that changed from
            //    its selection (propagation).
            foreach (Cell c in changed.Pop())
            {
                // shift affected cells up the bucket list
                bucketList[m_cellConstraintMatrix[c.row, c.col].Count].Remove(c);
                bucketList[m_cellConstraintMatrix[c.row, c.col].Count + 1].Add(c);
                m_cellConstraintMatrix[c.row, c.col][candidate] = true;
            }

            // 6) Add the cell back to the list of unsolved
            solved.Remove(aCell);
            unsolved.Add(aCell);
        }

        private void SelectCandidate(Cell aCell, int candidate)
        {
            HashSet<Cell> changedCells = new HashSet<Cell>();

            // place candidate on grid
            m_grid[aCell.row, aCell.col] = candidate;

            // remove from bucket list
            bucketList[m_cellConstraintMatrix[aCell.row, aCell.col].Count].Remove(aCell);

            // remove candidate from cell constraint matrix
            m_cellConstraintMatrix[aCell.row, aCell.col][candidate] = false;

            // add the candidate to the cell, row, col, region constraint matrices
            m_colConstraintMatrix[aCell.col][candidate] = true;
            m_rowConstraintMatrix[aCell.row][candidate] = true;
            m_regionConstraintMatrix[aCell.row / 3, aCell.col / 3][candidate] = true;

            /**** RIPPLE ACROSS COL, ROW, REGION ****/

            // (propagation)
            // remove candidates across unsolved cells in the same
            // row and col.
            for (int i = 0; i < 9; i++)
            {
                // only change unsolved cells containing the candidate
                if (m_grid[aCell.row, i] == 0)
                {
                    if (m_cellConstraintMatrix[aCell.row, i][candidate] == true)
                    {
                        // shift affected cells down the bucket list
                        bucketList[m_cellConstraintMatrix[aCell.row, i].Count].Remove(new Cell(aCell.row, i));
                        bucketList[m_cellConstraintMatrix[aCell.row, i].Count - 1].Add(new Cell(aCell.row, i));

                        // remove the candidate
                        m_cellConstraintMatrix[aCell.row, i][candidate] = false;

                        //update changed cells (for backtracking)
                        changedCells.Add(new Cell(aCell.row, i));
                    }
                }
                // only change unsolved cells containing the candidate
                if (m_grid[i, aCell.col] == 0)
                {
                    if (m_cellConstraintMatrix[i, aCell.col][candidate] == true)
                    {
                        // shift affected cells down the bucket list
                        bucketList[m_cellConstraintMatrix[i, aCell.col].Count].Remove(new Cell(i, aCell.col));
                        bucketList[m_cellConstraintMatrix[i, aCell.col].Count - 1].Add(new Cell(i, aCell.col));

                        // remove the candidate
                        m_cellConstraintMatrix[i, aCell.col][candidate] = false;

                        //update changed cells (for backtracking)
                        changedCells.Add(new Cell(i, aCell.col));
                    }
                }
            }

            // (propagation)
            // remove candidates across unsolved cells in the same
            // region.
            int grid_row_start = aCell.row / 3 * 3;
            int grid_col_start = aCell.col / 3 * 3;
            for (int row = grid_row_start; row < grid_row_start + 3; row++)
                for (int col = grid_col_start; col < grid_col_start + 3; col++)
                    // only change unsolved cells containing the candidate
                    if (m_grid[row, col] == 0)
                    {
                        if (m_cellConstraintMatrix[row, col][candidate] == true)
                        {
                            // shift affected cells down the bucket list
                            bucketList[m_cellConstraintMatrix[row, col].Count].Remove(new Cell(row, col));
                            bucketList[m_cellConstraintMatrix[row, col].Count - 1].Add(new Cell(row, col));

                            // remove the candidate
                            m_cellConstraintMatrix[row, col][candidate] = false;

                            //update changed cells (for backtracking)
                            changedCells.Add(new Cell(row, col));
                        }
                    }

            // add cell to solved list
            unsolved.Remove(aCell);
            solved.Add(aCell);
            changed.Push(changedCells);
        }

        private bool SolveRecurse(Cell nextCell)
        {
            // Our base case: No more unsolved cells to select, 
            // thus puzzle solved
            if (nextCell.row == -1)
                return true;

            // Loop through all candidates in the cell
            foreach (int candidate in m_cellConstraintMatrix[nextCell.row, nextCell.col])
            {
                writer.WriteLine("{4} -> ({0}, {1}) : {2} ({3})", nextCell.row, nextCell.col,
                    m_cellConstraintMatrix[nextCell.row, nextCell.col], m_cellConstraintMatrix[nextCell.row, nextCell.col].Count, steps++);

                SelectCandidate(nextCell, candidate);

                // Move to the next cell.
                // if it returns false backtrack
                if (SolveRecurse(NextCell()) == false)
                {
                    ++steps;
                    writer.WriteLine("{0} -> BACK", steps);
                    UnselectCandidate(nextCell, candidate);
                    continue;
                }
                else // if we recieve true here this means the puzzle was solved earlier
                    return true; 
            }

            // return false if path is unsolvable
            return false;

        }

        public bool Solve()
        {
            steps = 1;
            return SolveRecurse(NextCell());
        }

        public void PrintCandidates()
        {
            for (int row = 0; row < 9; row++)
            {
                Console.Write("[{0}]: ", row);
                for (int col = 0; col < 9; col++)
                {
                    Console.Write("{0,-9}", m_cellConstraintMatrix[row, col]);
                }
                Console.WriteLine();
            }
        }

        public void WritePuzzle(StreamWriter writer)
        {
            writer.Write(" ");
            for (int r = 0; r < 9; r++)
            {
                if ((r % 3) == 0 && r != 0)
                    writer.Write("- - - + - - - + - - -\r\n ");
                for (int c = 0; c < 9; c++)
                {
                    if ((c % 3) == 0 && c != 0)
                        writer.Write("| ");
                    writer.Write(m_grid[r, c] + " ");
                }
                writer.Write("\r\n ");
            }
        }

        public SudokuSolver(int[,] initialGrid)
        {
            m_grid = new int[9, 9];
            m_cellConstraintMatrix = new Candidate[9, 9];
            m_rowConstraintMatrix = new Candidate[9];
            m_colConstraintMatrix = new Candidate[9];
            m_regionConstraintMatrix = new Candidate[9, 9];
            solved = new HashSet<Cell>();
            unsolved = new HashSet<Cell>();
            changed = new Stack<HashSet<Cell>>();
            bucketList = new HashSet<Cell>[10];
            steps = 0;

            // initialize constraints

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    // copy grid, and turn on all Candidates for every cell
                    m_grid[row, col] = initialGrid[row, col];
                    m_cellConstraintMatrix[row, col] = new Candidate(9, true);
                }
            }

            for (int i = 0; i < 9; i++)
            {
                m_rowConstraintMatrix[i] = new Candidate(9, false);
                m_colConstraintMatrix[i] = new Candidate(9, false);
                bucketList[i] = new HashSet<Cell>();
            }
            bucketList[9] = new HashSet<Cell>();

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    m_regionConstraintMatrix[row, col] = new Candidate(9, false);

            InitializeMatrices();
            PopulateCandidates();
        }
    }
}