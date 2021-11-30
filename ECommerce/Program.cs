using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var pedidos = new List<(int, Stopwatch)>();

            for (int i = 0; i < 5000; i++)
            {
                Console.WriteLine($"{DateTime.Now:g} - Realizando pedido {i}");
                pedidos.Add((i, Stopwatch.StartNew()));
            }

            Console.WriteLine();

            foreach (var item in pedidos)
            {
                await Task.Delay(200);

                item.Item2.Stop();
                var tempo = TimeSpan.FromMilliseconds(item.Item2.ElapsedMilliseconds);

                Console.WriteLine($"{DateTime.Now:g} - Processando pedido {item.Item1}, levou {tempo.TotalSeconds} segundos desde a realização");
            }
        }
    }
}
