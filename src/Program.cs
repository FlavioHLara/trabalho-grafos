using System;

namespace TrabalhoPraticoGrafos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Trabalho Prático: Algoritmos em Grafos ===");
            Console.WriteLine("Integrantes do Grupo: [Seu Nome Aqui]");

            while (true)
            {
                Grafo grafo = null;

                Console.WriteLine("\n=================================");
                Console.WriteLine("       CONFIGURAÇÃO DO GRAFO      ");
                Console.WriteLine("=================================");
                Console.WriteLine("1 - Gerar Grafo Aleatório (Teste Rápido)");
                Console.WriteLine("2 - Criar Grafo Manualmente (Inserir dados)");
                Console.WriteLine("0 - Sair do Programa");
                Console.Write("Escolha uma opção: ");

                string opcaoEntrada = Console.ReadLine();

                if (opcaoEntrada == "0") break;

                if (opcaoEntrada == "1")
                {
                    grafo = GerarGrafoAleatorio();
                }
                else if (opcaoEntrada == "2")
                {
                    grafo = CriarGrafoManual();
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                    continue;
                }

                // Menu de Algoritmos (executa sobre o grafo criado acima)
                MenuAlgoritmos(grafo);
            }
        }

        // Função para gerar grafo automático com números aleatórios
        static Grafo GerarGrafoAleatorio()
        {
            Console.Write("\nQuantos vértices deseja no grafo aleatório? ");
            int qtdVertices;
            if (!int.TryParse(Console.ReadLine(), out qtdVertices) || qtdVertices <= 0)
            {
                Console.WriteLine("Número inválido, usando padrão de 5 vértices.");
                qtdVertices = 5;
            }

            Grafo g = new Grafo(qtdVertices);
            Random random = new Random();

            Console.WriteLine($"\nGerando arestas aleatórias com pesos de 1 a 10...");
            
            // Cria algumas arestas aleatórias
            // A lógica aqui tenta criar um grafo razoavelmente conectado
            for (int i = 0; i < qtdVertices; i++)
            {
                // Conecta cada vértice a 1 ou 2 outros vértices aleatórios
                int destino = random.Next(0, qtdVertices);
                int peso = random.Next(1, 11); // Peso de 1 a 10

                if (i != destino) // Evita laço no mesmo vértice
                {
                    g.AdicionarAresta(i, destino, peso);
                    Console.WriteLine($"Aresta criada: {i} <--> {destino} [Peso: {peso}]");
                }
                
                // Tenta criar uma segunda conexão para garantir mais caminhos
                int destino2 = random.Next(0, qtdVertices);
                if (i != destino2 && destino != destino2)
                {
                    int peso2 = random.Next(1, 11);
                    g.AdicionarAresta(i, destino2, peso2);
                    Console.WriteLine($"Aresta criada: {i} <--> {destino2} [Peso: {peso2}]");
                }
            }
            Console.WriteLine("Grafo Aleatório Gerado com Sucesso!");
            return g;
        }

        // Função para o usuário digitar o grafo
        static Grafo CriarGrafoManual()
        {
            Console.Write("\nDigite a quantidade total de vértices do grafo: ");
            int vertices = int.Parse(Console.ReadLine());
            Grafo g = new Grafo(vertices);

            Console.WriteLine("\n--- Inserção de Arestas ---");
            Console.WriteLine("Digite as conexões. Para parar, digite -1 na origem.");

            while (true)
            {
                try
                {
                    Console.Write("\nVértice de Origem (ou -1 para parar): ");
                    int origem = int.Parse(Console.ReadLine());
                    if (origem == -1) break;

                    if (origem < 0 || origem >= vertices)
                    {
                        Console.WriteLine($"Erro: O vértice deve ser entre 0 e {vertices - 1}");
                        continue;
                    }

                    Console.Write("Vértice de Destino: ");
                    int destino = int.Parse(Console.ReadLine());

                    if (destino < 0 || destino >= vertices)
                    {
                        Console.WriteLine($"Erro: O vértice deve ser entre 0 e {vertices - 1}");
                        continue;
                    }

                    Console.Write("Peso da Aresta: ");
                    int peso = int.Parse(Console.ReadLine());

                    g.AdicionarAresta(origem, destino, peso);
                    Console.WriteLine("Aresta adicionada!");
                }
                catch (Exception)
                {
                    Console.WriteLine("Entrada inválida. Use apenas números inteiros.");
                }
            }
            return g;
        }

        // Menu de Operações
        static void MenuAlgoritmos(Grafo g)
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n---------------------------------");
                Console.WriteLine("      ESCOLHA O ALGORITMO        ");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1 - Busca em Profundidade (DFS)");
                Console.WriteLine("2 - Busca em Amplitude (BFS)");
                Console.WriteLine("3 - Dijkstra (Menor Caminho)");
                Console.WriteLine("4 - Prim (Árvore Geradora Mínima)");
                Console.WriteLine("5 - Preencher um NOVO Grafo (Voltar)");
                Console.WriteLine("0 - Encerrar Programa");
                Console.Write("Opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.Write("Digite o vértice inicial para DFS: ");
                        int inicioDFS = int.Parse(Console.ReadLine());
                        g.ExecutarDFS(inicioDFS);
                        break;
                    case "2":
                        Console.Write("Digite o vértice inicial para BFS: ");
                        int inicioBFS = int.Parse(Console.ReadLine());
                        g.ExecutarBFS(inicioBFS);
                        break;
                    case "3":
                        Console.Write("Digite o vértice de origem para Dijkstra: ");
                        int inicioDijkstra = int.Parse(Console.ReadLine());
                        g.ExecutarDijkstra(inicioDijkstra);
                        break;
                    case "4":
                        g.ExecutarPrim();
                        break;
                    case "5":
                        continuar = false; // Sai do loop interno e volta para o menu principal
                        break;
                    case "0":
                        Console.WriteLine("Encerrando...");
                        Environment.Exit(0); // Mata o programa todo
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
                
                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}