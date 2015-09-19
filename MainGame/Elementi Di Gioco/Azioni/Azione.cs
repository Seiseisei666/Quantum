

namespace Quantum_Game.Azioni
{
    public abstract class Azione
{
        /*Metodo pubblico per eseguire l'azione e liberare le risorse */
        public void Start()
        {
            Esegui();
            Cleanup();
        }
        /*Metodo pubblico per fermare l'azione e liberare le risorse */
        public void Stop()
        {
             Cleanup();
        }

        /*Esegue l'azione*/
        protected abstract void Esegui();

        /* Da chiamare quando l'azione è finita per liberare eventuali risorse utilizzate */
        protected abstract void Cleanup();

    }
}
