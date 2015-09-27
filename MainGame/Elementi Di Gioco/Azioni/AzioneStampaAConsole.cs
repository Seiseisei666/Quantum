using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    /* azione da utilizzare per stampare a console */
    // EDIT: Questa mi sembra esagerata...
    public class AzioneStampaAConsole : Azione
    {
        private String messaggio;
        private Color color;


        public AzioneStampaAConsole(String messaggio, Color color)
        {
            this.messaggio = messaggio;
            this.color = color;

        }
        
        public AzioneStampaAConsole(String messaggio) : this(messaggio, Color.White)
        {
        }
        protected override void Cleanup()
        {
            
        }

        public override bool Abort() { return false; }

        protected override void Esegui()
        {
            ConsoleMessaggi.NuovoMessaggio(messaggio, color);
            Terminata = true;
        }
    }
}
