using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GrupoJap.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "Campo nome completo é obrigatorio.")]
        public string NomeCompleto { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Campo email é obrigatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Por favor, insira um email válido.")]
        public string Email { get; set; }
        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Campo telefone é obrigatorio.")]
        [RegularExpression(@"^(9\d{8}|2[12356]\d{7})$", ErrorMessage = "Por favor, insira um número de telefone válido em Portugal.")]
        public string Telefone { get; set; }
        [Display(Name = "Carta Condução")]
        [Required(ErrorMessage = "Campo carta de condução é obrigatorio.")]
        public bool CartaConducao { get; set; }
        [ValidateNever]
        public ICollection<ContratoAluguer> Contratos { get; set; }
    }
}
