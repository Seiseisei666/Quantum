using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Quantum_Game
{
    public class FlussoDiGioco
    {
        private GameSystem gameSystem;
        private Tabellone tabellone;
        private Giocatore giocatoreDiTurno;
        private MouseInput mouseInput;

        private bool _leggiClickDx;
        public bool LeggiClickDx { get { return _leggiClickDx; } }


        public object OggettoSelezionato
        {
            get
            {
                Casella cas = tabellone.TileSelezionato as Casella;
                Pianeta pian = tabellone.TileSelezionato as Pianeta;

                if (cas != null && cas.Occupante != null)
                    return cas.Occupante;
                else if (pian != null)
                    return pian;
                else return null;
            }
        }
        private Nave _naveSel { get { return OggettoSelezionato as Nave; } }
        private Pianeta _pianetaSel { get { return OggettoSelezionato as Pianeta; } }
        private Casella _casellaSel { get { return tabellone.TileSelezionato as Casella; } }

        public FlussoDiGioco(GameSystem m_gameSystem, MouseInput m_mouseInput, Tabellone m_tabellone)
        {
            gameSystem = m_gameSystem;
            tabellone = m_tabellone;
            mouseInput = m_mouseInput;
            _leggiClickDx = false;
        }



        public void Update()
        {
            giocatoreDiTurno = gameSystem.GiocatoreDiTurno;
            gestioneClickSinistro();
            if (_leggiClickDx)
                gestioneClickDestro();
        }

        // Qui vengono fatte le operazioni relative al click sinistro
        // (per ora solo movimento/attacco)
        private void gestioneClickSinistro()
        {
            _leggiClickDx =                                 // condizioni per interessarsi al click DX:

                (_naveSel != null &&                        // nave selezionata AND
                _naveSel.Alleato(giocatoreDiTurno) &&       // alleata AND
                !(_naveSel.Mossa || _naveSel.SpecialUsata));  // la nave selezionata non ha mosso

        }

        // Se serve, qui gestiamo il click destro (movimento/attacco)
        private void gestioneClickDestro()
        {
                Casella target = tabellone.TileTarget as Casella;
                if (target != null)
                {
                    Nave naveTarget = target.Occupante;
                    if (naveTarget != null)
                    {
                        if (!naveTarget.Alleato(giocatoreDiTurno))
                        {
                            bool RisultatoAttacco;
                            RisultatoAttacco = _naveSel.Attacco(naveTarget);

                            Debug.WriteLine("Una nave {0} di colore {1} ha attaccato una nave {2} di colore {3}.", 
                                _naveSel.Tipo, _naveSel.Colore, naveTarget.Tipo,naveTarget.Colore);
                            Debug.WriteLine("risultato: {0}", RisultatoAttacco);

                            if (RisultatoAttacco == true)
                                _naveSel.Muovi(_casellaSel, target);
                        }
                    }

                    else
                        _naveSel.Muovi(_casellaSel, target);

                }
            }
            
        }
    }

