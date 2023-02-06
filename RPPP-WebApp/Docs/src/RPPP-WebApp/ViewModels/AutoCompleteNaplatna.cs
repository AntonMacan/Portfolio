using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace RPPP_WebApp.ViewModels
{
    public class AutoCompleteNaplatna
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("geoduzina")]
        public double GeoDuzina { get; set; }

        [JsonPropertyName("geosirina")]
        public double GeoSirina { get; set; }
        public AutoCompleteNaplatna() { }
        public AutoCompleteNaplatna(int id, string label, double geoduzina, double geosirina)
        {
            Id = id;
            Label = label;
            GeoDuzina = geoduzina;
            GeoSirina = geosirina;
        }
    }
}
