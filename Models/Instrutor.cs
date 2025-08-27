namespace AlphaFit.Models;

public class Instrutor
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Especialidade { get; set; }

    public Instrutor(string nome, string especialidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");

        Nome = nome.Trim();
        Especialidade = string.IsNullOrWhiteSpace(especialidade)
            ? "Geral"
            : especialidade.Trim();
    }

    // Fábrica de treinos (separação de responsabilidades)
    public Treino CriarTreino(string nomeTreino, NivelTreino nivel, IEnumerable<string>? exercicios = null)
        => new Treino(nomeTreino, nivel, this, exercicios);
}
