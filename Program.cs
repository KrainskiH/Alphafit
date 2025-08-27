using AlphaFit.Models;

List<Aluno> alunos = new();
List<Instrutor> instrutores = new();
List<Treino> treinos = new();

// ===== Seed (dados iniciais) =====
var instPadrao = new Instrutor("Marina Rocha", "Hipertrofia");
instrutores.Add(instPadrao);

alunos.Add(new Aluno("Lucas Farias", "lucas@alpha.fit"));
alunos.Add(new Aluno("Ana Souza", "ana@alpha.fit"));
alunos[0].Matricular(Plano.Premium);
alunos[1].Matricular(Plano.Plus);

treinos.Add(instPadrao.CriarTreino(
    "FullBody Intermediário",
    NivelTreino.Intermediario,
    new[] { "Agachamento 4x10", "Supino 4x8", "Remada Curvada 4x10", "Desenvolvimento 3x12" }
));

// ===== Loop do menu =====
while (true)
{
    Console.Clear();
    Console.WriteLine("=== AlphaFit • Fundamentos C# e POO (Aula 27) ===\n");
    string[] menu = {
        "Cadastrar aluno",
        "Listar alunos",
        "Criar treino",
        "Atribuir treino a aluno",
        "Atualizar plano do aluno",
        "Remover aluno",
        "Listar treinos",
        "Estatísticas",
        "Sair"
    };
    for (int i = 0; i < menu.Length; i++) Console.WriteLine($"{i + 1}. {menu[i]}");

    int opc = LerInt("\nEscolha: ");
    switch (opc)
    {
        case 1: CadastrarAluno(); break;
        case 2: ListarAlunos(); break;
        case 3: CriarTreino(); break;
        case 4: AtribuirTreino(); break;
        case 5: AtualizarPlano(); break;
        case 6: RemoverAluno(); break;
        case 7: ListarTreinos(); break;
        case 8: MostrarEstatisticas(); break;
        case 9:
        case 0: return;
        default: Pausa("Opção inválida."); break;
    }
}

// ======= Casos de uso =======

void CadastrarAluno()
{
    Console.WriteLine("\n== Cadastrar aluno ==");
    string nome  = LerTexto("Nome: ");
    string email = LerTexto("Email: ");
    Console.WriteLine("Plano (1-Basico, 2-Plus, 3-Premium)");
    var plano = (Plano)LerInt("Escolha: ", min: 1, max: 3);

    try
    {
        var novo = new Aluno(nome, email);
        novo.Matricular(plano);
        alunos.Add(novo);
        Pausa("Aluno cadastrado!");
    }
    catch (Exception ex)
    {
        Pausa("Erro: " + ex.Message);
    }
}

void ListarAlunos()
{
    Console.WriteLine("\n== Alunos ==");
    if (alunos.Count == 0) { Pausa("(sem alunos)"); return; }

    foreach (var a in alunos)
        Console.WriteLine($"- {a.Nome} | Plano: {a.PlanoAtual?.ToString() ?? "—"} | Treino: {a.TreinoAtual?.Nome ?? "—"}");
    Pausa();
}

void CriarTreino()
{
    Console.WriteLine("\n== Criar treino ==");
    string nome = LerTexto("Nome do treino: ");
    Console.WriteLine("Nível (1-Iniciante, 2-Intermediario, 3-Avancado)");
    var nivel = (NivelTreino)LerInt("Escolha: ", 1, 3);

    var instrutor = SelecionarOuCriarInstrutor();

    var t = instrutor.CriarTreino(nome, nivel, Array.Empty<string>());

    Console.WriteLine("Adicione exercícios (vazio para parar):");
    while (true)
    {
        var ex = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ex)) break;
        t.AdicionarExercicio(ex);
    }

    treinos.Add(t);
    Pausa("Treino criado!");
}

void AtribuirTreino()
{
    Console.WriteLine("\n== Atribuir treino ==");
    var aluno = BuscarAlunoPorNome();
    if (aluno is null) return;

    var treino = SelecionarTreino();
    if (treino is null) return;

    aluno.AtribuirTreino(treino);
    Pausa("Treino atribuído!");
}

void AtualizarPlano()
{
    Console.WriteLine("\n== Atualizar plano do aluno ==");
    var aluno = BuscarAlunoPorNome();
    if (aluno is null) return;

    Console.WriteLine("Plano (1-Basico, 2-Plus, 3-Premium)");
    var plano = (Plano)LerInt("Escolha: ", 1, 3);
    aluno.Matricular(plano);
    Pausa("Plano atualizado!");
}

void RemoverAluno()
{
    Console.WriteLine("\n== Remover aluno ==");
    var aluno = BuscarAlunoPorNome();
    if (aluno is null) return;

    alunos.Remove(aluno);
    Pausa("Aluno removido.");
}

void ListarTreinos()
{
    Console.WriteLine("\n== Treinos ==");
    if (treinos.Count == 0) { Pausa("(sem treinos)"); return; }
    foreach (var t in treinos) Console.WriteLine($"- {t}");
    Pausa();
}

void MostrarEstatisticas()
{
    // Retorna tupla (total, comPlano) — atende o item 'tuplas'
    (int total, int comPlano) = (
        alunos.Count,
        alunos.Count(a => a.PlanoAtual.HasValue)
    );

    Console.WriteLine($"\nTotal de alunos: {total}");
    Console.WriteLine($"Alunos com plano: {comPlano}");

    // Exemplo com tipos numéricos/operadores
    double perc = total == 0 ? 0 : (comPlano * 100.0) / total;
    Console.WriteLine($"% com plano: {perc:0.0}%");
    Pausa();
}

// ======= Helpers =======

Aluno? BuscarAlunoPorNome()
{
    string nome = LerTexto("Nome do aluno: ");
    var aluno = alunos.FirstOrDefault(a => a.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
    if (aluno is null) Pausa("Aluno não encontrado.");
    return aluno;
}

Treino? SelecionarTreino()
{
    if (treinos.Count == 0) { Pausa("Nenhum treino cadastrado."); return null; }
    for (int i = 0; i < treinos.Count; i++) Console.WriteLine($"{i + 1}. {treinos[i]}");
    int idx = LerInt("Escolha: ", 1, treinos.Count) - 1;
    return treinos[idx];
}

Instrutor SelecionarOuCriarInstrutor()
{
    Console.Write("Usar instrutor padrão (s/n)? ");
    var s = (Console.ReadLine() ?? "").Trim().ToLower();
    if (s == "s") return instrutores[0];

    string nome = LerTexto("Nome do instrutor: ");
    string esp  = LerTexto("Especialidade: ");
    var novo = new Instrutor(nome, esp);
    instrutores.Add(novo);
    return novo;
}

string LerTexto(string label)
{
    Console.Write(label);
    var v = Console.ReadLine() ?? "";
    return v.Trim();
}

int LerInt(string label, int? min = null, int? max = null)
{
    while (true)
    {
        Console.Write(label);
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            if (min.HasValue && n < min.Value) { Console.WriteLine($"(mínimo {min})"); continue; }
            if (max.HasValue && n > max.Value) { Console.WriteLine($"(máximo {max})"); continue; }
            return n;
        }
        Console.WriteLine("(digite um número inteiro)");
    }
}

void Pausa(string msg = "Pressione ENTER...")
{
    Console.WriteLine(msg);
    Console.ReadLine();
}
