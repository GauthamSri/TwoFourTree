using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TwoFourTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose Operation");
            Console.WriteLine("1. Manual Operations");
            Console.WriteLine("2.Automated Performance Evaluation");
            Console.WriteLine("Enter your choice");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
                manual();
            else if (choice == 2)
                automation();
            else
                Console.WriteLine("Enter the damn correct option");
            Console.Read();
        }

        public static void automation()
        {
            TwoFourTree tree = new TwoFourTree();

            Console.WriteLine("Enter the input size");
            int size = Convert.ToInt32(Console.ReadLine());

            int[] input = new int[size];

            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {

                int no = rnd.Next(0, size);
                input[i] = no;
            }

            foreach (var t in input)
            {
                tree.InsertValue(t);
            }

            int[] operationsInput = new int[size * 2];

            double insertFactor = 0.4, deleteFactor = 0.25, searchFactor = 0.35;
            int insertCount = (int)((size * 2) * insertFactor);
            int deleteCount = (int)((size * 2) * deleteFactor);
            int searchCount = (int)((size * 2) * searchFactor);

            Random rnd2 = new Random();

            for (int i = 0; i < (size * 2); i++)
            {

                int no = rnd2.Next(0, (size * 2));
                operationsInput[i] = no;
            }

            int[] insertElem = new int[insertCount];
            int[] deleteElem = new int[deleteCount];
            int[] searchElem = new int[searchCount];


            Array.Copy(operationsInput, insertElem, insertCount);
            Array.Copy(operationsInput, deleteElem, deleteCount);
            Array.Copy(operationsInput, searchElem, searchCount);

            // int[] input2 = new int[] {2,12,19,16,15,18,10,25,30,22,35,24,27,21,20};
            //int[] input3 = new int[] {18,9,35,16,24,13,32,27,6,27,22,22,27,9,23,13};


            //foreach (var t in input3)
            //{
            //    tree.InsertValue(t);
            //}

            //Original Prog start
            //Console.WriteLine("Input for Tree");
            //foreach (var t in insertElem)
            //{
            //    Console.Write(t + " ");
            //}

            //Console.WriteLine("------------------------------------");

            Console.WriteLine(" Inserting {0} elements", insertCount);
            int totalSplitCount = 0;
            Stopwatch stopwatchInsertion = Stopwatch.StartNew();
            foreach (var t in insertElem)
            {
                int spCount = tree.InsertValue(t);
                totalSplitCount = totalSplitCount + spCount;
            }

            stopwatchInsertion.Stop();
            var insertionExecTime = stopwatchInsertion.ElapsedMilliseconds;

            tree.InOrderTraversal();
            Console.WriteLine("");

            //Console.WriteLine("Input for Deletion");
            //foreach (var t in deleteElem)
            //{
            //    Console.Write(t + " ");
            //}

            //Console.WriteLine("------------------------------------");


            Console.WriteLine(" Deleting {0} elements", deleteCount);
            int totalFusionCount = 0;
            Stopwatch stopwatchDeletion = Stopwatch.StartNew();
            foreach (var t in deleteElem)
            {
                //Console.WriteLine("Deleting {0} from the tree", t);
                int fusCount = tree.deleteKey(t);
                totalFusionCount = totalFusionCount + fusCount;
            }
            stopwatchDeletion.Stop();
            var deletionExecTime = stopwatchDeletion.ElapsedMilliseconds;

            tree.InOrderTraversal();
            Console.WriteLine("");

            Console.WriteLine(" Searching {0} elements", searchCount);
            Stopwatch stopwatchSearch = Stopwatch.StartNew();
            foreach (var t in searchElem)
            {
                tree.searchTree(t);
            }
            stopwatchSearch.Stop();
            var searchExecTime = stopwatchSearch.ElapsedMilliseconds;

            tree.InOrderTraversal();
            Console.WriteLine("");
            Console.WriteLine("Execution Time for Insertion Operation ( {0} elements) - {1}", insertCount, insertionExecTime);
            Console.WriteLine("Execution Time for Deletion Operation ( {0} elements) - {1}", deleteCount, deletionExecTime);
            Console.WriteLine("Execution Time for Search Operation ( {0} elements) - {1}", searchCount, searchExecTime);
            Console.WriteLine("Total number of Split Operations - {0}", totalSplitCount);
            Console.WriteLine("Total number of Fusion Operations - {0}", totalFusionCount);

            //Original Prog ENd

            //int counter = 100000000;
            //while (counter != 0)
            //{
            //    Console.WriteLine("Enter the key to delete");
            //    int deleteKey = Convert.ToInt32(Console.ReadLine());

            //    tree.deleteKey(deleteKey);

            //    counter = deleteKey;
            //    tree.InOrderTraversal();
            //}
        }

        public static void manual()
        {
            TwoFourTree tree = new TwoFourTree();

            int option = 0;
            do
            {
            Console.WriteLine("Choose an Option");
            Console.WriteLine("1. Insert Key");
            Console.WriteLine("2. Delete Key");
            Console.WriteLine("3. Search");
            Console.WriteLine("4. Exit");
            option = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Key to be manipulated");
            int key = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1: Console.WriteLine("Insertion Operation");
                    tree.InsertValue(key);
                    Console.WriteLine("InOrder of tree after inserting");
                    tree.InOrderTraversal();
                    Console.WriteLine("    ");
                    break;
                case 2:
                     Console.WriteLine("Deletion Operation");
                    tree.deleteKey(key);
                    Console.WriteLine("InOrder of tree after Deletion");
                    tree.InOrderTraversal();
                    Console.WriteLine("    ");
                    break;
                case 3: Console.WriteLine("Search Operation");
                    tree.searchTree(key);
                    Console.WriteLine("    ");
                    break;
                default:
                    break;
            }

            }while(option!= 4);
        }
    }
}
