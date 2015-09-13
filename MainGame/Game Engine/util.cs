using System;
namespace Quantum_Game {
public static class util {

	
	// i tiri di dado avvengono tutti a proposito dei metodi delle navi, per cui incorporo nella classe nave il RNG:
		private static Random _dado;
		
		 static util () {
			_dado = new Random();
		}
		
		public static int Dadi (int num = 1) {
			int res = 0;
			for (int i = 0; i < num; i++) {
				res += _dado.Next(1,6);
			}
			return res;
		}
	}}
	