using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Teste.Models
{
    public class Vendas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId {  get; set; }
        public int PrecoTotal { get; set; }
        public int Quantidade { get; set; }
    }
}
