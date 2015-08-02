using Quantum_Game;
namespace Quantum_Game {

public class Test {
		
		public static void Main () {
			
			Giocatore g1, g2;
			
			g1= new Giocatore();
			g2= new Giocatore();
			
			System.Console.WriteLine ("Il colore del primo giocatore è {0}, mentre quello del secondo è {1}", g1.Colore, g2.Colore);
			
			g1.PiazzaNave();
			g2.PiazzaNave();
			
			e_nave temp = g2.Flotta[0].Tipo;
			Nave n1 = g1.Flotta[0];
			Nave n2= g2.Flotta[0];
			
			
			if (n1.Attacco(n2)) {
				System.Console.WriteLine ("La nave {0} {1} ha distrutto la {2} {3}!!", n1.Tipo, n1.Colore, temp, n2.Colore );
				System.Console.WriteLine ("Stato della nave in difesa: {0}", n2.Viva);
				System.Console.WriteLine ("Ora la nave in difesa è un {0}", n2.Tipo);}
			else {
				System.Console.WriteLine ("La nave {0} è stata sconfitta dalla {1}!!", n1.Tipo, n2.Tipo);
				System.Console.WriteLine ("Stato della nave in difesa: {0}", n2.Viva);}
			
			g1.PiazzaNave(); g1.PiazzaNave(); g1.PiazzaNave();
			
			foreach (var n in g1.Flotta) {
				System.Console.WriteLine (n.Tipo);
			}
			
			
			System.Console.ReadKey();
		
//		System.Console.WriteLine ("dajea");
//			Nave navicella = new Nave(e_color.Blu);
//			navicella.Gioca();
//			navicella.Riconfig();
//			System.Console.WriteLine (navicella.Tipo);
//			navicella.Riconfig();
//			System.Console.WriteLine (navicella.Tipo);
//			System.Console.ReadKey();
		}
}
}
