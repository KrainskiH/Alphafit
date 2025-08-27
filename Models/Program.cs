using AlphaFit.Models;

// ===== 1) Variáveis, tipos, operadores e entrada/saída =====
Console.WriteLine("=== AlphaFit • Fundamentos de C# e POO ===");
Console.Write("Seu nome: ");
string? nomeEntrada = Console.ReadLine();
if (string.IsNullOrWhiteSpace(nomeEntrada)) nomeEntrada = "Visitante";

// Operadores numéricos + tipos
int x = 12, y = 5;
int soma = x + y;
double media = (x + y) / 2.0;
Console.WriteLine($"Olá, {nomeEntrada}! Soma={soma} | Média={media:0.00}");

// ===== 2) Funções com parâmetros e retorno =====
static int Somar(int a, int b) => a + b;
Console.WriteLine($"Somar(7, 3) = {Somar(7,3)}");

// ===== 3) Arrays e List<T> =====
string[] opcoes = { "Cadastrar Aluno", "Listar Alunos", "Matricular", "Criar Treino", "Atribuir Treino", "Listar Treinos", "Estatísticas", "Sair" };
Console.WriteLine("\nMenu (array):");
for (int i = 0; i < opcoes.Length; i++) Console.WriteLine($"  {i+1}. {opcoes[i]}");

// Coleções da aplicação (List<T>)
var alunos = new List<Aluno>();
var instrutores = new List<Instrutor>();
var treinos = new List<Treino>();

// ===== 4) Boas práticas e modularização =====
// usamos classes separadas em Models e mantemos aqui apenas o "fluxo" (camada de apresentação)

// ===== 5) Enum e  tuplas =====
var perfil = (Nome: nomeEntrada, Idade: 19); // tupla nomeada
Console.WriteLine($"\n(Tupla) Perfil: Nome={perfil.Nome}, Idade={perfil.Idade}");

// Dados iniciais
var inst = new Instrutor("Marina Rocha", "Hipertrofia");
instrutores.Add(inst);

var a1 = new Aluno("Lucas Farias", "lucas@alphafit.com");
var a2 = new Aluno("Ana Souza", "ana@alphafit.com");
alunos.AddRange(new[] { a1, a2 });

// 6) Classes + atributos + métodos | 7) Construtores/instanciamento
a1.Matricular(Plano.Premium);
a2.Matricular(Plano.Plus);
a1.Apresentar();
a2.Apresentar();

// 9) Relacionamento entre classes (Treino vincula Instrutor; Aluno recebe Treino)
var treinoMaromba = inst.CriarTreino(
    "Maromba FullBody",
    NivelTreino.Intermediario,
    new[] { "Agachamento 4x10", "Supino 4x8", "Remada 4x10", "Desenvolvimento 3x12" }
);
treinos.Add(treinoMaromba);
a1.AtribuirTreino(treinoMaromba);

// 8) Encapsulamento: setters privados em Email/Id/Plano/Treino (veja as classes)

Console.WriteLine("\n--- Alunos cadastrados ---");
foreach (var a in alunos)
    Console.WriteLine($"- {a.Nome} | Plano: {a.PlanoAtual?.ToString() ?? "—"} | Treino: {a.TreinoAtual?.Nome ?? "—"}");

Console.WriteLine("\n--- Treinos disponíveis ---");
foreach (var t in treinos)
    Console.WriteLine($"- {t}");

// 10) Exercícios de modelagem + separação de responsabilidades
// Estatísticas simples retornando tupla
static (int totalAlunos, int matriculados) Estatisticas(List<Aluno> lista)
{
    int total = lista.Count;
    int comPlano = lista.Count(a => a.PlanoAtual.HasValue);
    return (total, comPlano);
}

var (tot, comPlano) = Estatisticas(alunos);
Console.WriteLine($"\nEstatísticas -> Total alunos: {tot} | Com plano: {comPlano}");

// Pequena interação (entrada de dados)
Console.Write("\nQuer cadastrar um novo aluno agora? (s/n): ");
if ((Console.ReadLine() ?? "").Trim().ToLower() == "s")
{
    Console.Write("Nome: ");
    var nome = Console.ReadLine() ?? "Sem nome";
    Console.Write("Email: ");
    var email = Console.ReadLine() ?? "sem@email";
    var novo = new Aluno(nome, email);
    alunos.Add(novo);

    Console.Write("Plano (1-Basico, 2-Plus, 3-Premium): ");
    if (int.TryParse(Console.ReadLine(), out int opc) && Enum.IsDefined(typeof(Plano), opc))
        novo.Matricular((Plano)opc);

    Console.WriteLine("Aluno cadastrado!");
}

Console.WriteLine("\nFim do demo. Pressione ENTER para sair.");
Console.ReadLine();
