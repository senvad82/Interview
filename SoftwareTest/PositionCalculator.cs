// This test should take around 45 minutes to 1 hour. Read all of the instructions before you being. It will help you plan ahead.

/* TODO: Implement a net positions calculator
 * This NetPositionCalculator should read in test_data.csv
 * and output a file in the format of net_positions_expected.csv
 * 
 * Here is a sample of net positions:
 * TRADER   BROKER  SYMBOL  QUANTITY    PRICE
 * Joe      ML      IBM.N     100         50
 * Joe      DB      IBM.N    -50          50
 * Joe      CS      IBM.N     30          30
 * Mike     CS      AAPL.N    100         20
 * Mike     BC      AAPL.N    200         20
 * Debby    BC      NVDA.N    500         20
 * 
 * Expected Output:
 * TRADER   SYMBOL  QUANTITY
 * Joe      IBM.N     80
 * Mike     AAPL.N    300
 * Debby    NVDA.N    500
 */

/* TODO: Implement a boxed position calculator
 * This BoxedPositionCalculator should read in test_data.csv
 * and output a file the format of boxed_positions_expected.csv
 * 
 * Boxed positions are defined as:
 * A trader has long (quantity > 0) and short (quantity < 0) positions for the same symbol at different brokers.
 * 
 * This is an example of a boxed position:
 * TRADER   BROKER  SYMBOL  QUANTITY    PRICE
 * Joe      ML      IBM.N     100         50      <------Has at least one positive quantity for Trader = Joe and Symbol = IBM
 * Joe      DB      IBM.N    -50          50      <------Has at least one negative quantity for Trader = Joe and Symbol = IBM
 * Joe      CS      IBM.N     30          30
 * 
 * Expected Output:
 * TRADER   SYMBOL  QUANTITY
 * Joe      IBM.N     50        <------Show the minimum quantity of all long positions or the absolute sum of all short positions. ie. minimum of (100 + 30) and abs(-50) is 50
 * 
 * This is NOT a boxed position. Since no trader has both long and short positions at different brokers.
 * TRADER   BROKER  SYMBOL  QUANTITY    PRICE
 * Joe      ML      IBM.N     100         50
 * Joe      DB      IBM.N     50          50
 * Joe      CS      IBM.N     30          30
 * Mike     DB      IBM.N    -50          50
 * 
 */

/* TODO: Write tests to ensure your code works
 * Feel free to write as many or as few tests as you feel necessary to ensure that your
 * code is correct and stable.
 */

/*
 * How we review this test:
 * We look for clean, readable code, that is well designed and solves the problem.
 * As for testing, we simply look for completeness.
 * 
 * Some assumptions you can make when implementing:
 * 1) The file is always valid, you do not need to validate the file in any way
 * 2) You may write all classes in this one file
 */


