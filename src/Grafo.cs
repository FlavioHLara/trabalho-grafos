using System;
using System.Collections.Generic;
using System.Linq;

namespace TrabalhoPraticoGrafos
{
    public class Grafo
    {
        private int numeroDeVertices;
        // Lista de Adjacência: Para cada vértice, temos uma lista de arestas
        private List<List<Aresta>> listaDeAdjacencia;

        public Grafo(int vertices)
        {
            this.numeroDeVertices = vertices;
            listaDeAdjacencia = new List<List<Aresta>>(vertices);
            for (int i = 0; i < vertices; i++)
            {
                listaDeAdjacencia.Add(new List<Aresta>());
            }
        }

        // Adiciona aresta nos dois sentidos
        public void AdicionarAresta(int origem, int destino, int peso)
        {
            listaDeAdjacencia[origem].Add(new Aresta { Destino = destino, Peso = peso });
            listaDeAdjacencia[destino].Add(new Aresta { Destino = origem, Peso = peso });
        }

        //  1. Travessia em Profundidade (DFS)  
        public void ExecutarDFS(int verticeInicial)
        {
            Console.WriteLine($"\nIniciando DFS a partir do vértice {verticeInicial} ");
            bool[] visitados = new bool[numeroDeVertices];
            DFSRecursivo(verticeInicial, visitados);
            Console.WriteLine(); // Pular linha
        }

        private void DFSRecursivo(int vertice, bool[] visitados)
        {
            visitados[vertice] = true;
            Console.Write(vertice + " "); // Exibe o vértice visitado

            foreach (var aresta in listaDeAdjacencia[vertice])
            {
                if (!visitados[aresta.Destino])
                {
                    DFSRecursivo(aresta.Destino, visitados);
                }
            }
        }

        //  2. Travessia em Amplitude (BFS)  
        public void ExecutarBFS(int verticeInicial)
        {
            Console.WriteLine($"\nIniciando BFS a partir do vértice {verticeInicial} ");
            bool[] visitados = new bool[numeroDeVertices];
            Queue<int> fila = new Queue<int>();

            visitados[verticeInicial] = true;
            fila.Enqueue(verticeInicial);

            while (fila.Count > 0)
            {
                int verticeAtual = fila.Dequeue();
                Console.Write(verticeAtual + " ");

                foreach (var aresta in listaDeAdjacencia[verticeAtual])
                {
                    if (!visitados[aresta.Destino])
                    {
                        visitados[aresta.Destino] = true;
                        fila.Enqueue(aresta.Destino);
                    }
                }
            }
            Console.WriteLine();
        }

        //  3. Dijkstra - Menor caminho  
        public void ExecutarDijkstra(int origem)
        {
            Console.WriteLine($"\nDijkstra: Menores caminhos a partir do vértice {origem} ");

            int[] distancias = new int[numeroDeVertices];
            bool[] processados = new bool[numeroDeVertices];

            // Inicializa todas as distâncias como infinito
            for (int i = 0; i < numeroDeVertices; i++)
                distancias[i] = int.MaxValue;

            distancias[origem] = 0;

            for (int count = 0; count < numeroDeVertices - 1; count++)
            {
                // Pega o vértice com a menor distância que ainda não foi processado
                int u = MenorDistancia(distancias, processados);
                
                // Se u for -1, significa que os vértices restantes são inalcançáveis
                if (u == -1) break;

                processados[u] = true;

                foreach (var aresta in listaDeAdjacencia[u])
                {
                    if (!processados[aresta.Destino] && 
                        distancias[u] != int.MaxValue && 
                        distancias[u] + aresta.Peso < distancias[aresta.Destino])
                    {
                        distancias[aresta.Destino] = distancias[u] + aresta.Peso;
                    }
                }
            }

            ImprimirSolucaoDijkstra(distancias, origem);
        }

        //  4. Algoritmo de Prim - Árvore Geradora Mínima  
        public void ExecutarPrim()
        {
            Console.WriteLine("\nPrim: Árvore Geradora Mínima (MST) ");

            int[] pesoMinimo = new int[numeroDeVertices];
            int[] pai = new int[numeroDeVertices]; // Para armazenar a árvore
            bool[] naMST = new bool[numeroDeVertices];

            // Inicializa
            for (int i = 0; i < numeroDeVertices; i++)
            {
                pesoMinimo[i] = int.MaxValue;
                naMST[i] = false;
            }

            // Começa pelo primeiro vértice (raiz arbitrária)
            pesoMinimo[0] = 0;
            pai[0] = -1; 

            for (int count = 0; count < numeroDeVertices - 1; count++)
            {
                int u = MenorDistancia(pesoMinimo, naMST); // Reutiliza a lógica de buscar o menor
                
                if (u == -1) break;

                naMST[u] = true;

                foreach (var aresta in listaDeAdjacencia[u])
                {
                    if (naMST[aresta.Destino] == false && aresta.Peso < pesoMinimo[aresta.Destino])
                    {
                        pai[aresta.Destino] = u;
                        pesoMinimo[aresta.Destino] = aresta.Peso;
                    }
                }
            }

            ImprimirPrim(pai, pesoMinimo);
        }

        //  Métodos Auxiliares 
        
        // Encontra o vértice com a menor distância/peso que ainda não foi processado
        private int MenorDistancia(int[] valores, bool[] processados)
        {
            int min = int.MaxValue;
            int indiceMin = -1;

            for (int v = 0; v < numeroDeVertices; v++)
            {
                if (processados[v] == false && valores[v] <= min)
                {
                    min = valores[v];
                    indiceMin = v;
                }
            }
            return indiceMin;
        }

        private void ImprimirSolucaoDijkstra(int[] distancias, int origem)
        {
            Console.WriteLine("Vértice \t Distância da Origem (" + origem + ")");
            for (int i = 0; i < numeroDeVertices; i++)
            {
                string dist = distancias[i] == int.MaxValue ? "Infinito" : distancias[i].ToString();
                Console.WriteLine(i + " \t\t " + dist);
            }
        }

        private void ImprimirPrim(int[] pai, int[] pesos)
        {
            Console.WriteLine("Aresta \t\t Peso");
            int custoTotal = 0;
            for (int i = 1; i < numeroDeVertices; i++)
            {
                Console.WriteLine(pai[i] + " - " + i + " \t\t " + pesos[i]);
                custoTotal += pesos[i];
            }
            Console.WriteLine("Custo Total da MST: " + custoTotal);
        }
    }
}