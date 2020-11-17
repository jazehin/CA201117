using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA201117
{
    class Program
    {
        struct Tanulo
        {
            public string evf, jel, vnev, knev;

            public Tanulo Uj(string[] args)
            {
                this.evf = args[0];
                this.jel = args[1];
                this.vnev = args[2];

                string knev = ""; //+utónév...
                for (int i = 3; i < args.Length; i++)
                {
                    knev += args[i];
                    if (i != args.Length - 1) knev += " ";
                }

                this.knev = knev;
                return this;
            }
        }

        static List<Tanulo> tanulok = new List<Tanulo>();
        static void Main()
        {
            //a) Olvassa be a nevek.txt állományban talált adatokat, s annak felhasználásával oldja meg a következő feladatokat!
            A();

            //b) Írja ki a képernyőre, hogy hányan járnak az iskolába!
            B();

            //c) Kérje be az egyik évfolyamot és osztályt billentyűzetről, és írja ki a képernyőre az osztályban tanulók nevét!
            C();

            //d) Írja ki a képernyőre, hogy ki rendelkezik a leghosszabb névvel! A szóköz nem tartozik a névhez. Ha valakinek több keresztneve van, mindegyik növeli nevének hosszát.
            D();

            /*e) 
            Határozza meg és írja a képernyőre az egyes osztályok létszámát, valamint a legkisebb létszámú osztály évfolyamát és osztályjelét!
                Példa:
                2005 c 28 fő
            */
            E();

            //f) Készítsen lista.txt néven kimeneti fájlt, melyben évfolyamonként, azon belül osztályonként névsorban írja ki a diákok nevét!
            F();

            /*
            g) Az iskolai rendszergazdának egyedi azonosítókat kell készítenie a számítógép-hálózat használatához. Az azonosítókat a következő módon alakítja ki: első karaktere az évfolyam utolsó számjegye, következő karakter az osztály betűjele, majd vezetékneve első három karaktere, végül első keresztneve első három karaktere következik. Kérje be egy diák adatait és írja ki az azonosítóját! Az azonosítóban mindenütt kisbetűk szerepelnek. Feltételezhetjük, hogy a vezetéknév legalább 3 karakteres.
                Példa:
                2004 d Vavrek Kristóf azonosítója: 4dvavkri
            */
            G();

            //h) Kérjen be egy azonosítót és állapítsa meg, ki tartozhat hozzá! Adatait írja a képernyőre! Ha nem talál megfelelő diákot, akkor a „Nincs megfelelő személy” szöveget jelenítse meg!
            H();
            Console.ReadKey();
        }

        private static void H()
        {
            Console.Clear();
            Console.Write("Adjon meg egy azonosítót: ");
            string azonosito = Console.ReadLine();
            for (int i = 0; i < tanulok.Count; i++)
            {
                if (Azonosito("", "", "", "", tanulok[i]) == azonosito) { 
                    Console.WriteLine($"Osztály: {tanulok[i].evf}.{tanulok[i].jel}\nNév: {tanulok[i].vnev} {tanulok[i].knev}");
                    return;
                }
            }
            Console.WriteLine("Nincs megfelelő személy");
        }

        private static void G()
        {
            string evf, jel, vnev, knev;
            do
            {
                Console.Clear();
                Console.Write("Adjon meg az évfolyamát: ");
                evf = Console.ReadLine();
                Console.Write("Adjon meg az osztályjelét: ");
                jel = Console.ReadLine();
                Console.Write("Adjon meg a vezetéknevét: ");
                vnev = Console.ReadLine();
                Console.Write("Adjon meg a kereszt, ill. utóneve(i)t: ");
                knev = Console.ReadLine();
            } while (!TanuloLetezik(evf, jel, vnev, knev));
            Console.WriteLine($"{vnev} {knev} azonosítója {Azonosito(evf, jel, vnev, knev, new Tanulo())}");
            Console.ReadKey();
        }

        private static bool TanuloLetezik(string evf, string jel, string vnev, string knev)
        {
            bool letezik = false;
            for (int i = 0; i < tanulok.Count; i++)
            {
                if (tanulok[i].vnev == vnev && tanulok[i].knev == knev && tanulok[i].evf == evf && tanulok[i].jel == jel) letezik = true;
            }
            return letezik;
        }

        private static string Azonosito(string evf, string jel, string vnev, string knev, Tanulo tanulo)
        {
            if (!(evf == "")) return $"{evf[3]}{jel}{vnev.ToLower().Substring(0, 3)}{knev.ToLower().Substring(0, 3)}";
            else return $"{tanulo.evf[3]}{tanulo.jel}{tanulo.vnev.ToLower().Substring(0, 3)}{tanulo.knev.ToLower().Substring(0, 3)}";
        }

        private static void F()
        {
            var sw = new StreamWriter(@"..\..\Res\lista.txt", false, Encoding.UTF8);

            Dictionary<string, List<string>> osztalyok = new Dictionary<string, List<string>>();
            foreach (var tanulo in tanulok)
            {
                if (!osztalyok.ContainsKey($"{tanulo.evf}{tanulo.jel}")) osztalyok.Add($"{tanulo.evf}{tanulo.jel}", new List<string>());
                osztalyok[$"{tanulo.evf}{tanulo.jel}"].Add($"{tanulo.vnev} {tanulo.knev}");
                //Console.WriteLine(osztalyok[$"{tanulo.evf}{tanulo.jel}"][osztalyok[$"{tanulo.evf}{tanulo.jel}"].Count-1]);
            }
            List<string> osztNevek = new List<string>(osztalyok.Count);
            foreach (var osztaly in osztalyok)
            {
                osztNevek.Add(osztaly.Key);
                osztaly.Value.Sort(); //elég lusta és nem is tökéletes de egyelőre megteszi
            }
            osztNevek.Sort();
            foreach (var osztNev in osztNevek)
            {
                sw.WriteLine($"{osztNev}:");
                foreach (var nev in osztalyok[osztNev])
                {
                    sw.WriteLine(nev);
                }
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            Console.Clear();
            Console.WriteLine("Befejeztem az írást");
            Console.ReadKey();
        }

        private static void E()
        {
            Console.Clear();
            Dictionary<string, int> osztalyok = new Dictionary<string, int>();
            for (int i = 0; i < tanulok.Count; i++)
            {
                if (osztalyok.ContainsKey($"{tanulok[i].evf}{tanulok[i].jel}"))
                {
                    osztalyok[$"{tanulok[i].evf}{tanulok[i].jel}"]++;
                }
                else
                {
                    osztalyok.Add($"{tanulok[i].evf}{tanulok[i].jel}", 1);
                }
            }
            foreach (var osztaly in osztalyok)
            {
                Console.WriteLine($"{osztaly.Key.Substring(0, 4)}.{osztaly.Key[4]} : {osztaly.Value} fő");
            }
            Console.WriteLine("--------------------");
            string minKey = "";
            osztalyok.Add(minKey, int.MaxValue);
            foreach (var osztaly in osztalyok)
            {
                if (osztalyok[minKey] > osztaly.Value) minKey = osztaly.Key;
            }
            Console.WriteLine($"A legkevesebb tanulója a {minKey.Substring(0, 4)}.{minKey[4]} osztálynak van");
            Console.ReadKey();
        }

        private static void D()
        {
            Console.Clear();
            int maxi = 0;
            for (int i = 1; i < tanulok.Count; i++)
            {
                if (tanulok[maxi].vnev.Length + tanulok[maxi].knev.Replace(" ", "").Length < tanulok[i].vnev.Length + tanulok[i].knev.Replace(" ", "").Length) maxi = i;
            }
            Console.WriteLine($"A leghosszabb neve {tanulok[maxi].vnev} {tanulok[maxi].knev}-nak/nek van.");
            Console.ReadKey();
        }

        private static void C()
        {
            Console.Clear();
            string evf; string jel;
            do
            {
                Console.Clear();
                Console.Write("Adjon meg egy évfolyamot: ");
                evf = Console.ReadLine();
                Console.Write("Adjon meg egy osztályjelet: ");
                jel = Console.ReadLine();
            } while (!OsztalyLetezik(evf, jel));
            for (int i = 0; i < tanulok.Count; i++)
            {
                if (tanulok[i].evf == evf && tanulok[i].jel == jel) Console.WriteLine($"{tanulok[i].vnev} {tanulok[i].knev}");
            }
            Console.ReadKey();
        }

        private static bool OsztalyLetezik(string evf, string jel)
        {
            bool letezik = false;
            for (int i = 0; i < tanulok.Count; i++)
            {
                if (tanulok[i].evf == evf && tanulok[i].jel == jel) letezik = true;
            }
            return letezik;
        }

        private static void B()
        {
            Console.Clear();
            Console.WriteLine($"Az iskolába {tanulok.Count} tanuló jár.");
            Console.ReadKey();
        }

        private static void A()
        {
            var sr = new StreamReader(@"..\..\Res\nevek.txt", Encoding.UTF8, false);
            while (!sr.EndOfStream)
            {
                string[] adatok = sr.ReadLine().Split(' ');
                tanulok.Add(new Tanulo().Uj(adatok));
            }
            sr.Close();
            Console.WriteLine("Beolvastam az adatokat");
            Console.ReadKey();
        }
    }
}
