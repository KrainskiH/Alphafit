namespace AlphaFit.Models;

public class Aluno
{
    // Encapsulamento: set privado onde faz sentido
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Email { get; private set; }
    public Plano? PlanoAtual { get; private set; }
    public Treino? TreinoAtual { get; private set; }

    // Construtor com validação
    public Aluno(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new ArgumentException("Email inválido.");

        Nome = nome.Trim();
        Email = email.Trim();
    }

    public void Matricular(Plano plano) => PlanoAtual = plano;

    public void AtribuirTreino(Treino treino) => TreinoAtual = treino;

    public void Apresentar()
        => Console.WriteLine($"Aluno: {Nome} ({Email}) | Id={Id} | Plano={PlanoAtual?.ToString() ?? "—"}");
}
