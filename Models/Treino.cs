namespace AlphaFit.Models;

// Outro enum da lista de conteúdos
public enum NivelTreino { Iniciante = 1, Intermediario = 2, Avancado = 3 }

public class Treino
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public NivelTreino Nivel { get; private set; }
    public List<string> Exercicios { get; } = new();
    public Instrutor Responsavel { get; }

    public Treino(string nome, NivelTreino nivel, Instrutor responsavel, IEnumerable<string>? exerciciosIniciais = null)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome do treino é obrigatório.");
        Nome = nome.Trim();
        Nivel = nivel;
        Responsavel = responsavel ?? throw new ArgumentNullException(nameof(responsavel));
        if (exerciciosIniciais is not null) Exercicios.AddRange(exerciciosIniciais);
    }

    public void AdicionarExercicio(string descricao)
    {
        if (!string.IsNullOrWhiteSpace(descricao))
            Exercicios.Add(descricao.Trim());
    }

    public override string ToString()
        => $"{Nome} ({Nivel}) • {Exercicios.Count} exercícios • Instrutor: {Responsavel.Nome}";
}
