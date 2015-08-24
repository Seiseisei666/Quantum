﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class CheckSelezione: AzioneDiGioco
    {

        public CheckSelezione (Game game) : base(game) { }

        public override void Esegui()
        {
            Nave nave =
               casellaCliccata?.Occupante;

            if (nave == null) return;

            else if
                (ultimoClick == TipoEventoMouse.ClkSin && nave.Alleato(giocatoreDiTurno) && !nave.Mossa)
            {
                AzioneSuccessiva = new MovimentoAttacco(game);
                System.Diagnostics.Debug.WriteLine("Click Sinistro");
            }

            else if
                (ultimoClick == TipoEventoMouse.ClkDx && nave.Alleato(giocatoreDiTurno) &&
                (!nave.SpecialUsata || !nave.Riconfigurata))
            {
                AzioneSuccessiva = new SelezioneDestra(game);
                System.Diagnostics.Debug.WriteLine("Click Destro");
            }
        }

    }
}
