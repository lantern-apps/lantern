namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Task.Run(() =>
            {
                bool prevInstance;
                _ = new System.Threading.Mutex(true, "Application Name", out prevInstance);
                if (prevInstance == false)
                {
                    MessageBox.Show("There is another instance running");
                    return;
                }
            });
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            Application.Run(new Form1());

            Console.ReadLine();
        }
    }
}