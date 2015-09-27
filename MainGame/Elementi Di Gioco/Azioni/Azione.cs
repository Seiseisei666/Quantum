

namespace Quantum_Game.Azioni
{
    public abstract class Azione
{
        protected Azione ()
        {
            Terminata = false;
        }

        /*Metodo pubblico per eseguire l'azione e liberare le risorse */
        public void Start()
        {
            //EDIT: A questo punto non vedo perché separare Start da Esegui! Metterei solo un Esegui astratto e pubblico
            Esegui();
        }

        /*Metodo pubblico per fermare l'azione e liberare le risorse */
        /* EDIT: preferisco rinominare Stop in Abort, rende meglio l'idea
        inoltre dovrebbe restituire un bool per dire che l'interruzione dell'azione è andata a buon fine */
        public virtual bool Abort()
        {
            Cleanup();
            Terminata = true;
            return true;
        }

        /*Esegue l'azione*/
        protected abstract void Esegui();

        /* True quando l'azione è terminata */
        public bool Terminata { get; protected set; }

        /* Da chiamare quando l'azione è finita per liberare eventuali risorse utilizzate */
        protected abstract void Cleanup();

    }
}
