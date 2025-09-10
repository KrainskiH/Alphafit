using AlphaFit.Models;

var builder = WebApplication.CreateBuilder(args);

// (opcional, mas recomendado p/ consumir via alunos.html no Live Server)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()
));

var app = builder.Build();
app.UseCors();

/* ===== DADOS EM MEMÓRIA (SEED) ===== */
List<Aluno> alunos = new();
List<Instrutor> instrutores = new();
List<Treino> treinos = new();

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

/* ===== ENDPOINTS ===== */

// Health-check
app.MapGet("/", () => Results.Ok(new { api = "AlphaFit Minimal API", status = "ok" }));

// ---------- ALUNOS ----------
app.MapGet("/alunos", () => Results.Ok(alunos));

app.MapGet("/alunos/{email}", (string email) =>
{
    var a = alunos.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    return a is null ? Results.NotFound() : Results.Ok(a);
});

app.MapPost("/alunos", (AlunoDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
        return Results.BadRequest(new { erro = "Nome e Email são obrigatórios." });

    if (alunos.Any(a => a.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
        return Results.Conflict(new { erro = "Já existe aluno com esse email." });

    var novo = new Aluno(dto.Nome, dto.Email);
    if (dto.Plano is not null) novo.Matricular(dto.Plano.Value);

    alunos.Add(novo);
    return Results.Created($"/alunos/{novo.Email}", novo);
});

app.MapPut("/alunos/{email}", (string email, AtualizarAlunoDto dto) =>
{
    var a = alunos.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    if (a is null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(dto.Nome)) a.Nome = dto.Nome!;
    if (dto.Plano is not null) a.Matricular(dto.Plano.Value);

    return Results.Ok(a);
});

app.MapDelete("/alunos/{email}", (string email) =>
{
    var a = alunos.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    if (a is null) return Results.NotFound();
    alunos.Remove(a);
    return Results.NoContent();
});

// atribuir treino ao aluno
app.MapPost("/alunos/{email}/treino/{treinoNome}", (string email, string treinoNome) =>
{
    var a = alunos.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    var t = treinos.FirstOrDefault(x => x.Nome.Equals(treinoNome, StringComparison.OrdinalIgnoreCase));
    if (a is null || t is null) return Results.NotFound(new { erro = "Aluno ou treino não encontrado." });

    a.AtribuirTreino(t);
    return Results.Ok(a);
});

// ---------- INSTRUTORES ----------
app.MapGet("/instrutores", () => Results.Ok(instrutores));

app.MapPost("/instrutores", (InstrutorDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Especialidade))
        return Results.BadRequest(new { erro = "Nome e Especialidade são obrigatórios." });

    var novo = new Instrutor(dto.Nome, dto.Especialidade);
    instrutores.Add(novo);
    return Results.Created($"/instrutores/{novo.Nome}", novo);
});

// ---------- TREINOS ----------
app.MapGet("/treinos", () => Results.Ok(treinos));

app.MapPost("/treinos", (CriarTreinoDto dto) =>
{
    var instr = instrutores.FirstOrDefault(i => i.Nome.Equals(dto.InstrutorNome, StringComparison.OrdinalIgnoreCase));
    if (instr is null) return Results.NotFound(new { erro = "Instrutor não encontrado." });

    var t = instr.CriarTreino(dto.Nome, dto.Nivel, dto.Exercicios ?? Array.Empty<string>());
    treinos.Add(t);
    return Results.Created($"/treinos/{t.Nome}", t);
});

app.MapDelete("/treinos/{nome}", (string nome) =>
{
    var t = treinos.FirstOrDefault(x => x.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
    if (t is null) return Results.NotFound();
    treinos.Remove(t);
    return Results.NoContent();
});

app.Run();

/* ===== DTOs ===== */
public record AlunoDto(string Nome, string Email, Plano? Plano);
public record AtualizarAlunoDto(string? Nome, Plano? Plano);
public record InstrutorDto(string Nome, string Especialidade);
public record CriarTreinoDto(string Nome, NivelTreino Nivel, string InstrutorNome, string[]? Exercicios);

/*codigo criação do banco*/
using System;

class Program
{
    static void Main()
    {
        using var contexto = new MeuDbContext();

        // Criar um usuário
        var usuario = new Usuario { Nome = "João", Email = "joao@example.com" };
        contexto.Usuarios.Add(usuario);
        contexto.SaveChanges();

        // Buscar todos os usuários
        var usuarios = contexto.Usuarios.ToList();
        foreach (var u in usuarios)
        {
            Console.WriteLine($"{u.Id}: {u.Nome} - {u.Email}");
        }
    }
}
