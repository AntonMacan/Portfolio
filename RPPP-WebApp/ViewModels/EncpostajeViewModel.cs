namespace RPPP_WebApp.ViewModels
{

    /// <summary>
    /// Model više ENC postaja
    /// </summary>
    public class EncpostajeViewModel
    {
        public IEnumerable<EncpostajaViewModel> Encpostaje { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
