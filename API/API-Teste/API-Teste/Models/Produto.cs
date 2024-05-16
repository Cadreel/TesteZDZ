using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Teste.Models
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CategoriaId { get; set; }

        public string Nome { get; set; }

        public string Unidade { get; set; }

        public decimal? Preco { get; set; }

        public int? QuantidadeEstoque { get; set; }
    }
}
