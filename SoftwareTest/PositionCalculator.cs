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
}
