using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsActivityTracker
{
    internal class Program
    {
        private const int TITLE_WINDOWS_STRING_COUNT = 1000;
        private const string FORMAT_DATETIME_LOG = "dd-MM-yyyy HH:mm:ss";

        /// <summary>
        /// Recupera un identificador de la ventana de primer plano (la ventana con la que el usuario está trabajando actualmente). 
        /// El sistema asigna una prioridad ligeramente mayor al subproceso que crea la ventana de primer plano que a otros subprocesos.
        /// https://learn.microsoft.com/es-es/windows/win32/api/winuser/nf-winuser-getforegroundwindow
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();


        /// <summary>
        /// https://learn.microsoft.com/es-es/windows/win32/api/winuser/nf-winuser-getwindowtexta
        /// </summary>
        /// <param name="hwnd">Identificador de la ventana o control que contiene el texto.</param>
        /// <param name="sb">Búfer que recibirá el texto. Si la cadena es tan larga o más larga que el búfer, la cadena se trunca y finaliza con un carácter</param>
        /// <param name="nMaxCount">Número máximo de caracteres que se van a copiar en el búfer, incluido el carácter NULL. Si el texto supera este límite, se trunca.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder sb, int nMaxCount);


        static void Main(string[] args)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("Windows Activity Tracker!");
            Console.WriteLine("------------------------------------------------------");


            var timer = new System.Timers.Timer(1000);

            timer.Elapsed += Timer_Elapsed;

            //timer.Elapsed += (sender, eventArgs) =>
            //{
            //    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}]");
            //};

            timer.Start();
            
            Console.ReadKey();
        }


        private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            string windowTitle = GetActiveWindowTitle();

            if (string.IsNullOrEmpty(windowTitle))
            {
                Console.WriteLine($"[{DateTime.Now.ToString(FORMAT_DATETIME_LOG)}] s/d");
            }

            Console.WriteLine($"[{DateTime.Now.ToString(FORMAT_DATETIME_LOG)}] {windowTitle}");
           

        }


        private static string GetActiveWindowTitle()
        {
            var sb = new StringBuilder(TITLE_WINDOWS_STRING_COUNT);

            var handle = IntPtr.Zero;

            handle = GetForegroundWindow();

            if (GetWindowText(handle, sb, TITLE_WINDOWS_STRING_COUNT) > 0) {
                return sb.ToString();
            }
                
            return string.Empty;
        }
    }
}
