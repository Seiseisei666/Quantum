using System;

namespace Quantum_Game
{
#if WINDOWS || LINUX

    public static class Start
    {
        // The main entry point for the application.
        [STAThread]
        public static void Main()
        {
            // TODO: creare un menu' di entrata
            
            //modalità "single"player:   
            using (var game = new Quantum())
                game.Run();

            //modalità multiplayer:
            /*
            using (var game = new QuantumMultiplayer())
                game.Run();
            */
        }
    }
#endif
}
