using System.ComponentModel.DataAnnotations;

namespace GrupoJap.Models
{
    public class TipoCombustivel
    {
        public Guid Id { get; set; }
        [Display(Name = "Tipo Combustivel")]
        public string Descritivo { get; set; }
    }
}
