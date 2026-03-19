using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoConcessionaria.Models
{
    public class Veiculo
    {
        [Key]
        [Column("id_veiculo")]
        public int Id { get; set; }

        [Required]
        public string Marca { get; set; } = null!;

        [Required]
        public string Modelo { get; set; } = null!;
        public int Ano { get; set; }

        public string? Cor { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        // Relação com vendas
        public ICollection<Vendas>? Vendas { get; set; }
    }
}
