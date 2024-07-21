namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                abc();
            }
            catch (Exception ex) { 
            Console.WriteLine(ex.ToString());   
            }
        }

        private static void abc()
        {
            var b = 0;
            var a = 1 / b;
        }
    }
}
