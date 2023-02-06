namespace IzvidaciWebApp.Domain.Models;
    public class PredavacNaEdukaciji
    {
        private int _idClan;
        private int _idPredavac;

        public PredavacNaEdukaciji(int idPredavac, int idClan)
        {
            _idClan = idClan;
            _idPredavac = idPredavac;
        }

        public int idClan { get => _idClan; set => _idClan = value; }
        public int idPredavac { get => _idPredavac; set => _idPredavac = value; }


}
