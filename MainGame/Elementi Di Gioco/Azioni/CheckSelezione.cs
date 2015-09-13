﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    public class CheckSelezione: AzioneDiGioco
    {

        public CheckSelezione (Game game) : base(game) { }

        public override void Esegui()
        {
            if (!giocatoreDiTurno.PuòAgire)
            {
                AzioneSuccessiva = new FineTurno(game);
                return;
            }

            if (gui.Tabellone.TileClick as Pianeta != null)
            {
                AzioneSuccessiva = new SelezionePianeta(game);
                return;
            }

            Nave nave = casellaCliccata?.Occupante;

            if (nave != null) { 

                if (ultimoClick == TipoEventoMouse.ClkSin && nave.Alleato(giocatoreDiTurno) && !nave.Mossa)
                {
                    AzioneSuccessiva = new MovimentoAttacco(game);
                    return;
                }

                else if (ultimoClick == TipoEventoMouse.ClkDx && nave.Alleato(giocatoreDiTurno) && (!nave.SpecialUsata || !nave.Riconfigurata))
                {
                    AzioneSuccessiva = new SelezioneDestra(game);
                    return;
                }
            }

            else if
                (gui.Cimitero.NaveSelezionata != null)
            {
                AzioneSuccessiva = new GiocaDaRiserva(game, gui.Cimitero.NaveSelezionata);
                return;
            }
        }

        public override bool Abort() { Cleanup();  return true; }
        protected override void Cleanup() { gui.Tabellone.ResetSelezioneMouse(); }

    }
}