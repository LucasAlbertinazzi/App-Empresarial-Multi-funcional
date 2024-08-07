namespace AppEmpresa.Classes.API.Auditoria
{
    public class CarregamentoClass
    {
        public class Veiculos
        {
            public int Codigo { get; set; }
            public string? Veiculo { get; set; }
        }

        public class ListaCarregamento
        {
            public short? Quantidade { get; set; }
            public int? Volume { get; set; }
            public int? MaxVolume { get; set; }
            public string? CodProduto { get; set; }
            public string? Descricao { get; set; }
            public long CodigoRomaneio { get; set; }
        }
    }
}
