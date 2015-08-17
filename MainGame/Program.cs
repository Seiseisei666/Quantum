using System;

namespace Quantum_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>C:\Users\Asus\Documents\GitHubVisualStudio\Quantum\MainGame\MainGame\Program.cs
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Quantum())
                game.Run();
        }
    }
#endif
}
