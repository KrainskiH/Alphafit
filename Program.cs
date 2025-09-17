using AlphaFit.Models;
using AlphaFit.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- EF Core + SQLite (usa appsettings.json) ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// ---------- CORS (útil para front rodando no Live Server) ----------
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
     .AllowAnyHeader()
     .AllowAnyMethod()
));

var app = builder.Build();
app.UseCors();

// Health-check
app.MapGet("/", () => Results.Ok(new { api = "AlphaFit Minimal API", status = "ok" }));

// ===================================================
// ===============   CRUD: ALUNOS   ==================
// ===================================================
app.MapGet("/alunos", async (AppDbContext db) =>
    Results.Ok(await db.Alunos.ToListAsync()));

app.MapGet("/alunos/{email}", async (string email, AppDbContext db) =>
{
    var a = await db.Alunos.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    return a is null ? Results.NotFound() : Results.Ok(a);
});

app.MapPost("/alunos", async (AlunoDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
        return Results.BadRequest(new { erro = "Nome e Email são obrigatórios." });

    var existe = await db.Alunos.AnyAsync(a => a.Email.ToLower() == dto.Email.ToLower());
    if (existe) return Results.Conflict(new { erro = "Já existe aluno com esse email." });

    var novo = new Aluno(dto.Nome, dto.Email);
    if (dto.Plano is not null) novo.Matricular(dto.Plano.Value);

    await db.Alunos.AddAsync(novo);
    await db.SaveChangesAsync();

    return Results.Created($"/alunos/{novo.Email}", novo);
});

app.MapPut("/alunos/{email}", async (string email, AtualizarAlunoDto dto, AppDbContext db) =>
{
    var a = await db.Alunos.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    if (a is null) return Results.NotFound();

    if (!string.IsNullOrWhiteSpace(dto.Nome)) a.Nome = dto.Nome!;
    if (dto.Plano is not null) a.Matricular(dto.Plano.Value);

    await db.SaveChangesAsync();
    return Results.Ok(a);
});

app.MapDelete("/alunos/{email}", async (string email, AppDbContext db) =>
{
    var a = await db.Alunos.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    if (a is null) return Results.NotFound();

    db.Alunos.Remove(a);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ===================================================
// ============   CRUD: INSTRUTORES   ================
// ===================================================
app.MapGet("/instrutores", async (AppDbContext db) =>
    Results.Ok(await db.Instrutores.ToListAsync()));

app.MapPost("/instrutores", async (InstrutorDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Especialidade))
        return Results.BadRequest(new { erro = "Nome e Especialidade são obrigatórios." });

    var novo = new Instrutor(dto.Nome, dto.Especialidade);
    await db.Instrutores.AddAsync(novo);
    await db.SaveChangesAsync();
    return Results.Created($"/instrutores/{novo.Nome}", novo);
});

// ===================================================
// ================   CRUD: TREINOS   =================
// ===================================================
app.MapGet("/treinos", async (AppDbContext db) =>
    Results.Ok(await db.Treinos.ToListAsync()));

app.MapPost("/treinos", async (CriarTreinoDto dto, AppDbContext db) =>
{
    var instr = await db.Instrutores.FirstOrDefaultAsync(i => i.Nome.ToLower() == dto.InstrutorNome.ToLower());
    if (instr is null) return Results.NotFound(new { erro = "Instrutor não encontrado." });

    var t = instr.CriarTreino(dto.Nome, dto.Nivel, dto.Exercicios ?? Array.Empty<string>());
    await db.Treinos.AddAsync(t);
    await db.SaveChangesAsync();
    return Results.Created($"/treinos/{t.Nome}", t);
});

app.MapDelete("/treinos/{nome}", async (string nome, AppDbContext db) =>
{
    var t = await db.Treinos.FirstOrDefaultAsync(x => x.Nome.ToLower() == nome.ToLower());
    if (t is null) return Results.NotFound();
    db.Treinos.Remove(t);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

/* ===== DTOs ===== */
public record AlunoDto(string Nome, string Email, Plano? Plano);
public record AtualizarAlunoDto(string? Nome, Plano? Plano);
public record InstrutorDto(string Nome, string Especialidade);
public record CriarTreinoDto(string Nome, NivelTreino Nivel, string InstrutorNome, string[]? Exercicios);