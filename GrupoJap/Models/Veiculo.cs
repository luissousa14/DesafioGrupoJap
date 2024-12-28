using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GrupoJap.Models
{
    public class Veiculo
    {
        public Guid Id { get; set; }
        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Campo marca é obrigatoria.")]
        public string Marca { get; set; }
        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Campo modelo é obrigatorio.")]
        public string Modelo { get; set; }
        [Display(Name = "Matricula")]
        [Required(ErrorMessage = "Campo matricula é obrigatorio.")]
        public string Matricula { get; set; }
        [Display(Name = "Ano Fabrico")]
        [Required(ErrorMessage = "Campo ano de fabrico é obrigatorio.")]
        public int AnoFabrico { get; set; }
        [Display(Name = "Estado")]
        public bool Estado { get; set; }
        [Display(Name = "Tipo Combustivel")]
        [Required(ErrorMessage = "Campo tipo de Combustivel é obrigatorio.")]
        public Guid TipoCombustivelId { get; set; }
        [ValidateNever]
        public TipoCombustivel TipoCombustivel { get; set; }
        [ValidateNever]
        public ICollection<ContratoAluguer> Contratos { get; set; }
    }
}