namespace mlp.interviews.boxing.problem
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;


    public class Position
    {
        public string trader { get; set; }
        public string broker { get; set; }
        public string symbol { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }

    public static class PositionTranslator
    {
        public static Position TranslatePosition(string csvLine)
        {
            Position pos = new Position();
            string[] values = csvLine.Split(',');
            pos.trader = values[0];
            pos.broker = values[1];
            pos.symbol = values[2];
            pos.quantity = Convert.ToInt32(values[3]);
            pos.price = Convert.ToDouble(values[4]);

            return pos;
        }

    }
    public interface IPositionDataService
    {
        string InputFilePath { get; set; }
        string OutputFilePath { get; set; }
        IList<Position> Import();
        bool Export(IList<Position> input);
    }
    public class CSVPositionDataService : IPositionDataService
    {
        public string InputFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public CSVPositionDataService(string inputFilePath, string outputFilePath)
        {
            this.InputFilePath = inputFilePath;
            this.OutputFilePath = outputFilePath;
        }
        public IList<Position> Import()
        {
            return File.ReadAllLines(this.InputFilePath)
                                                       .Skip(1)
                                                       .Select(p => PositionTranslator.TranslatePosition(p))
                                                       .ToList();
        }

        public bool Export(IList<Position> positions)
        {
            try
            {
                using (var writer = new StreamWriter(this.OutputFilePath))
                {
                    writer.WriteLine(string.Join(",", "TRADER", "SYMBOL", "QUANTITY"));

                    foreach (var item in positions)
                    {
                        writer.WriteLine(string.Join(",", item.trader, item.symbol, item.quantity));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


        }
    }
    public interface IPositionCalculator
    {
        IList<Position> PositionList { get; set; }
        List<Position> Calculate();

        bool ProcessFile();


    }
    public class BoxPositionCalculator : IPositionCalculator
    {
        public IList<Position> PositionList { get; set; }

        private IPositionDataService _positionDataService;
        public BoxPositionCalculator(IPositionDataService dataService)
        {
            _positionDataService = dataService;
        }
        public bool ProcessFile()
        {
            try
            {
                PositionList = _positionDataService.Import();
                var boxPositions = Calculate();
                _positionDataService.Export(boxPositions);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

        public List<Position> Calculate()
        {
            return PositionList.Where(p1 => PositionList.Any(p2 => p2.trader == p1.trader && p2.symbol == p1.symbol && p2.broker != p1.broker && ((p2.quantity < 0 && p1.quantity > 0) || (p2.quantity > 0 && p1.quantity < 0))) && p1.quantity < 0).GroupBy(p3 => new { p3.trader, p3.symbol }).Select(cl => new Position
            {
                trader = cl.First().trader,
                symbol = cl.First().symbol,
                quantity = cl.Sum(c => Math.Abs(c.quantity)),
            }).ToList();

        }
    }

    public class NetPositionCalculator: IPositionCalculator
    {
        public IList<Position> PositionList { get; set; }

        private IPositionDataService _positionDataService;

        public NetPositionCalculator(IPositionDataService dataService)
        {
            _positionDataService = dataService;

        }

        public List<Position> Calculate()
        {
            return PositionList.GroupBy(p => new { p.trader, p.symbol }).Select(cl => new Position
            {
                trader = cl.First().trader,
                symbol = cl.First().symbol,
                quantity = cl.Sum(c => c.quantity),
            }).ToList();

        }
        public bool ProcessFile()
        {
            try
            {
                PositionList = _positionDataService.Import();
                var netPositions = Calculate();
                _positionDataService.Export(netPositions);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

       

    }
    public class PositionCalculatorTest{
        public PositionCalculatorTest()
        {

        }
        public bool RunTestCases()
        {
            bool retResult = true;
            IPositionDataService dataService = new CSVPositionDataService(null, null);
            IPositionCalculator netPosCalc = new NetPositionCalculator(dataService);
            IPositionCalculator boxPosCalc = new BoxPositionCalculator(dataService);
            var positionList = new List<Position>();
            positionList.Add(new Position() { trader = "Joe", broker = "ML", symbol = "IBM", quantity = 50, price = 100.23 });
            positionList.Add(new Position() { trader = "Joe", broker = "ML", symbol = "IBM", quantity = 100, price = 100.23 });
            positionList.Add(new Position() { trader = "Joe", broker = "ML", symbol = "IBM", quantity = -50, price = 100.23 });
            positionList.Add(new Position() { trader = "Joe", broker = "ML", symbol = "IBM", quantity = -20, price = 100.23 });
            positionList.Add(new Position() { trader = "Joe", broker = "ML", symbol = "IBM", quantity = -10, price = 100.23 });
            positionList.Add(new Position() { trader = "Joe", broker = "MLB", symbol = "IBM", quantity = -10, price = 100.23 });
            netPosCalc.PositionList = positionList;
            var result = netPosCalc.Calculate();
            boxPosCalc.PositionList = positionList;
            var result2 = boxPosCalc.Calculate();
            if (result.Count == 1 && result[0].quantity == 60)
                Console.WriteLine("Net Position Validated");
            else { retResult = false; Console.WriteLine("Net Position validation error"); }
                
            if (result2.Count == 1 && result2[0].quantity == 10)
                Console.WriteLine("Box Position Validated");
            else { retResult = false; Console.WriteLine("Box Position Validation error"); }

            return retResult;

        }
    }
    
    public class PositionCalculatorClient
    {
        //public static void Main(string[] args)
        //{
        //    //Test cases usually set it up using nunit, here i just added below for simplocity
        //    //
        //    var test = new PositionCalculatorTest();
        //    if (!test.RunTestCases()) return;

        //    // Only below code should be here and above test cases should be moved to nunit 
        //    Console.WriteLine("Please Enter Position Data file fullpath(Including FileName)");
        //    Console.WriteLine("----");
        //    var inputFile = Console.ReadLine();
        //    Console.WriteLine("Please Enter Net Position Output File Path(Including FileName).");
        //    Console.WriteLine("----");
        //    var netOutputPathFile1 = Console.ReadLine();
        //    Console.WriteLine("Please Enter Box Position Output File Path(Including FileName).");
        //    Console.WriteLine("----");
        //    var boxOutputPathFile2 = Console.ReadLine();
                        
        //    IPositionDataService netDataService = new CSVPositionDataService(inputFile, netOutputPathFile1);
        //    IPositionCalculator netPocCalc = new NetPositionCalculator(netDataService);
        //    var result = netPocCalc.ProcessFile();

        //    IPositionDataService boxDataService = new CSVPositionDataService(inputFile, boxOutputPathFile2);
        //    IPositionCalculator boxPocCalc = new BoxPositionCalculator(boxDataService);
        //    var result2 = boxPocCalc.ProcessFile();

        //    if (result && result2) Console.WriteLine("Processing completed"); else Console.WriteLine("Error occured");

        //    Console.WriteLine("Press any key to continue.");
        //    Console.WriteLine("----");
        //    Console.ReadLine();
            
        //}
    }


}
